
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCol;
    [SerializeField] private GameObject oldPlayer;
    [SerializeField] private GameObject oldCamera;
    [SerializeField] private GameObject oldDummy;
    void Awake()
    {
        oldDummy = GameObject.Find("DummyManager");
        oldPlayer = GameObject.FindWithTag("Player");
        oldCamera = GameObject.FindWithTag("StageCamera");
        Destroy(oldPlayer);
        Destroy(oldCamera);
        Destroy(oldDummy);
        UIManager.instance.Status_UI.SetActive(false);
        UIManager.instance.skill_UI.SetActive(false);
    }


    public void ChangeBGM()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.StopBGM();
            SoundManager.instance.PlaySceneBGM("EndingScene2");
        }
    }    

    void Update()
    {
        
    }

    public void SetClearGame()
    {
        UIManager.instance.gameClear = true;
    }

    public void NextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
