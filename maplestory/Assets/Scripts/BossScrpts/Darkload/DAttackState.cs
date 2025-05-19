using UnityEngine;

public class DAttackState : BossState
{
    private Darkload DBoss;
    private float attackDuration = 2f;
    private bool attack = true;

    public DAttackState(Darkload boss, BossStateMachine sm, string animBoolName)
      : base(boss, sm, animBoolName)
    {
        DBoss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        DBoss.active = false;
        Debug.Log("AttackState");
        var darkload = (Darkload)boss;
            Vector2 spawnPos = new Vector2(
                darkload.transform.position.x,
                darkload.transform.position.y+1
                );
            GameObject.Instantiate(
                darkload.motion1Prefab,
                spawnPos,
                Quaternion.identity
            );
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer >= 1 && attack == true)
        {
            // Darkload로 캐스팅
            var darkload = (Darkload)boss;
            attack = false;
            Vector2 spawnPos = new Vector2(
                darkload.transform.position.x,
                darkload.transform.position.y
                );
            // 프리팹 소환
            GameObject.Instantiate(
                darkload.furyPrefab,
                spawnPos,
                Quaternion.identity
            );
        }
        if (stateTimer >= attackDuration)
        {
            stateMachine.ChangeState(DBoss.idle);
        }
    }

    public override void Exit()
    {
        DBoss.active = true;
        attack = true;
        base.Exit();
    }
}