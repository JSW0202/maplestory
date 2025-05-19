using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_UI : MonoBehaviour
{
    public static Skill_UI instance;
    public float skillCooldown;
    public bool isSkillLearned;
    [SerializeField] private Image myImage;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TMP_Text cooldownValue;
    [SerializeField] private TMP_Text inputText;
    [SerializeField] private KeyCode inputKey;
    [SerializeField] private float skillTimer = 0;
    [SerializeField] private int learnLevel;


    void Awake()
    {
        if (instance = null)
            instance = this;
        if (instance != null)
            Destroy(gameObject);
    }
    
    void Start()
    {
        cooldownValue.enabled = false;
        inputText = GetComponentInChildren<TMP_Text>();
        myImage = GetComponent<Image>();
    }

    void Update()
    {
        string keyText = inputKey.ToString();
        inputText.text = keyText.Replace("LeftShift", "Shift")
            .Replace("RightShift", "Shift")
            .Replace("LeftControl", "Ctrl")
            .Replace("RightControl", "Ctrl")
            .Replace("LeftAlt", "Alt")
            .Replace("RightAlt", "Alt")
            .Replace("Alpha", "")
            .Replace("Keypad", "")
            .Replace(" ", "")
            .Replace("Keypad", "")
            .Replace("NumPad", "")
            .Replace("0", "0")
            .Replace("1", "1")
            .Replace("2", "2")
            .Replace("3", "3")
            .Replace("4", "4")
            .Replace("5", "5")
            .Replace("6", "6")
            .Replace("7", "7")
            .Replace("8", "8")
            .Replace("9", "9")
            .Replace("None", "");

        if (learnLevel > UIManager.instance.LV)
        {
            myImage.color = new Color(0.2f, 0.2f, 0.2f, 1);
            inputText.color = Color.clear;
            isSkillLearned = false;
        }
        else if (learnLevel <= UIManager.instance.LV)
        {
            myImage.color = Color.white;
            inputText.color = Color.white;
            isSkillLearned = true;
        }
    
        skillTimer -= Time.deltaTime;
        cooldownValue.text = $"{skillTimer:F0}";
        cooldownImage.fillAmount = skillTimer / skillCooldown;

        if(Input.GetKeyDown(inputKey) && skillTimer <= 0 && isSkillLearned)
        {
            cooldownValue.enabled = true;
            skillTimer = skillCooldown;
            cooldownImage.color = new Color (0.5f, 0.5f, 0.5f, 1);
        }

        if (skillTimer <= 0)
        {
            cooldownValue.enabled = false;
            cooldownImage.color = Color.white;
        }
    }

}
