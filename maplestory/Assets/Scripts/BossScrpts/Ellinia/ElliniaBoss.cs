using UnityEngine;

/// <summary>
/// Ellinia 보스 전용 클래스.
/// BaseBoss의 공통 로직을 물려받고,
/// 자신의 감지/공격 범위, 투사체 설정, Override Controller 적용까지 담당.
/// </summary>
public class ElliniaBoss : BaseBoss
{
    [Header("투사체 설정")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private Transform playerTransform;

    private void Start()
    {
        var balrog = FindObjectOfType<Balrog>();
        if (balrog != null)
            playerTransform = balrog.transform;
        else
            Debug.LogError("씬에 Balrog 컴포넌트가 붙은 오브젝트가 없습니다!");
    }

    /// <summary>
    /// 스킬 전이·기본 전이를 모두 등록
    /// </summary>
    protected override void SetupStateMachine()
    {
        // 1) 상태 인스턴스 생성
        var idle       = new EBIdleState(this, stateMachine);
        var walk       = new EBWalkState(this, stateMachine);
        var attack     = new EBAttackState(this, stateMachine);
        var hp1    = new EBHpSkill1State(this, stateMachine, idle);       
        var hp2    = new EBHpSkill2State(this, stateMachine, idle);       
        var t1     = new EBTimeSkill1State(this, stateMachine, idle);     
        var t2     = new EBTimeSkill2State(this, stateMachine, idle);
        var deathState = new EBDeathState(this, stateMachine);

        // 이들 인스턴스를 전이 테이블에 등록
        stateMachine.AddAnyTransition(deathState,    () => currentHp <= 0);
        stateMachine.AddAnyTransition(hp2,           () => ShouldHpSkill2());
        stateMachine.AddAnyTransition(hp1,           () => ShouldHpSkill1());
        stateMachine.AddAnyTransition(t2,            () => ShouldTimeSkill2());
        stateMachine.AddAnyTransition(t1,            () => ShouldTimeSkill1());
        //stateMachine.AddTransition(idle,   attack,   () => true);
        //stateMachine.AddTransition(attack, idle,     () => true);
        stateMachine.SetInitialState(idle);
    }

    /// <summary>
    /// 언제든 호출 가능한 공용 발사 메서드 (Death 상태면 무시)
    /// </summary>
    /*public void FireProjectile()
    {
        // Death 상태일 때는 발사 금지
        if (stateMachine.CurrentStateType == BossStateType.Death)
            return;

        if (projectilePrefab == null || firePoint == null)
            return;

        var proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        // Projectile 스크립트가 있으면 데미지 세팅
        if (proj.TryGetComponent<Projectile>(out var p))
            p.SetDamage(attackDamage);
    }*/
}