using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        UIManager.instance.gameClear = true;
        Debug.Log("Button Clicked");
        SceneManager.LoadScene("TitleScene"); 
    }
}
