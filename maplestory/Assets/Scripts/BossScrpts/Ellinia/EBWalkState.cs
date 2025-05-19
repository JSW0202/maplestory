using UnityEngine;

public class EBWalkState : BossState
{
    public EBWalkState(BaseBoss boss, BossStateMachine sm)
      : base(boss, sm, "Walk")
    {
    }

    public override void Enter()
    {
        base.Enter();
        //boss.StartMoving();
    }

    public override void Update()
    {
        base.Update();
        //boss.UpdateMovement();
    }

    public override void Exit()
    {
        base.Exit();
        //boss.StopMoving();
    }
}