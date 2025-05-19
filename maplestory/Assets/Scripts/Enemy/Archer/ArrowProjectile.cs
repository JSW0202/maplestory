using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    private Vector2 direction;
    private float maxLifetime = 5f;
    private float timer;

    private void Update()
    {
        // 정확한 이동 방향으로 날아가게 함
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // 시간 지나면 제거
        timer += Time.deltaTime;
        if (timer >= maxLifetime)
            Destroy(gameObject);
    }

    // 방향과 회전 모두 적용
    public void Setup(Vector2 _direction, int _damage)
    {
        direction = _direction.normalized;
        damage = _damage;

        // 왼쪽이 기본 방향인 스프라이트라면 +180도 보정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Entity balrog = collision.GetComponent<Entity>();
            if (balrog != null)
            {
                balrog.GetDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
