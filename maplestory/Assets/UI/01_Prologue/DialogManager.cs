using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    public Text dialogText;
    public Text speakerName;
    public TMP_Text dialogTMP;
    public TMP_Text speakerTMP;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        dialogTMP.text = dialogText.text;
        speakerTMP.text = speakerName.text;
    }
}
