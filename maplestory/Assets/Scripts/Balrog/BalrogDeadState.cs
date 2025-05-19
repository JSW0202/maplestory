using UnityEngine;

public class BalrogDeadState : BalrogState
{
    public BalrogDeadState(Balrog _balrog, BalrogStateMachine _stateMachine, string _animBoolName) : base(_balrog, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        balrog.isDead = true;
        balrog.currentHp = 0;
        balrog.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
        balrog.isDead = false;
        balrog.currentHp = balrog.maxHp;        
    }    
    
    public override void Update()
    {
        base.Update();
    }
}
