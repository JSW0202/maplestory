using UnityEngine;

public class DTimeSkill2State : BossState
{
    private Darkload DBoss;
    private readonly float duration = 4f;
    private bool omenShoot = true;

    public DTimeSkill2State(Darkload boss, BossStateMachine sm, string animBoolName)
      : base(boss, sm, animBoolName)
    {
        DBoss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        DBoss.active = false;
        Debug.Log("TimeSkill2State");
        var darkload = (Darkload)boss;
            Vector2 spawnPos = new Vector2(
                darkload.transform.position.x,
                darkload.transform.position.y+0.1f
                );
            GameObject.Instantiate(
                darkload.motion4Prefab,
                spawnPos,
                Quaternion.identity
            );
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer >= 1.5f && omenShoot == true)
        {
            omenShoot = false;
            // Darkload로 캐스팅
            var darkload = (Darkload)boss;
            Vector2 spawnPos = new Vector2(
                darkload.playerTransform.position.x,
                darkload.playerTransform.position.y-2.5f
            );
            // 프리팹 소환
            GameObject.Instantiate(
            darkload.omenPrefab,
            spawnPos,
            Quaternion.identity
            );
        }
        if (stateTimer >= duration)
        {
            stateMachine.ChangeState(DBoss.idle);
        }
    }

    public override void Exit()
    {
        omenShoot = true;
        DBoss.active = true;
        base.Exit();
    }
}