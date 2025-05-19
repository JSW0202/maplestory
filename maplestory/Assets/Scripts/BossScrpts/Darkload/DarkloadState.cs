using UnityEngine;

public class DarkloadState : MonoBehaviour
{
    /// <summary>
    /// 공격 애니메이션에서, 총알이 나가야 할 프레임에 Animation Event로 설정하세요.
    /// </summary>
    public void OnAttackFrame()
    {
        var boss = GetComponent<Darkload>();
        //boss?.FireProjectile();
    }

    /// <summary>
    /// 스킬1 애니메이션에서 투사체가 발사될 프레임에 Animation Event로 설정하세요.
    /// </summary>
    public void OnSkill1Frame()
    {
        var boss = GetComponent<Darkload>();
        //boss?.FireProjectile();
    }

    // 필요하다면 OnHpSkill1Frame(), OnTimeSkill2Frame() 등 다른 프레임 콜백도 추가!
}