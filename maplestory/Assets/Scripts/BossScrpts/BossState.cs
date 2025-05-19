using UnityEngine;

/// <summary>
/// 상태 객체 패턴용 추상 클래스.
/// Enter/Update/Exit 루틴과 애니메이터 Bool 파라미터 토글까지 제공.
/// </summary>
public abstract class BossState
{
    protected BaseBoss boss;
    protected BossStateMachine stateMachine;
    protected string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    public BossState(BaseBoss boss, BossStateMachine stateMachine, string animBoolName)
    {
        this.boss         = boss;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    /// <summary>상태 진입 시 한 번 호출</summary>
    public virtual void Enter()
    {
        triggerCalled = false;
        stateTimer    = 0f;
        if (boss != null && boss.anim != null)
        {
            boss.anim.SetBool(animBoolName, true);
            boss.anim.SetTrigger(animBoolName);
        }
    }

    /// <summary>상태 유지 중 매 프레임 호출</summary>
    public virtual void Update()
    {
        stateTimer += Time.deltaTime;
    }

    /// <summary>상태 종료 시 한 번 호출</summary>
    public virtual void Exit()
    {
        if (boss != null && boss.anim != null)
        {
            boss.anim.SetBool(animBoolName, false);
        }
    }
}