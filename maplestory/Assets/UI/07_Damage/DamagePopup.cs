using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public Sprite[] digitSprites; // 0~9 스프라이트 배열 (인스펙터에 등록)
    public GameObject digitPrefab; // 하나의 숫자 프리팹 (SpriteRenderer 포함)
    public float digitSpacing = 0.1f;
    public float lifetime = 1.0f;
    public float floatSpeed = 1.0f;

    public void Setup(int damage)
    {
        string damageStr = damage.ToString();
        float totalWidth = (damageStr.Length - 1) * digitSpacing;

        for (int i = 0; i < damageStr.Length; i++)
        {
            int digit = int.Parse(damageStr[i].ToString());
            GameObject digitObj = Instantiate(digitPrefab, transform);

            // 위치 계산
            float x = i * digitSpacing - totalWidth / 2f;
            float y = (i % 2 == 1) ? -0.02f : 0f; // 짝수번째 (1, 3, 5...)는 아래로
            digitObj.transform.localPosition = new Vector3(x, y, 0);

            // 스프라이트 설정
            SpriteRenderer sr = digitObj.GetComponent<SpriteRenderer>();
            sr.sprite = digitSprites[digit];

            // 맨 앞 숫자 크게
            if (i == 0)
            {
                digitObj.transform.localScale = Vector3.one * 1.3f;
                sr.sortingOrder = 10;
            }
            else
            {
                digitObj.transform.localScale = Vector3.one;
                sr.sortingOrder = 10 - i;
            }
        }

        Destroy(gameObject, lifetime);
    }



    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }
}
