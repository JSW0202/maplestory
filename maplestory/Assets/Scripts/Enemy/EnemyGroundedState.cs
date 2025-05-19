using UnityEngine;

// Idle과 Move가 공통으로 상속받는 상태
public class EnemyGroundedState : EnemyState
{
    protected Transform balrog;

    public EnemyGroundedState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
        : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        balrog = enemy.balrog;
        
        
    }

    public override void Update()
    {
        base.Update();

       

        // 발록이 죽었거나 비활성화된 경우 → Idle 상태로 강제 전환
        if (enemy.balrog == null || !enemy.balrog.gameObject.activeInHierarchy)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        // 현재 상태가 Flee일 경우엔 감지 로직 건너뜀
        if (stateMachine.currentState == enemy.fleeState)
            return;

        if (balrog == null)
            return;

        // 발록이 감지되면 전투 상태로 전환
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, balrog.position) < enemy.detectRange)
            stateMachine.ChangeState(enemy.battleState);
    }
}
