using UnityEngine;

public class EnemyHurtState : EnemyState
{
    private float hurtDuration = 0.35f;
    private float timer;

    public EnemyHurtState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
        : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = hurtDuration;
    }

    public override void Update()
    {
        base.Update();

        timer -= Time.deltaTime;

        if (timer <= 0 && enemy.IsGroundDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
