using UnityEngine;

    public class DChainState : BossState
{
    private Darkload DBoss;
    private float duration = 3f;
    private bool chainShoot = true;
    public DChainState(Darkload boss, BossStateMachine sm, string animBoolName)
      : base(boss, sm, animBoolName)
    {
        DBoss = boss;
    }

        public override void Enter()
    {
        base.Enter();
        DBoss.active = false;
        Debug.Log("ChainState");
        var darkload = (Darkload)boss;
            Vector2 spawnPos = new Vector2(
                darkload.transform.position.x,
                darkload.transform.position.y
                );
            GameObject.Instantiate(
                darkload.motion2Prefab,
                spawnPos,
                Quaternion.identity
            );
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer >= 1.5f && chainShoot == true)
        {
            // Darkload로 캐스팅
            var darkload = (Darkload)boss;
            chainShoot = false;
            Vector2 spawnPos = new Vector2(
                darkload.playerTransform.position.x,
                darkload.playerTransform.position.y + 10f
                );
            // 프리팹 소환
            GameObject.Instantiate(
                darkload.chainPrefab,
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
        chainShoot = true;
        DBoss.active = true;
        base.Exit();
    }
}
