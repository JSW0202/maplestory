using UnityEngine;

public class EBDeathState : BossState
{
    public EBDeathState(BaseBoss boss, BossStateMachine sm)
      : base(boss, sm, "Death") { }

    public override void Enter()
    {
        base.Enter();
        var col = boss.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        boss.rb.linearVelocity = Vector2.zero;
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