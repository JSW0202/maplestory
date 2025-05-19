
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    private Image bossHP;
    private Entity boss;
    
    void Start()
    {
        bossHP = GetComponent<Image>();
        boss = GetComponentInParent<Entity>();
    }

    void Update()
    {
        bossHP.fillAmount =  (float)boss.currentHp / (float)boss.maxHp;
    }
}
