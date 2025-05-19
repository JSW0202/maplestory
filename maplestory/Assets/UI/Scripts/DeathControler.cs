using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DeathControler : MonoBehaviour
{
    public GameObject reviveUI;
    public TMP_InputField reviveInput;
    public TMP_Text warningText; // 추가된 경고 텍스트

    private Vector3 spawnPosition;
    private bool isDead = false;

    void Start()
    {
        spawnPosition = transform.position;

        reviveInput.onEndEdit.AddListener(OnReviveInputEnd);
        reviveUI.SetActive(false);
        warningText.text = "";
    }

    public void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
        //reviveUI.SetActive(true);
        reviveInput.text = "";
        warningText.text = "";
        reviveInput.ActivateInputField();
    }

    public void OnReviveInputEnd(string input)
    {
        if (input == "살려주세요")
        {
            Revive();
        }
        else
        {
            StartCoroutine(ShowWarning());
        }
    }

    IEnumerator ShowWarning()
    {
        warningText.text = "다시 제대로 빌어보세요";
        yield return new WaitForSeconds(2f); // 2초 후 메시지 사라짐
        warningText.text = "";
        reviveInput.ActivateInputField(); // 다시 입력할 수 있게 포커싱
    }

    void Revive()
    {
        reviveInput.text = "";        
        transform.position = spawnPosition;
        gameObject.SetActive(true);
        reviveUI.SetActive(false);
        UIManager.instance.Death_UI.SetActive(false);          
        Balrog.instance.stateMachine.ChangeState(Balrog.instance.idleState);
    }
}
