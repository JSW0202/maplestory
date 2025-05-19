using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        
    }

    public void StartGame()
    {    
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);        
    }


    // 게임 종료 버튼 클릭 시
    public void QuitGame()
    {
        Debug.Log("게임 종료");

#if UNITY_EDITOR
        // 에디터에서 실행 중이라면
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 앱에서는 앱 종료
        Application.Quit();
#endif
    }
}
