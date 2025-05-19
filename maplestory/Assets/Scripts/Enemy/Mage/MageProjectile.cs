using UnityEngine;

// 메이지가 발사하는 마법 투사체 스크립트
public class MageProjectile : MonoBehaviour
{
    public float speed = 8f;           // 이동 속도
    public int damage = 2;             // 공격력 (Setup에서 설정)
    public float maxLifetime = 5f;     // 자동 제거 시간

    private Vector2 direction;         // 이동 방향
    private float timer;               // 생존 시간 추적용

    private void Update()
    {
        // 지정된 방향으로 이동
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // 일정 시간 후 자동 제거
        timer += Time.deltaTime;
        if (timer >= maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    // 마법 발사 방향과 공격력 설정
    public void Setup(Vector2 _direction, int _damage)
    {
        direction = _direction.normalized;
        damage = _damage;

        // 왼쪽이 기본 스프라이트일 경우: +180도 회전 보정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 발록과 충돌했는지 확인
        if (collision.CompareTag("Player"))
        {
            Entity balrog = collision.GetComponent<Entity>();
            if (balrog != null)
            {
                balrog.GetDamage(damage); // 데미지 전달
            }

            Destroy(gameObject); // 1회 히트 후 제거
        }
    }
}
