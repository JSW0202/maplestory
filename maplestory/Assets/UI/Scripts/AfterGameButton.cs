using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfterGameButton : MonoBehaviour
{
    public static AfterGameButton instance;
    [SerializeField] private GameObject afterGameButton;
    [SerializeField] private Button thisButton;
    [SerializeField] private TMP_Text buttonText;
    public bool gameClear = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }   
    }
    void Start()
    {
        afterGameButton = this.gameObject;
        thisButton = afterGameButton.GetComponent<Button>();
        buttonText = afterGameButton.GetComponentInChildren<TMP_Text>();
        buttonText.color = Color.clear;
        thisButton.interactable = false;
    }

    void Update()
    {
        if (gameClear == true)
        {
            thisButton.interactable = true;
            buttonText.color = Color.white;
        }
        else
        {
            thisButton.interactable = false;
            buttonText.color = Color.clear;
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (gameClear == false)
                gameClear = true;
            else { gameClear = false; }
        }

        gameClear = UIManager.instance.gameClear;
    }

    public void OnButtonClick()
    {
        SceneManager.LoadScene("CreditScene"); 
    }
}


