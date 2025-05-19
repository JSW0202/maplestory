using UnityEngine;

public class DTeleportState : BossState
{
    private Darkload DBoss;
    private float Duration = 2.0f;

    public DTeleportState(BaseBoss boss, BossStateMachine sm, string animBoolName)
      : base(boss, sm, animBoolName)
    {
        DBoss = boss as Darkload;
    }

    public override void Enter()
    {
        base.Enter();
        DBoss.active = false;
        Debug.Log("TeleportState");
    }

    public override void Update()
    {
        if(stateTimer >= Duration){
            stateMachine.ChangeState(DBoss.idle);
        }
        base.Update();
    }

    public override void Exit()
    {
        DBoss.active = true;
        base.Exit();
    }

}