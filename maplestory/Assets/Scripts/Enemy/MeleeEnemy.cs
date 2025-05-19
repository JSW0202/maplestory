using UnityEngine;

public class MeleeEnemy : Enemy
{
    private float attackTimer;

    protected override void Start()
    {
        base.Start();

        if (balrog == null)
            balrog = GameObject.FindWithTag("Player")?.transform;

        attackTimer = 0;
    }

    protected override void Update()
    {
        base.Update();
        attackTimer -= Time.deltaTime;

        // Null 또는 Destroy 체크 먼저
        if (balrog == null)
            return;

        if (stateMachine.currentState == attackState)
            return;

        attackTimer -= Time.deltaTime;

        // 현재 상태가 공격 중이 아닐 때만 조건 검사
        bool isIdleOrMove = stateMachine.currentState == idleState || stateMachine.currentState == moveState;
        float dist = Vector2.Distance(transform.position, balrog.position);

        if (isIdleOrMove && dist <= attackRange && attackTimer <= 0f)
        {
            attackTimer = attackCooldown;
            stateMachine.ChangeState(attackState);
        }
    }
}
