using UnityEngine;

public class EBAttackState : BossState
{
    //private bool hasFired;
    private readonly float fireDelay = 0.3f;

    public EBAttackState(BaseBoss boss, BossStateMachine sm)
      : base(boss, sm, "Attack")
    {
    }

    public override void Enter()
    {
        base.Enter();
        //hasFired = false;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer >= fireDelay)
        {
            //(boss as ElliniaBoss)?.FireProjectile();
            //hasFired = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}