using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class SkipButton : MonoBehaviour
{
    public static SkipButton instance;
    public GameObject skipButton;
    public Button buttonComponent;
    [SerializeField] private TimelineManager timelineManager;

    private void Awake()
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
        skipButton = this.gameObject;
        timelineManager = TimelineManager.instance;
        buttonComponent = skipButton.GetComponent<Button>();
    }

    void Update()
    {

        if(skipButton == null)
        {
            skipButton = this.gameObject;
            buttonComponent = skipButton.GetComponent<Button>();
        }
        if (timelineManager == null)
        {
            timelineManager = TimelineManager.instance;
            return;
        }

        if (timelineManager.myTimeline == null)
        {
            GameObject timeline = GameObject.FindGameObjectWithTag("Timeline");
            if (timeline != null)
            {
                timelineManager.myTimeline = timeline.GetComponent<PlayableDirector>();
            }
        }

        if (timelineManager.myTimeline != null)
        {
            Debug.Log($"Timeline State: {timelineManager.myTimeline.state}");
            if (timelineManager.myTimeline.state == PlayState.Playing)
            {
                skipButton.SetActive(true);
            }
            else
            {
                skipButton.SetActive(false);
            }
        }
    }
}
