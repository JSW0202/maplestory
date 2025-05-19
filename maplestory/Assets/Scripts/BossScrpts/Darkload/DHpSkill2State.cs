using UnityEngine;

    public class DHpSkill2State : BossState
{
    private Darkload DBoss;
    private readonly float duration = 3f;

    public DHpSkill2State(Darkload boss, BossStateMachine sm, string animBoolName)
      : base(boss, sm, animBoolName)
    {
        DBoss = boss;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer >= duration)
            stateMachine.ChangeState(DBoss.idle);
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}