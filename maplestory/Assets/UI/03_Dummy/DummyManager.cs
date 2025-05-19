using UnityEngine;

public class DummyManager : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        UIManager.instance.Status_UI.SetActive(true);
        UIManager.instance.skill_UI.SetActive(true);
        UIManager.instance.LearnSkill_UI.SetActive(false);
        TimelineManager.instance.fastUI.SetActive(false);
    }

    void Update()
    {
        
    }
}
