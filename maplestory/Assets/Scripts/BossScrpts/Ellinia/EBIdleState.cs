using UnityEngine;

public class EBIdleState : BossState
{
    public EBIdleState(BaseBoss boss, BossStateMachine sm)
      : base(boss, sm, "Idle")
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Idle 진입 시 초기화 로직 있으면 추가
    }

    public override void Update()
    {
        base.Update();
        // 전이는 모두 SetupStateMachine() 에 등록되어 있으므로 여기선 없음
    }

    public override void Exit()
    {
        base.Exit();
    }
}