using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine;

    public EnemyIdleState idleState;
    public EnemyMoveState moveState;
    public EnemyState battleState;
    public EnemyAttackState attackState;
    public EnemyHurtState hurtState;
    public EnemyFleeState fleeState;

    [Header("행동 관련")]
    public Transform balrog;
    public LayerMask whatIsPlayer;
    public float detectRange = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public float rangedAttackRange = 5f;
    public float attackDelayTime = 0.6f;

    [Header("공격력")]
    public int attackPower;

    [Header("투사체")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("근접 이펙트")]
    [SerializeField] private GameObject meleeEffectPrefab;
    [SerializeField] private Transform effectSpawnPoint;

    [Header("묘비")]
    [SerializeField] private GameObject tombPrefab;

    public bool IsFacingRight => facingRight;
    [HideInInspector] public float lastTimeAttacked;

    public GameObject damagePopupPrefab; // DamagePopup 프리팹


    protected override void Start()
    {
        base.Start();

        if (balrog == null)
            balrog = GameObject.FindWithTag("Player")?.transform;

        // 상태머신 및 각 상태 초기화
        if (stateMachine == null)
        {
            stateMachine = new EnemyStateMachine();

            idleState = new EnemyIdleState(this, stateMachine, "Idle");
            moveState = new EnemyMoveState(this, stateMachine, "Move");
            battleState = new EnemyBattleState(this, stateMachine, "Move");
            attackState = new EnemyAttackState(this, stateMachine, "Attack");
            hurtState = new EnemyHurtState(this, stateMachine, "Idle");
            fleeState = new EnemyFleeState(this, stateMachine, "Move");
        }

        stateMachine.Initialize(idleState); // Idle 상태로 시작
    }

    protected override void Update()
    {
        base.Update();
        stateMachine?.currentState?.Update();
    }

    private void OnEnable()
    {
        // 체력 및 상태 초기화
        isDead = false;
        currentHp = maxHp;

        // 애니메이션 초기화
        if (anim != null)
        {
            anim.ResetTrigger("Die");
            anim.Play("Idle", 0);
        }

        // 상태머신 초기화는 Start()에서 수행
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        ShowDamagePopup(damage);

        if (currentHp <= 0)
            Die();
        else
            stateMachine.ChangeState(idleState);
    }

    private void ShowDamagePopup(int damage)
    {
        Vector3 popupPosition = transform.position + Vector3.up * 1.5f;
        GameObject popupObj = Instantiate(damagePopupPrefab, popupPosition, Quaternion.identity);
        popupObj.GetComponent<DamagePopup>().Setup(damage);
    }

    public override void Die()
    {
        if (isDead) return;
        isDead = true;

        ZeroVelocity();
        stateMachine.Stop();

        anim.SetBool("Move", false);
        anim.SetBool("Attack", false);
        anim.ResetTrigger("Die");
        anim.SetTrigger("Die");

        AdventureEnemy adv = GetComponent<AdventureEnemy>();

        if (adv != null && Balrog.instance != null)
            Balrog.instance.AddExp(adv.expValue);

        GameManager.AddDeadEnemy();

        if (adv == null)
        {
            Debug.LogError($"[Enemy.Die] AdventureEnemy 없음! {gameObject.name} Destroy 방지용 로그");
            return;
        }

        if (tombPrefab != null)
        {
            GameObject tomb = Instantiate(tombPrefab, transform.position, Quaternion.identity);
            Tomb tombScript = tomb.GetComponent<Tomb>();
            if (tombScript != null)
                tombScript.InitAndReturn(this, adv.jobType);
        }
        else
        {
            EnemyPool.Instance.Return(gameObject, adv.jobType);
        }

        col.enabled = false;
        rb.gravityScale = 0;
    }

    public RaycastHit2D IsPlayerDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.left * facingDir, detectRange, whatIsPlayer);
    }

    public bool ShouldFlee()
    {
        if (isDead) return false;

        AdventureEnemy adv = GetComponent<AdventureEnemy>();
        if (adv == null) return false;

        return adv.jobType == AdventureClass.Mage && GameManager.deadEnemyCount >= 1;
    }

    public virtual void PerformAttack()
    {
        AdventureEnemy adv = GetComponent<AdventureEnemy>();
        if (adv == null) return;

        switch (adv.jobType)
        {
            case AdventureClass.Warrior:
            case AdventureClass.Thief:
                PerformMeleeAttack();
                break;
            case AdventureClass.Archer:
                ShootArrow();
                break;
            case AdventureClass.Mage:
                CastMagic();
                break;
        }
    }

    public void PerformMeleeAttack()
    {
        if (balrog == null) return;

        float dist = Vector2.Distance(transform.position, balrog.position);
        if (dist <= attackRange)
        {
            if (meleeEffectPrefab != null && effectSpawnPoint != null)
            {
                Quaternion rot = IsFacingRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
                GameObject fx = Instantiate(meleeEffectPrefab, effectSpawnPoint.position, rot);
                Destroy(fx, 0.4f);
            }

            Balrog br = balrog.GetComponent<Balrog>();
            if (br != null)
                br.GetDamage(attackPower);
        }
    }

    private void ShootArrow()
    {
        if (arrowPrefab == null || projectileSpawnPoint == null)
            return;

        GameObject arrow = Instantiate(arrowPrefab, projectileSpawnPoint.position, Quaternion.identity);
        Vector2 dir = (balrog.position - transform.position).normalized;
        arrow.GetComponent<ArrowProjectile>().Setup(dir, attackPower);
    }

    private void CastMagic()
    {
        if (magicPrefab == null || projectileSpawnPoint == null)
            return;

        GameObject magic = Instantiate(magicPrefab, projectileSpawnPoint.position, Quaternion.identity);
        Vector2 dir = (balrog.position - transform.position).normalized;
        magic.GetComponent<MageProjectile>().Setup(dir, attackPower);
    }
}
