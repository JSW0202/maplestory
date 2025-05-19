using UnityEngine;

public class EBTimeSkill1State : BossState
{
    private readonly BossState idleState;
    private readonly float duration = 1.5f;

    public EBTimeSkill1State(BaseBoss boss, BossStateMachine sm, BossState idleState)
      : base(boss, sm, "TimeSkill1")
    {
        this.idleState = idleState;
    }

    public override void Enter()
    {
        base.Enter();
        //(boss as ElliniaBoss)?.FireProjectile();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer >= duration)
            stateMachine.ChangeState(idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}