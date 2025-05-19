using UnityEngine;

// Enemy가 발록을 공격하는 상태
public class EnemyAttackState : EnemyState
{
    private bool hasFired = false; // 공격 1회만 발사되도록 체크

    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.ZeroVelocity();
        // 애니메이션 파라미터 설정
        enemy.anim.SetBool("Move", false);   // 걷기 끄기
        enemy.anim.SetBool("Attack", true);  // 공격 켜기

        enemy.ZeroVelocity();                // 이동 멈추기
        enemy.lastTimeAttacked = Time.time;  // 쿨다운 시간 기록

        hasFired = false;                    // 공격 여부 초기화
        stateTimer = enemy.attackDelayTime;  // 공격 애니 시간만큼 대기
    }

    public override void Update()
    {
        base.Update();

        // 공격 중에도 항상 발록 방향을 바라보도록 Flip 처리
        if (enemy.balrog != null)
        {
            float xDiff = enemy.balrog.position.x - enemy.transform.position.x;
            int dir = xDiff > 0 ? 1 : -1;

            if ((dir == 1 && !enemy.IsFacingRight) || (dir == -1 && enemy.IsFacingRight))
            {
                enemy.Flip(); // 방향 전환
            }
        }

        // 공격 중간 타이밍에 딱 한 번만 투사체 발사
        if (!hasFired && stateTimer <= enemy.attackDelayTime * 0.5f)
        {
            PerformAttack(); // 발사체 또는 근접 공격
            hasFired = true;
        }

        // 대기 끝나면 다시 Battle 상태로 복귀
        if (stateTimer <= 0f)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    private void PerformAttack()
    {
        enemy.PerformAttack(); // Enemy.cs에서 직업별 공격 처리
    }

    public override void Exit()
    {
        base.Exit();

        // 애니메이션 파라미터 리셋
        enemy.anim.SetBool("Attack", false);
    }
}
