using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// Ellinia 보스 전용 클래스.
/// BaseBoss의 공통 로직을 물려받고,
/// 자신의 감지/공격 범위, 투사체 설정, Override Controller 적용까지 담당.
/// </summary>
public class Darkload : BaseBoss
{
    [Header("투사체 설정")]
    [SerializeField] public GameObject chainPrefab;
    [SerializeField] public GameObject darkFlarePrefab;
    [SerializeField] public GameObject omenPrefab;
    [SerializeField] public GameObject furyPrefab;
    [SerializeField] public GameObject motion1Prefab;
    [SerializeField] public GameObject motion2Prefab;
    [SerializeField] public GameObject motion3Prefab;
    [SerializeField] public GameObject motion4Prefab;
    [SerializeField] private Transform firePoint;
   

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float ChainTime = 10f;
    private float ChainTimer = 0f;
    [SerializeField] private float AttackTime = 3f;
    private float AttackTimer = 0f;
    [SerializeField] private float TeleportTime = 5f;
    private float TeleportTimer = 0f;

    public Transform playerTransform;

    public DIdleState idle;
    public DWalkState walk;
    public DAttackState attack;
    public DChainState chain;
    //public DHpSkill1State hp1;
    //public DHpSkill2State hp2;
    public DTimeSkill1State t1;
    public DTimeSkill2State t2;
    public DDeathState deathState;
    public DTeleportState teleport;

    [Header("텔레포트 설정")]
    [SerializeField] private float teleportDistance = 5f; // 텔레포트 거리 임계값

    private void Start()
    {
        var balrog = FindObjectOfType<Balrog>();
        if (balrog != null)
            playerTransform = balrog.transform;
        else
            Debug.LogError("씬에 Balrog 컴포넌트가 붙은 오브젝트가 없습니다!");
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChainTimer = 0f; // 타이머 초기화
        TeleportTimer = 0f;
    }

    /// <summary>
    /// 스킬 전이·기본 전이를 모두 등록
    /// </summary>
    protected override void SetupStateMachine()
    {
        // 1) 상태 인스턴스 생성
        idle   = new DIdleState(this, stateMachine,"Idle");
        walk   = new DWalkState(this, stateMachine,"Walk");
        attack = new DAttackState(this, stateMachine,"Attack");
        chain  = new DChainState(this, stateMachine,"Chain");
        //hp1    = new DHpSkill1State(this, stateMachine,"Hp1");       
        //hp2    = new DHpSkill2State(this, stateMachine,"Hp2");       
        t1     = new DTimeSkill1State(this, stateMachine,"Time1");     
        t2     = new DTimeSkill2State(this, stateMachine,"Time2");
        deathState = new DDeathState(this, stateMachine,"Death");
        teleport = new DTeleportState(this, stateMachine,"Teleport");

        // 이들 인스턴스를 전이 테이블에 등록
        stateMachine.AddAnyTransition(deathState,    () => currentHp <= 0);
        stateMachine.AddTransition(chain,            () => ShouldChainSkill());
        //stateMachine.AddTransition(hp2,           () => ShouldHpSkill2());
        //stateMachine.AddTransition(hp1,           () => ShouldHpSkill1());
        stateMachine.AddTransition(t2,            () => ShouldTimeSkill2());
        stateMachine.AddTransition(t1,            () => ShouldTimeSkill1());
        stateMachine.AddTransition(teleport,         () => ShouldTeleport());
        stateMachine.AddTransition(attack,           () => ShouldAttack());

        stateMachine.SetInitialState(idle);
    }

    private void Update()
    {
        base.Update();  // 부모 클래스의 Update 호출
        ChainTimer += Time.deltaTime;
        AttackTimer += Time.deltaTime;
        TeleportTimer += Time.deltaTime;

        // 플레이어 방향으로 보기
        if (playerTransform != null)
        {
            // 플레이어가 보스의 왼쪽에 있으면 왼쪽을 보고, 오른쪽에 있으면 오른쪽을 봄
            spriteRenderer.flipX = playerTransform.position.x < transform.position.x;
        }
        if(currentHp <= 0){
            Destroy(gameObject,1.6f);
        }
    }

    private bool ShouldTeleport()
    {
        if (playerTransform == null) return false;

        float distanceX = Mathf.Abs(transform.position.x - playerTransform.position.x);
        float distanceY = Mathf.Abs(transform.position.y - playerTransform.position.y);
        if (distanceX >= teleportDistance && TeleportTimer >= TeleportTime)
        {
            TeleportTimer = 0f;
            return true;
        }
        else if (distanceY >= 3f)
        {
            TeleportTimer = 0f;
            return true;
        }
        else return false;
    }

    private bool ShouldChainSkill(){
        if(ChainTimer >= ChainTime && active){
            ChainTimer = 0f;
            return true;
        }
        else return false;
    }        

    public void TeleportStart()
    {
        Vector2 playerPos = playerTransform.position;
        // 플레이어가 0,0 기준 왼쪽에 있으면 오른쪽으로, 오른쪽에 있으면 왼쪽으로 텔레포트
        float teleportDirection = (playerPos.x < 0) ? 2f : -2f;
        transform.position = playerPos + new Vector2(teleportDirection, -0.5f);
    }

    private bool ShouldAttack(){
            if(AttackTimer >= AttackTime && active){
            AttackTimer = 0f;
            return true;
        }
        else return false;
    }
}