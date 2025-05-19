using UnityEngine;

public class Skill : MonoBehaviour
{
    protected float cooldown;
    protected float cooldownTimer;



    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }


    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }


    public virtual void UseSkill()
    {
       
    }
}
