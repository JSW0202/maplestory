using UnityEngine;

public class BalrogState
{
    //상속받은 애들이 쓸수있어야하니까
    //프로텍티드로 스테이트 머신과 Enemy를 선언해주기
    protected BalrogStateMachine stateMachine;
    protected Balrog balrog;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;

    protected bool triggerCalled;
    private string animBoolName;

    //상태의 지속시간 설정
    protected float stateTimer;

    //상태의 구성요소를 생성자로 받아주기
    public BalrogState(Balrog _balrog, BalrogStateMachine _stateMachine, string _animBoolName)
    {
        this.balrog = _balrog;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;        
    }

    public virtual void Enter()
    {
        rb = balrog.rb;
        triggerCalled = false;
        balrog.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        balrog.anim.SetFloat("yVelocity", rb.linearVelocityY);
    }


    public virtual void Exit()
    {
        balrog.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    
}
