using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Balrog : Entity
{
    public static Balrog instance;

    #region 컴포넌트
    private PlayerSFXManager playerSFX => GetComponent<PlayerSFXManager>();
    private MapChangeImage changeScene;
    private CapsuleCollider2D capsuleCollider2d;
    private SpriteRenderer spriteRenderer;
    #endregion 

    public bool isInPortal = false;
    public bool isInTeleport = false;
    public string targetSceneName = "";

    #region 상태머신과 상태들 선언
    public BalrogStateMachine stateMachine { get; private set; }
    public BalrogIdleState idleState { get; private set; }
    public BalrogMoveState moveState { get; private set; }
    public BalrogAttackState attackState { get; private set; }
    public BalrogDeadState deadState { get; private set; }
    public BalrogSkillState thunderSkillState { get; private set; }
    public BalrogRoarState roarSkillState { get; private set; }
    #endregion

    [Header("Player Info")]
    [SerializeField] public int level = 60;
    [SerializeField] public int exp = 0;
    [SerializeField] public int maxExp = 1000;

    [Header("Attack Effect")]
    [SerializeField] private GameObject AttackEffectPrefab;
    [SerializeField] private GameObject AttackPrefab;


    [Header("Attack Range")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckDistance;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private Transform holeCheck;

    [Header("Skill Effect")]
    [SerializeField] private GameObject teleportEffectPrefab;
    [SerializeField] private GameObject MagicCircleEffectPrefab;
    [SerializeField] private GameObject FireballPrefab;
    [SerializeField] private GameObject MeteorPrefab;
    [SerializeField] private GameObject HealPrefab;
    [SerializeField] private GameObject RoarPrefab;


    [Header("Skill Cooldown")]
    [SerializeField] private float teleportCooldown = 2f;
    [SerializeField] private float teleportTime;
    [SerializeField] private float healCooldown = 4f;
    [SerializeField] private float healTime;
    [SerializeField] public float roarCooldown = 15f;
    [SerializeField] public float roarTime;
    [SerializeField] public float meteorCooldown = 8f;
    [SerializeField] public float meteorTime;
    [SerializeField] private float darkSightDuration = 3f;
    [SerializeField] private float darkSightCooldown = 10f;
    
    private float darkSightTime;
    private bool isDarkSight = false;


    


    [Header("데미지 증가, 이속 증가, 사거리 증가")]
    public bool isSpeedUpgraded = false;
    public bool isHpUpgraded = false;
    public bool isRangeUpgraded = false;
    public bool isSharpUpgraded = false;
    public bool isWallUpgraded = false;
    public int sharpEyes = 1;
    public int ironWall = 1;

    [Header("상태이상 정보")]    
    public bool isStunned = false;
    public bool isConfusion = false;

    protected override void Awake()
    {
        base.Awake();

        if (instance != null && this != instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stateMachine = new BalrogStateMachine();

        idleState = new BalrogIdleState(this, stateMachine, "Idle");
        moveState = new BalrogMoveState(this, stateMachine, "Move");
        attackState = new BalrogAttackState(this, stateMachine, "Attack");
        deadState = new BalrogDeadState(this, stateMachine, "Dead");
        thunderSkillState = new BalrogSkillState(this, stateMachine, "Skill_Meteor");
        roarSkillState = new BalrogRoarState(this, stateMachine, "Skill_Roar");
    }

    public void DownJump()
    {
        if (IsGroundDetected())
            StartCoroutine(DisableCollider());
    }

    private IEnumerator DisableCollider()
    {
        capsuleCollider2d.enabled = false;
        rb.gravityScale = 2f;
        yield return new WaitForSeconds(0.5f);
        capsuleCollider2d.enabled = true;
        rb.gravityScale = 5f;
    }
    
    protected override void Start()
    {

        base.Start();
        changeScene = FindAnyObjectByType<MapChangeImage>();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if (isDead)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            BecomeInvinsible();
        }

        PortalAndTeleport();
        stateMachine.currentState.Update();

        #region 스킬 쿨타임
        teleportTime -= Time.deltaTime;
        healTime -= Time.deltaTime;
        roarTime -= Time.deltaTime;
        meteorTime -= Time.deltaTime;
        darkSightTime -= Time.deltaTime;
        #endregion

        if (instance.level >= 140 && isSpeedUpgraded == false)
        {
            UpgradeSpeed();
            instance.isSpeedUpgraded = true;
        }

        if(instance.level >= 155 && isRangeUpgraded == false)
        {
            instance.targetCheckDistance += 2f;
            instance.isRangeUpgraded = true;
        }

        if (instance.level >= 170 && isSharpUpgraded == false)
        {
            instance.sharpEyes = 2;
            instance.isSharpUpgraded = true;
        }

        if (instance.level >= 185 && isHpUpgraded == false)
        {
            HyperBody();
            isHpUpgraded = true;
        }

        if(instance.level >= 215 && isWallUpgraded == false)
        {
            instance.ironWall = 2;
            instance.isWallUpgraded = true;
        }
    }

    public override void GetDamage(int damage)
    {
        if (!isInvincible && currentHp > 0)
        {
            instance.currentHp -= damage / instance.ironWall;
            Debug.Log(damage + "만큼 데미지 받음");            
        }
        else
            return;
    }

    private void PortalAndTeleport()
    {
        if (isInPortal && Input.GetKeyDown(KeyCode.UpArrow))
        {
            //SceneManager.LoadScene(targetSceneName);
            SoundManager.instance.PlaySFX(SoundManager.SFXType.portal);
            changeScene.FadeOutLoadScene(targetSceneName);
            isInPortal = false;
        }

        if (isInTeleport && Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position = TeleportPos.instance.PortalSetPos();
            playerSFX.PlayerJump();

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            isInPortal = true;
            Portal portal = collision.GetComponentInParent<Portal>();
            if (portal != null)
            {
                targetSceneName = portal.GetTargetSceneName();
            }
        }

        if (collision.CompareTag("Teleport"))
        {
            isInTeleport = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            isInPortal = false;
            targetSceneName = "";
        }

        if (collision.CompareTag("Teleport"))
        {
            isInTeleport = false;
        }
    }


    public void FlipController(float _x)
    {
        if (_x < 0 && facingRight)
            Flip();
        else if (_x > 0 && !facingRight)
            Flip();
    }

    public void DarkSight()
    {
        if (darkSightTime > 0)
            return;

        StartCoroutine(DarkSightRoutine());
        darkSightTime = darkSightCooldown;
    }

    private IEnumerator DarkSightRoutine()
    {        
        isDarkSight = true;
        isInvincible = true;
        
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        
        yield return new WaitForSeconds(darkSightDuration);
        
        isDarkSight = false;
        isInvincible = false;

        // 투명도 원래대로
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }

    public void SpawnMagicCircle()
    {
        GameObject go = Instantiate(MagicCircleEffectPrefab, transform.position, Quaternion.identity);
        go.transform.localScale = new Vector3(-facingDir, 1, 1);
        Destroy(go, 0.8f);
    }

    public void SpawnMeteor()
    {
        if(meteorTime > 0)
            return;

        Collider2D[] targets = Physics2D.OverlapCircleAll(targetCheck.position, targetCheckDistance, whatIsTarget);

        foreach (Collider2D target in targets)
        {

            GameObject go = Instantiate(MeteorPrefab, target.transform.position, Quaternion.identity);

            Destroy(go, 1.5f);

            Entity enemy = target.GetComponent<Entity>();
            if (enemy != null)
                enemy.GetDamage(15 * level * sharpEyes);
        }

        instance.meteorTime = instance.meteorCooldown;
    }

    //스폰 어택 이펙트는 애니메이션에서 이벤트 추가해서 함수 처리해줬음
    public void SpawnAttackEffect()
    {
        GameObject go = Instantiate(AttackEffectPrefab, transform.position, Quaternion.identity);
        go.transform.localScale = new Vector3(-facingDir, 1, 1);
        Destroy(go, 0.8f);
    }

    //얘는 레이캐스트히트2D로 빔을 쏴서 거기에 닿은 대상을 맞춰야됨
    public void SpawnClawEffect()
    {
        Vector2 boxSize = new Vector2(targetCheckDistance, 3f);
        Vector2 boxPosition = (Vector2)targetCheck.position + (Vector2.right * facingDir * targetCheckDistance / 2);

        RaycastHit2D hit = Physics2D.BoxCast(
            boxPosition,
            boxSize,
            0,
            Vector2.right * facingDir,
            0.1f,
            whatIsTarget);

        if (hit.collider != null)
        {
            GameObject go = Instantiate(AttackPrefab, hit.transform.position, Quaternion.identity);
            Destroy(go, 1f);

            // Test
            Entity enemy = hit.collider.GetComponent<Entity>();
            if (enemy != null)
                enemy.GetDamage(15 * level * sharpEyes);
        }
    }


    public void Teleport()
    {
        if (teleportTime > 0)
            return;

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition;


        if (horizontalInput != 0)
        {
            endPosition.x += facingDir * 5;
        }
        else if (verticalInput != 0)
        {
            endPosition.y += verticalInput * 5;
        }
        else
            endPosition.x += facingDir * 5;

        // 도착 지점의 땅 체크
        RaycastHit2D hit = Physics2D.Raycast(endPosition, Vector2.down, groundCheckDistance, whatIsGround);

        if (hit.collider == null)
        {
            SpawnTeleportEffect(transform.position);
            return;
        }

        playerSFX.PlayerTeleport();
        // 텔레포트 실행
        SpawnTeleportEffect(startPosition);
        transform.position = endPosition;
        SpawnTeleportEffect(endPosition);

        instance.teleportTime = instance.teleportCooldown;

    }

    public void SpawnTeleportEffect(Vector2 position)
    {
        
        GameObject go = Instantiate(teleportEffectPrefab, position, Quaternion.identity);
        Destroy(go, 0.2f);
    }

    public void BecomeInvinsible()
    {
        if (isInvincible == false)
        {
            Debug.Log("눈도 깜짝안한다!");
            isInvincible = true;
        }

        if (isInvincible == true)
        {
            isInvincible = false;
        }
    }

    public void Roar(Vector2 position)
    {
        if(roarTime > 0)
            return;

        if (instance.currentHp > instance.maxHp / 3)
            instance.currentHp -= (instance.maxHp / 3);
        else
            return;

        GameObject go = Instantiate(RoarPrefab, position, Quaternion.identity);
        Destroy(go, 1f);

        float roarRange = targetCheckDistance * 3f;
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, roarRange, whatIsTarget);

        foreach (Collider2D target in targets)
        {
           Entity enemy = target.GetComponent<Entity>();
            if (enemy != null && enemy != this)
            {
                enemy.GetDamage(20 * level * sharpEyes);
            }
        }
    }

    public void Heal()
    {
        if (healTime < 0)
        {
            playerSFX.PlayerHeal();

            SetVelocity(0, 0);
            Vector2 healPos = transform.position;
            healPos.y -= 1.5f;
            GameObject go = Instantiate(HealPrefab, healPos, Quaternion.identity);

            instance.currentHp += instance.maxHp / 2;

            if (instance.currentHp > instance.maxHp)
                instance.currentHp = instance.maxHp;

            Destroy(go, 0.5f);
            instance.healTime = instance.healCooldown;
        }
        else
            return;
    }

    public void AddExp(int amount)
    {
        instance.exp += amount;
        Debug.Log($"발록이 {amount} 경험치를 얻었다.");

        if (instance.exp >= instance.maxExp)
        {
            LevelUp();            
        }
    }

    public void LevelUp()
    {        
        instance.level += 15;       
    }

    public void UpgradeSpeed()
    {
        instance.moveSpeed += 2.5f;
    }

    public void HyperBody()
    {
        instance.maxHp *= 2;
        instance.currentHp = instance.maxHp;
    }   

    public void StunBalrog()
    {
        StartCoroutine(StunBalrogCoroutine());
    }

    IEnumerator StunBalrogCoroutine()
    {
        isStunned = true;

        yield return new WaitForSeconds(2.5f);

        isStunned = false;
    }

    public void ConfusionBalrog()
    {
        StartCoroutine(Confusion());
    }

    IEnumerator Confusion()
    {
        isConfusion = true;
        yield return new WaitForSeconds(2f);
        isConfusion = false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckDistance);

        Gizmos.color = Color.yellow;
        Vector2 attackSize = new Vector2(targetCheckDistance, 3f);
        Vector2 attackPosition = (Vector2)targetCheck.position + (Vector2.right * facingDir * targetCheckDistance / 2);
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }

    public PlayerSFXManager GetPlayerSFXManager()
    {
        return playerSFX;
    }

}