using UnityEngine;

public class EBTimeSkill2State : BossState
{
    private readonly BossState idleState;
    private readonly float duration = 2.5f;

    public EBTimeSkill2State(BaseBoss boss, BossStateMachine sm, BossState idleState)
      : base(boss, sm, "TimeSkill2")
    {
        this.idleState = idleState;
    }

    public override void Enter()
    {
        base.Enter();
        // (boss as ElliniaBoss)?.FireProjectile(); // 필요 시 활성화
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