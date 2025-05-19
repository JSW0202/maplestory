using UnityEngine;

public class EnemyMoveState : EnemyGroundedState
{
    public EnemyMoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
        : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.balrog != null && GameManager.deadEnemyCount == 0)
        {
            if ((enemy.balrog.position.x > enemy.transform.position.x && !enemy.IsFacingRight)
                || (enemy.balrog.position.x < enemy.transform.position.x && enemy.IsFacingRight))
            {
                enemy.Flip();
            }
        }

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.linearVelocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
        }

        if (enemy.IsPlayerDetected())
        {
            if (enemy.ShouldFlee())
            {
                // Mage만 도망
                stateMachine.ChangeState(enemy.fleeState);
            }
            else
            {
                // Warrior, Archer는 계속 전투
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}
