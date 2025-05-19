using UnityEngine;

public enum StageName
{
    Elinia,
    Henesis,
    Perion,
    KerningCity
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isClear = false;

    public static int deadEnemyCount = 0;

    [System.Serializable]
    public class StageGoal
    {
        public StageName stage;  // 각 스테이지 이름
        public int killGoal;     // 처치 목표 수
    }

    [Header("스테이지 설정")]
    public StageGoal[] stages;

    [Header("현재 진행 중인 스테이지")]
    public StageName currentStage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환해도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    // 몬스터가 죽을 때 호출
    public static void AddDeadEnemy()
    {
        deadEnemyCount++;
        Debug.Log($"[GameManager] 현재 처치 수: {deadEnemyCount}");
        instance.CheckStageProgress();
    }

    // 현재 스테이지 목표 처치 수 도달 확인
    public void CheckStageProgress()
    {
        foreach (var stage in stages)
        {
            if (stage.stage == currentStage)
            {
                Debug.Log($"[GameManager] {currentStage} → {deadEnemyCount} / {stage.killGoal}");

                if (deadEnemyCount >= stage.killGoal)
                {
                    deadEnemyCount = 0;
                    AdvanceToNextStage();
                    isClear = true;
                }
                return;
            }
        }

        Debug.LogWarning($"[GameManager] 현재 스테이지({currentStage}) 설정이 없음!");
    }

    // 스테이지 전환 + 몹 제거
    private void AdvanceToNextStage()
    {

        Debug.Log($"[GameManager] {currentStage} 완료! 몬스터 제거 중...");
        RemoveAllEnemies();

        // 현재 스테이지의 스폰 정지
        StartSpawner spawner = Object.FindFirstObjectByType<StartSpawner>();
        if (spawner != null)
            spawner.StopSpawning();

        for (int i = 0; i < stages.Length - 1; i++)
        {
            if (stages[i].stage == currentStage)
            {
                currentStage = stages[i + 1].stage;
                Debug.Log($"[GameManager] 다음 스테이지: {currentStage}");
                return;
            }
        }

        Debug.Log($"[GameManager] 마지막 스테이지({currentStage}) 완료! 게임 클리어!");
    }


    // 모든 몬스터 제거 (풀로 반환 또는 제거)
    private void RemoveAllEnemies()
    {
        var enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            var adv = enemy.GetComponent<AdventureEnemy>();
            if (adv != null)
            {
                EnemyPool.Instance.Return(enemy.gameObject, adv.jobType);
            }
            else
            {
                Destroy(enemy.gameObject); // 풀 대상 아님
            }
        }

        Debug.Log("[GameManager] 모든 적 제거 완료");
    }
}