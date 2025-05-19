using UnityEngine;

// 전투 상태: 발록을 추적하거나 사정거리 안에 들면 공격
public class EnemyBattleState : EnemyGroundedState
{
    public EnemyBattleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
        : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (enemy.balrog == null) return;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.balrog == null) return;

        AdventureEnemy adv = enemy.GetComponent<AdventureEnemy>();
        if (adv == null) return;

        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.balrog.position);
        float range = enemy.rangedAttackRange;               // 원거리 사정거리
        float stopChaseRange = range * 0.9f;                 // 추적을 멈추는 범위 (조금 여유 있게)

        switch (adv.jobType)
        {
            case AdventureClass.Warrior:
            case AdventureClass.Thief:
                if (distanceToPlayer <= enemy.attackRange)
                {
                    enemy.ZeroVelocity();
                    enemy.anim.SetBool("Move", false);
                    stateMachine.ChangeState(enemy.attackState);
                }
                else
                {
                    MoveTowardPlayer();
                }
                break;

            case AdventureClass.Archer:
            case AdventureClass.Mage:
                if (enemy.ShouldFlee())
                {
                    stateMachine.ChangeState(enemy.fleeState);
                    return;
                }

                if (distanceToPlayer <= range)
                {
                    // 사정거리 안 → 정지 후 공격
                    enemy.ZeroVelocity();
                    enemy.anim.SetBool("Move", false);

                    if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
                    {
                        stateMachine.ChangeState(enemy.attackState);
                    }
                }
                else if (distanceToPlayer > range && distanceToPlayer <= stopChaseRange + 2f)
                {
                    // 살짝 멀면 추적 계속
                    MoveTowardPlayer();
                }
                else
                {
                    // 너무 멀면 추적 멈춤 (불필요한 추적 방지)
                    enemy.ZeroVelocity();
                    enemy.anim.SetBool("Move", false);
                }
                break;
        }
    }

    // 발록 쪽으로 이동하는 로직
    private void MoveTowardPlayer()
    {
        float xDiff = enemy.balrog.position.x - enemy.transform.position.x;

        if (Mathf.Abs(xDiff) < 0.1f)
        {
            enemy.ZeroVelocity();
            enemy.anim.SetBool("Move", false);
            return;
        }

        int moveDir = xDiff > 0 ? 1 : -1;

        if ((moveDir == 1 && !enemy.IsFacingRight) || (moveDir == -1 && enemy.IsFacingRight))
        {
            enemy.Flip();
        }

        enemy.anim.SetBool("Move", true);
        enemy.SetVelocity(enemy.moveSpeed * moveDir, enemy.rb.linearVelocity.y);
    }
}
