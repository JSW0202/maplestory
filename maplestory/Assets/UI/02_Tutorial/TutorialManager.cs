using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector nexonTL;
    [SerializeField] private Camera mainCamera;
 
    public void Start()
    {
        mainCamera = Camera.main;
        TimelineManager.instance.ResetFastLevel();
    }

    public void NexonSFX()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.SFXType.nexon);
    }


    public void EndTimeline()
    {
        nexonTL.Stop();
    }

    public void FreezeGame()
    {
        Time.timeScale = 0;
        if (SoundManager.instance != null)
            SoundManager.instance.StopBGM();
    }

    public void UnfreezeGame()
    {
        Time.timeScale = 1;
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySceneBGM("TutorialScene");
    }

    public void ChangeBGM()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.StopBGM();
            SoundManager.instance.PlaySceneBGM("TutorialScene2");
        }
    }

    public void StopBGM()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.StopBGM();
    }

    public void NextScene()
    {
        TimelineManager.instance.NextScene();
    }

    public void ShowLearnSkill()
    {
        UIManager.instance.LearnSkill_UI.SetActive(true);
    }
    public void HideLearnSkill()
    {
        UIManager.instance.LearnSkill_UI.SetActive(false);
    }

    public void ShowTutorialLearnSkill()
    {
        UIManager.instance.SetTutorialSkill();
        UIManager.instance.LearnSkill_UI.SetActive(true);

    }

    public void ShowStatusUI()
    {
        UIManager.instance.Status_UI.SetActive(true);
    }
    public void HideStatusUI()
    {
        UIManager.instance.Status_UI.SetActive(false);
    }

}
