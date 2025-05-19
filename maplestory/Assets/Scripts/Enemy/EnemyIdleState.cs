using UnityEngine;

public class EnemyIdleState : EnemyGroundedState
{
    public EnemyIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
        : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.col.enabled = true;
        enemy.rb.gravityScale = 1f;
    }


    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            if (enemy.ShouldFlee())
            {
                // Mage만 도망감
                stateMachine.ChangeState(enemy.fleeState);
            }
            else
            {
                // Warrior, Archer는 전투
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}
