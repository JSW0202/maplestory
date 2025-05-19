using UnityEngine;

    public class DDeathState : BossState
{
    private Darkload DBoss;
    public DDeathState(Darkload boss, BossStateMachine sm, string animBoolName)
      : base(boss, sm, animBoolName) { 
        DBoss = boss;
      }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("DDeathState Enter called");
        DBoss.Die();
    }

    public override void Update()
    {
        // 사망 애니 끝난 뒤 완전 제거 로직 등 추가 가능
    }

    public override void Exit()
    {
        base.Exit();
    }
}