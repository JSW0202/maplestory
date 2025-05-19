using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] protected int attackPower = 10;
    [SerializeField] protected GameObject hitEffectPrefab; // 이펙트가 있을 경우

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Rigidbody2D>();
            if (player != null)
            {
                //player.TakeDamage(attackPower);
            }

            OnHit();
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            OnHit(); // 벽이나 바닥에 맞아도 사라지게
        }
    }

    protected virtual void OnHit()
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
