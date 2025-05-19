using System.Collections;
using UnityEngine;


public class OneWayPlatformController : MonoBehaviour
{
    private Collider2D platformCollider;

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // ↓ + 점프 키 누를 때 콜라이더 잠시 꺼짐
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DisableColliderTemporarily());
        }
    }

    IEnumerator DisableColliderTemporarily()
    {
        platformCollider.enabled = false;
        yield return new WaitForSeconds(1f); // 통과할 시간
        platformCollider.enabled = true;
    }
}
