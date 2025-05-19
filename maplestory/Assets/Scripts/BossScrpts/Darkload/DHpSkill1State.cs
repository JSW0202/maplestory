using UnityEngine;

    public class DHpSkill1State : BossState
{
    private Darkload DBoss;
    private readonly float duration = 2f;

    public DHpSkill1State(Darkload boss, BossStateMachine sm, string animBoolName)
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