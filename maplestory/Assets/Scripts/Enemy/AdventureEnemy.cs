using UnityEngine;

// 각 적 프리팹에 붙여서 직업 구분
public class AdventureEnemy : MonoBehaviour
{
    // 프리팹에 스크립트 붙이고
    // 이 적의 직업 설정(인스펙터에서), jopType을 Warrio, Archer, Mage 중에 선택
    public AdventureClass jobType;
    public int expValue;        // 발록이 죽었을 때 얻는 경험치
}
