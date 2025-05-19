using UnityEngine;

public class StartSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public AdventureClass jobType;  // 소환할 몬스터 타입
        public Transform spawnPoint;    // 소환 위치
    }

    public SpawnData[] spawnDatas;     // 인스펙터에서 등록
    public float spawnInterval = 5f;   // 소환 주기

    private void Start()
    {
        SpawnMonsters(); // 시작 즉시 한 번 실행
        InvokeRepeating(nameof(SpawnMonsters), spawnInterval, spawnInterval); // 이후 반복 소환
    }

    private void SpawnMonsters()
    {
        foreach (var data in spawnDatas)
        {
            GameObject enemy = EnemyPool.Instance.Get(data.jobType);
            if (enemy == null || data.spawnPoint == null) continue;

            enemy.transform.position = data.spawnPoint.position;

            // EnemyPool 내부에서 Initialize() + SetActive(true) 모두 처리하므로 여기선 추가 코드 불필요
        }
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnMonsters));
        Debug.Log("[StartSpawner] 스폰 정지됨");
    }
}
