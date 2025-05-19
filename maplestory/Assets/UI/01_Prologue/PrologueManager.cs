using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrologueManager : MonoBehaviour
{

 
    public void Start()
    {

    }

    public void LightbulbSFX()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.SFXType.lightbulb);
    }

    public void SwingSFX()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.SFXType.swing);
    }

    public void NextScene()
    {
        TimelineManager.instance.NextScene();
    }
}
