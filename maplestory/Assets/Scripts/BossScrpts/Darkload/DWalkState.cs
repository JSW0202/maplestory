using UnityEngine;

public class DWalkState : BossState
{
    public DWalkState(BaseBoss boss, BossStateMachine sm, string animBoolName)
      : base(boss, sm, animBoolName)
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