using UnityEngine;
using System.Collections;

public class BalrogIdleState : BalrogState
{
    public BalrogIdleState(Balrog _balrog, BalrogStateMachine _stateMachine, string _animBoolName)
        : base(_balrog, _stateMachine, _animBoolName){}

    public override void Enter()
    {
        base.Enter();
        balrog.ZeroVelocity();
    }   

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.P) || balrog.currentHp <= 0)
        {
            stateMachine.ChangeState(balrog.deadState);
        }
        #region 발록 기본 이동
        if (Input.GetAxisRaw("Horizontal") != 0)
            stateMachine.ChangeState(balrog.moveState);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            stateMachine.ChangeState(balrog.attackState);

        if (Input.GetKeyDown(KeyCode.LeftAlt) && balrog.IsGroundDetected())
        {
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            balrog.GetPlayerSFXManager().PlayerJump();
        }

        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Space))
        {
            balrog.DownJump();
        }
        #endregion


        #region 발록 스킬
        if (Input.GetKeyDown(KeyCode.LeftShift) && balrog.level >= 95)
        {
            balrog.Teleport();
        }

        if (Input.GetKeyDown(KeyCode.End) && balrog.level >= 110)
        {
           balrog.Heal();
        }

        if (Input.GetKeyDown(KeyCode.Z) && balrog.level >= 125 && balrog.meteorTime < 0)
        {
            stateMachine.ChangeState(balrog.thunderSkillState);
        }


        if(Input.GetKeyDown(KeyCode.X) && balrog.level >= 200 && balrog.roarTime < 0)
        {
            stateMachine.ChangeState(balrog.roarSkillState);
        }

        if (Input.GetKeyDown(KeyCode.PageDown) && balrog.level >= 260)
        {
            balrog.DarkSight();
        }       

        #endregion


    }

    public override void Exit()
    {
        base.Exit();
    }
}
