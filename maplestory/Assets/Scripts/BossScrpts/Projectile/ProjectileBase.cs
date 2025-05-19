using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected int attackPower = 10; // 디폴트 데미지
    protected bool hasDamaged = false;
    protected Collider2D projectileCollider;

    protected virtual void Awake()
    {
        projectileCollider = GetComponent<Collider2D>();
        if (projectileCollider == null)
        {
            Debug.LogError("Projectile에 Collider2D가 없습니다!");
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDamaged) return;

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Entity>();
            if (player != null)
            {
                player.GetDamage(attackPower);
                hasDamaged = true;
            }
        }
    }

    public virtual void SetAttackPower(int power)
    {
        attackPower = power;
    }

    // 애니메이션 이벤트에서 호출될 메서드 - Collider 크기 조절
    public virtual void UpdateColliderSize()
    {
        if (projectileCollider != null)
        {
            var sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                Vector2 spriteSize = sprite.bounds.size;
                
                if (projectileCollider is BoxCollider2D boxCollider)
                {
                    // 크기 설정
                    boxCollider.size = spriteSize;
                    // Top Center pivot에 맞게 offset 조정
                    boxCollider.offset = new Vector2(0, -spriteSize.y / 2);
                }
                else if (projectileCollider is CircleCollider2D circleCollider)
                {
                    // 크기 설정
                    circleCollider.radius = spriteSize.x / 2f;
                    // Top Center pivot에 맞게 offset 조정
                    circleCollider.offset = new Vector2(0, -spriteSize.y / 2);
                }
            }
        }
    }

    public virtual void UpdateCenterCollider()
    {
        if (projectileCollider != null)
        {
            var sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                Vector2 spriteSize = sprite.bounds.size;
                
                if (projectileCollider is BoxCollider2D boxCollider)
                {
                    // 크기 설정
                    boxCollider.size = spriteSize;
                    // Center pivot이므로 offset 불필요
                    boxCollider.offset = Vector2.zero;
                }
                else if (projectileCollider is CircleCollider2D circleCollider)
                {
                    // 크기 설정
                    circleCollider.radius = spriteSize.x / 2f;
                    // Center pivot이므로 offset 불필요
                    circleCollider.offset = Vector2.zero;
                }
            }
        }
    }
    public virtual void UpdateBottomCollider()
    {
        if (projectileCollider != null)
        {
            var sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                Vector2 spriteSize = sprite.bounds.size;
                
                if (projectileCollider is BoxCollider2D boxCollider)
                {
                    // 크기 설정
                    boxCollider.size = spriteSize;
                    // Bottom Center pivot에 맞게 offset 조정
                    boxCollider.offset = new Vector2(0, spriteSize.y / 2);
                }
                else if (projectileCollider is CircleCollider2D circleCollider)
                {
                    // 크기 설정
                    circleCollider.radius = spriteSize.x / 2f;
                    // Bottom Center pivot에 맞게 offset 조정
                    circleCollider.offset = new Vector2(0, spriteSize.y / 2);
                }
            }
        }
    }
}
