using UnityEngine;

public class EnemyFleeState : EnemyGroundedState
{
    private int fleeDir;
    private float fleeEndDistance = 10f;

    public EnemyFleeState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
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

        if (enemy.isDead)
        {
            enemy.ZeroVelocity();
            return;
        }

        if (enemy.balrog == null) return;

        float distanceToBalrog = Vector2.Distance(enemy.transform.position, enemy.balrog.position);
        if (distanceToBalrog >= fleeEndDistance)
        {
            enemy.ZeroVelocity();
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        // 도망 방향을 매 프레임 계산
        fleeDir = enemy.transform.position.x < enemy.balrog.position.x ? -1 : 1;

        if ((fleeDir == 1 && !enemy.IsFacingRight) || (fleeDir == -1 && enemy.IsFacingRight))
            enemy.Flip();

        // 벽 충돌 시 방향 반전
        if (enemy.IsWallDetected())
        {
            fleeDir *= -1;
            enemy.Flip();
        }

        enemy.SetVelocity(enemy.moveSpeed * fleeDir, enemy.rb.linearVelocity.y);
    }
}
