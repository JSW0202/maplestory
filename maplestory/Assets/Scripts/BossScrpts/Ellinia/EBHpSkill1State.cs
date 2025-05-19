using UnityEngine;

public class EBHpSkill1State : BossState
{
    private readonly BossState idleState;
    private readonly float duration = 2f;

    public EBHpSkill1State(BaseBoss boss, BossStateMachine sm, BossState idleState)
      : base(boss, sm, "HpSkill1")
    {
        this.idleState = idleState;
    }

    public override void Enter()
    {
        base.Enter();
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