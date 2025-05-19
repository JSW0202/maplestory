using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SkillInfo
{
    public int requiredLevel;
    public int skillIconIndex;
    public string titleText;
    public string skillName;
    public string descriptionText;
    public string dialogueText;
    public bool isStageSkill;    // 스테이지 클리어 스킬인지 여부
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject canvas;
    public AudioSource SFX_levelUP;
    public GameObject VFX_levelUP;
    public Transform playerPosition;
    public GameObject Status_UI;
    public GameObject skill_UI;
    public GameObject LearnSkill_UI;
    public GameObject Death_UI;
    public GameObject Dialogue_UI;
    public TMP_Text Dialouge_Text;
    public bool gameClear = false;
    [SerializeField] private bool CanLearnSkill = false;
    [SerializeField] private float UI_Timer;

    [SerializeField] private List<Sprite> skillIconList;

    #region 변수들
    [Header("UI 연결")]
    public Image HP_UI;
    public Image EXP_UI;
    public TMP_Text textHP;
    public TMP_Text textEXP;
    public TMP_Text textLV;
    public Image LearnSkill_Icon;
    public TMP_Text LearnSkill_Title;
    public TMP_Text LearnSkill_Name;
    public TMP_Text LearnSkill_Text;

    [Space]

    [Header("HP 수치")]
    public float HP;
    public float maxHP;

    [Space]

    [Header("EXP 수치")]
    public float EXP;
    public float maxEXP;
    public float presentEXP;

    public int LV = 80;
    public int maxLV = 260;

    [Header("스킬 정보")]
    [SerializeField] private List<SkillInfo> skillInfoList;

    #endregion


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(canvas);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeSkillInfoList();

    }
    private void InitializeSkillInfoList()
    {
        skillInfoList = new List<SkillInfo>
        {
            // 엘레니아 스킬
            new SkillInfo {
                requiredLevel = 95,
                skillIconIndex = 1,
                titleText = "스킬을 습득했습니다",
                skillName = "텔레포트",
                descriptionText = "마법사들의 이동을 따라하는 법을 습득했습니다.\nShift키로 텔레포트를 사용하실 수 있습니다.",
                dialogueText = "레벨업은 이런 상쾌한 느낌이군!\n마법사들의 지식이 흘러들어온다!!!",
                isStageSkill = false
            },
            new SkillInfo {
                requiredLevel = 110,
                skillIconIndex = 2,
                titleText = "스킬을 습득했습니다",
                skillName = "힐",
                descriptionText = "마법사들의 생명연장법을 습득했습니다.",
                dialogueText = "이런 스킬들을 가지고 있었다니, 마법사들은 비열하군.",
                isStageSkill = false
            },
            new SkillInfo {
                requiredLevel = 125,
                skillIconIndex = 3,
                titleText = "스테이지 클리어",
                skillName = "메테오",
                descriptionText = "일정 범위 내 적에게 메테오를 소환합니다.",
                dialogueText = "힘이... 힘이 점점 돌아온다!!",
                isStageSkill = true
            },

            // 헤네시스 스킬
            new SkillInfo {
                requiredLevel = 140,
                skillIconIndex = 4,
                titleText = "스킬을 습득했습니다",
                skillName = "쓰러스트",
                descriptionText = "발록이 궁수들의 민첩함의 비법을 깨달았습니다.\n발록 이동속도 x2",
                isStageSkill = false
            },
            new SkillInfo {
                requiredLevel = 155,
                skillIconIndex = 5,
                titleText = "스킬을 습득했습니다",
                skillName = "아마존의 눈",
                descriptionText = "발록의 시야가 기적적으로 늘어납니다.\n발록 스킬 사거리 x2",
                isStageSkill = false
            },
            new SkillInfo {
                requiredLevel = 170,
                skillIconIndex = 6,
                titleText = "스테이지 클리어",
                skillName = "샤프 아이즈",
                descriptionText = "적의 급소만을 노립니다. \n발록 공격력 x2",
                isStageSkill = true
            },

            //페리온 스킬
            new SkillInfo {
                requiredLevel = 185,
                skillIconIndex = 8,
                titleText = "스킬을 습득했습니다",
                skillName = "하이퍼바디",
                descriptionText = "발록이 전사들의 강인함을 깨달았습니다.\n발록 체력 x2",
                isStageSkill = false
            },
            new SkillInfo {
                requiredLevel = 200,
                skillIconIndex = 9,
                titleText = "스킬을 습득했습니다",
                skillName = "드래곤 로어",
                descriptionText = "발록의 포효에 용기사의 힘이 깃듭니다.\n X키를 눌러 드래곤로어를 사용할 수 있습니다.",
                isStageSkill = false
            },
            new SkillInfo {
                requiredLevel = 215,
                skillIconIndex = 10,
                titleText = "스테이지 클리어",
                skillName = "아이언월",
                descriptionText = "발록의 가죽이 강철처럼 단단해졌습니다.\n발록이 받는 데미지가 50%로 감소합니다.",
                isStageSkill = true
            },
        };


    }

    void Start()
    {
        SFX_levelUP = GetComponent<AudioSource>();
        LearnSkill_UI.SetActive(false);
        // maxHP = Balrog.instance.maxHp;
        // HP = maxHP;
        // EXP = Balrog.instance.exp;
        SetMaxEXP();
        HP_UI.fillAmount = 1;
        EXP_UI.fillAmount = 0;
    }

    void Update()
    {


        if (Balrog.instance != null)
        {
            HP = Balrog.instance.currentHp;
            maxHP = Balrog.instance.maxHp;
            EXP = Balrog.instance.exp;
        }
        else
        {
            HP = 1000;
            maxHP = 1000;
            EXP = 0;
        }

        textHP.text = $"[{HP}/{maxHP}]";
        textLV.text = $"{LV}";

        HP_UI.fillAmount = HP / maxHP;

        KeyCheck();

        if (HP <= 0)
        {
            HP = 0;
            //발록 사망 이벤트에서 불러오기
            Death_UI.SetActive(true);
        }

        LevelUP();
        LearnSkill();
    }

    private void LearnSkill()
    {
        if (!CanLearnSkill) return;

        foreach (var skillInfo in skillInfoList)
        {
            if (LV == skillInfo.requiredLevel)
            {
                ShowSkillUI(skillInfo);
                break;
            }
        }
    }

    private void ShowSkillUI(SkillInfo skillInfo)
    {
        LearnSkill_UI.SetActive(true);
        Time.timeScale = 0;

        LearnSkill_Icon.sprite = skillIconList[skillInfo.skillIconIndex];
        LearnSkill_Title.text = skillInfo.titleText;
        LearnSkill_Name.text = skillInfo.skillName;
        LearnSkill_Text.text = skillInfo.descriptionText;

        if (!string.IsNullOrEmpty(skillInfo.dialogueText))
        {
            Dialogue_UI.SetActive(true);
            Dialouge_Text.text = skillInfo.dialogueText;
        }

        StartCoroutine(CloseUI());
    }

    public IEnumerator CloseUI()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LearnSkill_UI.SetActive(false);
            Dialogue_UI.SetActive(false);
            CanLearnSkill = false;
            Time.timeScale = 1;
        }
    }

    private void SetMaxEXP()
    {
        Debug.Log($"SetMaxEXP 호출됨. 현재 레벨: {LV}");

        switch (LV)
        {
            case 80: maxEXP = 800; break;
            case 95: maxEXP = 900; break;
            case 110: maxEXP = 1000; break;
            case 125: maxEXP = 2000; break;
            case 140: maxEXP = 2500; break;
            case 155: maxEXP = 2800; break;
            case 170: maxEXP = 4500; break;
            case 185: maxEXP = 4750; break;
            case 200: maxEXP = 5000; break;
            case 215: maxEXP = 7000; break;
            case 230: maxEXP = 8000; break;
            case 245: maxEXP = 8500; break;
            default:
                Debug.LogWarning($"레벨 {LV}에 대한 maxEXP 설정이 없습니다.");
                maxEXP = 1000; // 기본값 설정
                break;
        }

        Debug.Log($"설정된 maxEXP: {maxEXP}");
    }


    private void LevelUP()
    {
        if(Balrog.instance == null)
            return;
        else
            Balrog.instance.level = LV;

        if (LV < maxLV)
        {
            if (EXP >= maxEXP)
            {
                // 초과한 경험치 저장
                float remainExp = EXP - maxEXP;

                // 레벨업 처리 - 다음 레벨 결정
                int nextLevel = LV;
                switch (LV)
                {
                    case 80: nextLevel = 95; break;
                    case 95: nextLevel = 110; break;
                    case 110: nextLevel = 125; break;
                    case 125: nextLevel = 140; break;
                    case 140: nextLevel = 155; break;
                    case 155: nextLevel = 170; break;
                    case 170: nextLevel = 185; break;
                    case 185: nextLevel = 200; break;
                    case 200: nextLevel = 215; break;
                    case 215: nextLevel = 230; break;
                    case 230: nextLevel = 245; break;
                    case 245: nextLevel = 260; break;
                }

                LV = nextLevel;
                
                SetMaxEXP();  // 새로운 maxEXP 설정

                // 초과 경험치 적용
                Balrog.instance.exp = int.Parse(remainExp.ToString());
                EXP = remainExp;

                playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
                if (playerPosition != null)
                {
                    GameObject go = Instantiate(VFX_levelUP, playerPosition.position, Quaternion.identity);
                    Destroy(go, 1f);
                    if (SoundManager.instance != null)
                        SoundManager.instance.PlaySFX(SoundManager.SFXType.levelUP);
                    Balrog.instance.maxHp += 1000;
                    Balrog.instance.currentHp = Balrog.instance.maxHp;
                    CanLearnSkill = true;
                }

                Debug.Log($"레벨업! 현재 레벨: {LV}, 남은 경험치: {remainExp}");
            }

            presentEXP = EXP / maxEXP * 100;
            EXP_UI.fillAmount = EXP / maxEXP;
            textEXP.text = $"{EXP}[{presentEXP:F2}%]";
        }
    }


    public void KeyCheck()
    {
        if (Input.GetKey(KeyCode.Keypad1))
        {
            Debug.Log("HP가 10 감소합니다.");
            Balrog.instance.GetDamage(10);
        }

        if (Input.GetKey(KeyCode.Keypad3))
        {
            Debug.Log("EXP가 5000 증가합니다.");
            Balrog.instance.exp += 5000;
        }
    }

    public void SetTutorialSkill()
    {
        LearnSkill_Icon.sprite = skillIconList[7];
        LearnSkill_Title.text = "주니어 발록의 전직에 대하여";
        LearnSkill_Name.text = "환생의 축복";
        LearnSkill_Text.text = "주니어 발록은 전직 대신, 모험가들을 사냥하여\n 해당하는 직업군의 스킬을 흡수할 수 있습니다.";
    }


}

