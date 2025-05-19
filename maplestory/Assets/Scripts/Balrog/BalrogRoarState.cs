using UnityEngine;

public class BalrogRoarState : BalrogState
{
    public BalrogRoarState(Balrog _balrog, BalrogStateMachine _stateMachine, string _animBoolName) : base(_balrog, _stateMachine, _animBoolName)
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
        balrog.Roar(balrog.transform.position);
        stateTimer = 1f;
        balrog.roarTime = balrog.roarCooldown;
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
