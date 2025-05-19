using UnityEngine;

public class BalrogMoveState : BalrogState
{
    public BalrogMoveState(Balrog _balrog, BalrogStateMachine _stateMachine, string _animBoolName) : base(_balrog, _stateMachine, _animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();


        if (balrog.isStunned)
        {
            balrog.SetVelocity(0, 0);
            return;
        }
            


        balrog.SetVelocity(xInput * balrog.moveSpeed, rb.linearVelocityY);

        balrog.FlipController(xInput);

        if (xInput == 0)
            stateMachine.ChangeState(balrog.idleState);

        if (Input.GetKeyDown(KeyCode.LeftShift) && balrog.level >= 95)
            balrog.Teleport();        

        if (Input.GetKeyDown(KeyCode.LeftControl))
            stateMachine.ChangeState(balrog.attackState);

        if (Input.GetKeyDown(KeyCode.Z) && balrog.level >= 125 && balrog.meteorTime < 0)
        {
            stateMachine.ChangeState(balrog.thunderSkillState);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) && balrog.IsGroundDetected())
        {
            balrog.GetPlayerSFXManager().PlayerJump();
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            balrog.DarkSight();
        }

        if (balrog.isConfusion)
        {
            balrog.SetVelocity(-xInput * balrog.moveSpeed, rb.linearVelocityY);
            balrog.FlipController(-xInput);
        }


        
    }

    

    public override void Exit()
    {
        base.Exit();
    }
}
