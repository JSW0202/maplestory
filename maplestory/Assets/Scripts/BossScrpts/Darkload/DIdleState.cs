using UnityEngine;

    public class DIdleState : BossState
{
    private Darkload DBoss;
    private float duration = 2f; // idle 상태 유지 시간(초)

    public DIdleState(Darkload boss, BossStateMachine sm, string animBoolName)
      : base(boss, sm, animBoolName)
    {
        DBoss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        DBoss.active = true;
        Debug.Log("IdleState");
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer >= duration)
        {
            stateMachine.ChangeState(DBoss.attack);
        }
    }

    public override void Exit()
    {
        DBoss.active = false;
        base.Exit();
    }
}