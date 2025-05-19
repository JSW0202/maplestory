using UnityEngine;

/// <summary>
/// 각 스테이지에 맞춰 보스를 생성/제거하고,
/// 처치될 때 매니저 쪽 로직을 실행하도록 하는 공장(Factory) 클래스.
/// </summary>
public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject ElliniaBossPrefab;
    [SerializeField] private GameObject HenesysBossPrefab;
    [SerializeField] private GameObject PerionBossPrefab;
    [SerializeField] private GameObject KerningCityBossPrefab;
    [SerializeField] private GameObject DarkloadPrefab;

    private BaseBoss currentBoss;

    /// <summary>
    /// stageNumber에 따라 알맞은 보스 프리팹을 Instantiate.
    /// 이전 보스가 있으면 Destroy.
    /// </summary>
    public BaseBoss CreateBossForStage(int stageNumber, Vector2 spawnPosition)
    {
        if (currentBoss != null)
            Destroy(currentBoss.gameObject);

        GameObject prefab = stageNumber switch
        {
            1 => ElliniaBossPrefab,
            2 => HenesysBossPrefab,
            3 => PerionBossPrefab,
            4 => KerningCityBossPrefab,
            5 => DarkloadPrefab,
            _ => null
        };

        if (prefab == null)
        {
            Debug.LogError($"[BossManager] Stage {stageNumber}에 해당하는 보스 프리팹이 없습니다.");
            return null;
        }

        var bossObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
        currentBoss = bossObj.GetComponent<BaseBoss>();
        currentBoss.Initialize(this);
        return currentBoss;
    }

    /// <summary>
    /// 보스 사망 시 호출되는 콜백
    /// </summary>
    public void OnBossDefeated(BaseBoss boss)
    {
        Debug.Log($"[BossManager] {boss.name} 처치됨.");
        FieldSkillManager.instance.StopFieldSkill();
        // 다음 스테이지 로직…
    }

    private void Start(){
        CreateBossForStage(5,new Vector2(0f, -3f));
    }
}