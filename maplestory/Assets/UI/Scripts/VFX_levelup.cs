using UnityEngine;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UIElements;

public class VFX_levelup : MonoBehaviour
{
    public float speed;
    public float colorTimer;
    public SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        colorTimer = Time.deltaTime;

        transform.Translate(Vector3.up * speed * Time.deltaTime);
        // sr.color = new Color (1, 1, 1, 1 - colorTimer);
        // 색 시간 지날수록 투명하게 하고 싶은데 잘 안됨.
    }
}
