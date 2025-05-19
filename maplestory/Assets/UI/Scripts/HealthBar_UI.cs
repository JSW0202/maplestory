using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity; // Entity 컴포넌트 참조
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        entity = GetComponentInParent<Entity>();

        if (entity != null)
        {
            entity.onFlipped += FlipUI;
            UpdateHealthUI();
        }
    }

    private void Update()
    {
        if (entity != null)
            UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = entity.maxHp;
        slider.value = entity.currentHp;
    }

    public void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;
    }
}
