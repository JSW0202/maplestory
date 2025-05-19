using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager instance;
    public GameObject timeline;
    public PlayableDirector myTimeline;
    private bool checkTimeline = true;


    [SerializeField] public GameObject skipUI;
    public GameObject fastUI;
    [SerializeField] private TMP_Text fastText;

    private bool isPaused = false;
    private int fastLevel = 1;
    private double pausedTime; // 일시정지된 시간을 저장할 변수

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }   
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        skipUI.SetActive(false);
        fastUI.SetActive(false);
        fastText = fastUI.GetComponentInChildren<TMP_Text>();
        timeline = GameObject.FindGameObjectWithTag("Timeline");
        myTimeline = timeline.GetComponent<PlayableDirector>();

        myTimeline.played += OnTimelineStarted;
        myTimeline.stopped += OnTimelineStopped;
        myTimeline.extrapolationMode = DirectorWrapMode.Hold;
    }

    void Update()
    {

        if (timeline == null && checkTimeline)
        {
            timeline = GameObject.FindGameObjectWithTag("Timeline");
            myTimeline = timeline.GetComponent<PlayableDirector>();
            if(timeline == null)
            {
                checkTimeline = false;
                return;
            }
        }

        if(myTimeline != null && myTimeline.state == PlayState.Playing)
        {
            FastForward();
        }


    }

    private void FastForward()
    {
        myTimeline.playableGraph.GetRootPlayable(0).SetSpeed(fastLevel);
        fastText.text = $"x{fastLevel} 배속 진행 중";

        if (fastLevel == 1)
        {
            fastUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (fastLevel < 2)
            {
                fastLevel = 2;
                fastUI.SetActive(true);
            }
            else if (fastLevel == 2)
            {
                fastLevel = 4;
            }
            else if (fastLevel == 4)
            {
                fastLevel = 1;
            }
        }
    }

    public void ResetFastLevel()
    {
        fastLevel = 1;
        fastUI.SetActive(false);
    }


    public void PauseTimeline()
    {
        if (!isPaused)
        {
            pausedTime = myTimeline.time;
            myTimeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void ResumeTimeline()
    {
        if (isPaused)
        {
            myTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void SetSkipUI()
    {
        skipUI.SetActive(true);
    }

    public void CloseSkipUI()
    {
        skipUI.SetActive(false);
    }

    public void StopTimeline()
   {
        fastLevel = 1;
        fastUI.SetActive(false);
        skipUI.SetActive(false);
        myTimeline.Stop();
        Time.timeScale = 1;
        isPaused = false;
   }

    public void NextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            timeline = GameObject.FindGameObjectWithTag("Timeline");
            myTimeline = timeline.GetComponent<PlayableDirector>();
        }
    }

    private void OnTimelineStarted(PlayableDirector director)
    {
        isPaused = false;
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        isPaused = false;
        skipUI.SetActive(false);
    }

    void OnDestroy()
    {
        // 이벤트 구독 해제
        if (myTimeline != null)
        {
            myTimeline.played -= OnTimelineStarted;
            myTimeline.stopped -= OnTimelineStopped;
        }
    }

}
