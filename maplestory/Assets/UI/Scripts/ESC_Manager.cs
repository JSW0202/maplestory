using NUnit.Framework.Internal.Commands;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESC_Manager : MonoBehaviour
{
    public static ESC_Manager instance;

    [Header("Settings UI")]
    [SerializeField] public GameObject settingsUI;

    [Header("Audio UI")]
    [SerializeField] private Slider volumeSlider;

    [Header("Resolution UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private Resolution[] resolutions;
    private int currentResolutionIndex;

    private bool isTutorialActive = false;



    void Awake()
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
        settingsUI.SetActive(false);

        SetupResolutions();
        fullscreenToggle.isOn = Screen.fullScreen;

        volumeSlider.onValueChanged.AddListener(SetVolume);
        Screen.SetResolution(1920, 1080, true);

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (TimelineManager.instance.myTimeline != null)
            {
                if(TimelineManager.instance.myTimeline.state == PlayState.Playing)
                {
                    TimelineManager.instance.PauseTimeline();
                }
            }

            if(settingsUI.activeSelf)
            {
                SkipButton.instance.buttonComponent.interactable = false;
                SkipButton.instance.buttonComponent.interactable = true;
                TimelineManager.instance.skipUI.SetActive(false);
                settingsUI.SetActive(false);
                TimelineManager.instance.ResumeTimeline();
                Time.timeScale = 1;
            }
            else
            {
                settingsUI.SetActive(true);
                SkipButton.instance.skipButton.SetActive(true);
                Time.timeScale = 0;
                
            }
        }
    }

    public void SkipTimeline()
    {
        
        if (TimelineManager.instance != null)
        {
            // TimeScale을 1로 설정하기 전에 먼저 처리
            TimelineManager.instance.StopTimeline();
            TimelineManager.instance.NextScene();
            settingsUI.SetActive(false);
            Time.timeScale = 1f;
            
            Debug.Log("씬 전환 완료");
        }
        else
        {
            Debug.LogError("TimelineManager.instance가 null입니다!");
        }
    }

    void SetVolume(float volume)
    {
        // AudioManager.Instance?.SetSFXVolume(volume); 등으로 연결 가능
    }

    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i]. width + "*" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width
                && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    
}
