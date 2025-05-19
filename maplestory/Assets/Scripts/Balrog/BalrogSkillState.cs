using UnityEngine;

public class BalrogSkillState : BalrogState
{
    public BalrogSkillState(Balrog _balrog, BalrogStateMachine _stateMachine, string _animBoolName) : base(_balrog, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        balrog.SetVelocity(0, 0);
        stateTimer = 1f;
        balrog.GetPlayerSFXManager().PlayerMeteor();
    }
       

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(balrog.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
