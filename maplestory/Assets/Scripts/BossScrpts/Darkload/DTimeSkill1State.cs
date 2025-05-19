using UnityEngine;

public class DTimeSkill1State : BossState
{
    private Darkload DBoss;
    private float duration = 3f;
    private bool darkflareShoot = true;

    public DTimeSkill1State(Darkload boss, BossStateMachine sm, string animBoolName)
      : base(boss, sm, animBoolName)
    {
        DBoss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        DBoss.active = false;
        Debug.Log("TimeSkill1State");
        var darkload = (Darkload)boss;
            Vector2 spawnPos = new Vector2(
                darkload.transform.position.x,
                darkload.transform.position.y+0.3f
                );
            GameObject.Instantiate(
                darkload.motion3Prefab,
                spawnPos,
                Quaternion.identity
            );
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer >= 1.5f && darkflareShoot == true)
        {
            darkflareShoot = false;
            // Darkload로 캐스팅
            var darkload = (Darkload)boss;
            Vector2 spawnPos = new Vector2(
                darkload.playerTransform.position.x,
                darkload.playerTransform.position.y
            );
            // 프리팹 소환
            GameObject.Instantiate(
            darkload.darkFlarePrefab,
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
        darkflareShoot = true;
        DBoss.active = true;
        base.Exit();
    }
}