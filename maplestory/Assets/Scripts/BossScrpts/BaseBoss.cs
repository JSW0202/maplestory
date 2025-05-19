using UnityEngine;
using System;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;

/// <summary>
/// 모든 보스가 상속받는 추상 클래스.
/// 상태 머신 초기화, 체력 관리, 공통 스킬 전이(HP/시간) 로직을 담고 있다.
/// </summary>
[RequireComponent(typeof(BossStateMachine))]
public abstract class BaseBoss : Entity
{
    // 보스 처치 시 호출되는 이벤트 (BossManager 구독)
    public event System.Action<BaseBoss> OnBossDefeated;

    public Animator anim;
    public BossStateMachine stateMachine;

    public GameObject damagePopupPrefab; // DamagePopup 프리팹

    [Header("공통 스킬 임계값")]
    [SerializeField] protected float hpSkill1Threshold = 0.7f;  // 체력 ≤ 70%
    [SerializeField] protected float hpSkill2Threshold = 0.2f;  // 체력 ≤ 20%
    [SerializeField] protected float timeSkill1Interval = 10f;  // 10초마다
    [SerializeField] protected float timeSkill2Interval = 15f;  // 30초마다
    protected bool hpSkill1 = true;
    protected bool hpSkill2 = true;
    public bool active = true;

    // 내부 타이머
    protected float timeSkill1Timer;
    protected float timeSkill2Timer;

    /// <summary>
    /// 현재 체력 비율을 0~1 로 반환
    /// </summary>
    public float HealthPercent => (float)currentHp / maxHp;

    /// <summary>
    /// BossManager가 새 보스를 만들고 바로 호출해야 하는 초기화 메서드
    /// </summary>
    public virtual void Initialize(BossManager manager)
    {
        
        //체력·타이머 초기화
        maxHp = 500000;
        currentHp = maxHp;
        timeSkill1Timer   = 0f;
        timeSkill2Timer   = 0f;

        anim = GetComponent<Animator>();
        stateMachine = GetComponent<BossStateMachine>();
        SetupStateMachine();
    
        //사망 시 매니저에 알림
        OnBossDefeated += manager.OnBossDefeated;
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        ShowDamagePopup(damage);

        if (currentHp <= 0)
            Die();
    }

    private void ShowDamagePopup(int damage)
    {
        Vector3 popupPosition = transform.position + Vector3.up * 1.5f; // 위쪽에 띄우기
        GameObject popupObj = Instantiate(damagePopupPrefab, popupPosition, Quaternion.identity);
        popupObj.GetComponent<DamagePopup>().Setup(damage);
    }

    /// <summary>
    /// 자식 클래스에서 "어떤 상태들이 있는지, 어떤 전이 규칙인지" 등록하도록 강제하는 추상 메서드
    /// </summary>
    protected abstract void SetupStateMachine();

    /// <summary>
    /// 매 프레임 호출: 공통 스킬 타이머 누적
    /// </summary>
    protected virtual void Update()
    {
        timeSkill1Timer += Time.deltaTime;
        timeSkill2Timer += Time.deltaTime;
    }

    /// <summary>
    /// 사망 처리: Death 상태 전환 + 매니저 콜백
    /// </summary>
    public override void Die()
    {
        Debug.Log("BaseBoss Die called!");
        StartCoroutine(DelayedBossDefeated());
    }

    private IEnumerator DelayedBossDefeated()
    {
        yield return new WaitForSeconds(1f);
        OnBossDefeated?.Invoke(this);  // 1초 후 보스 처치 이벤트 발생
    }

    // ─────────────────────────────────────────────────
    // 공통 스킬 조건 메서드들
    // ─────────────────────────────────────────────────

    // HP 기준
    public bool ShouldHpSkill1(){
        if (HealthPercent <= hpSkill1Threshold  && HealthPercent > hpSkill2Threshold && hpSkill1)
        {
            hpSkill1 = false;
            return true;
        }
        return false;
    }
    public bool ShouldHpSkill2(){
        if (HealthPercent <= hpSkill2Threshold && hpSkill2)
        {
            hpSkill2 = false;
            return true;
        }
        return false;
    }

    
    public bool ShouldTimeSkill1()
    {
        if (timeSkill1Timer >= timeSkill1Interval && active)
        {
            timeSkill1Timer = 0f;
            return true;
        }
        return false;
    }
    public bool ShouldTimeSkill2()
    {
        if (timeSkill2Timer >= timeSkill2Interval && active)
        {
            timeSkill2Timer = 0f;
            return true;
        }
        return false;
    }


    
}