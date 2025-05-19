using UnityEngine;

public class Basemotion : MonoBehaviour
{
    public virtual void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
