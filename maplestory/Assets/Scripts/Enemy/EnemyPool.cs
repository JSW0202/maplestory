using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [System.Serializable]
    public class PoolConfig
    {
        public AdventureClass jobType;
        public GameObject prefab;
        public int initialSize = 20;
    }

    public List<PoolConfig> poolConfigs;
    private Dictionary<AdventureClass, Queue<GameObject>> poolDict = new();

    private void Awake()
    {
        Instance = this;

        foreach (var config in poolConfigs)
        {
            var queue = new Queue<GameObject>();

            for (int i = 0; i < config.initialSize; i++)
            {
                GameObject obj = Instantiate(config.prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            poolDict[config.jobType] = queue;
        }
    }

    public GameObject Get(AdventureClass job)
    {
        if (!poolDict.ContainsKey(job))
        {
            Debug.LogWarning($"[EnemyPool] {job} 풀 없음");
            return null;
        }

        var queue = poolDict[job];
        if (queue.Count == 0)
        {
            Debug.LogWarning($"[EnemyPool] {job} 풀 비어 있음!");
            return null; // 절대 Instantiate 하지 않음
        }

        GameObject obj = queue.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj, AdventureClass job)
    {
        obj.SetActive(false);

        if (!poolDict.ContainsKey(job))
        {
            Debug.LogWarning($"[EnemyPool] {job} 풀 없음 → 제거");
            Destroy(obj);
            return;
        }

        poolDict[job].Enqueue(obj);
    }
}
