// using System.Collections.Generic;
// using UnityEngine;

// public class SkillDatabase : MonoBehaviour
// {
//     [SerializeField] private List<Skill> skills = new List<Skill>();
//     private static SkillDatabase instance;

//     public static SkillDatabase Instance
//     {
//         get
//         {
//             if(instance == null)
//             {
//                 instance = FindAnyObjectByType<SkillDatabase>();
//                 if(instance == null)
//                 {
//                     GameObject obj = new GameObject("SkillList");
//                     instance = obj.AddComponent<SkillDatabase>();
//                 }
//             }
//             return instance;
//         }
//     }

//     private void Awake()
//     {
//         if (instance !=null && instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }
//         instance = this;
//         DontDestroyOnLoad(gameObject);

//         LoadSkills();
//     }
    
//     private void LoadSkills()
//     {
//         skills.Add(new Skill(
//             1,
//             "발록 클로",
//             "발록의 날카로운 발톱을 적을 찣어발깁니다.",
//             Resources.Load<Sprite>("")
//         ));
//     }

//     private void Update()
//     {
//         if(Input.GetKeyDown(KeyCode.Keypad4))
//         {
//             skills.Add(new Skill(
//                 2,
//                 "파이어볼",
//                 "발록의 분노를 가득 담은 불덩이를 날립니다.",
//                 Resources.Load<Sprite>("")
//             ));
//         }

//         if(Input.GetKeyDown(KeyCode.Keypad5))
//         {
//             skills.Add(new Skill(
//                 3,
//                 "메테오",
//                 "하인즈조차 두려워할 메테오 마법을 시전합니다.",
//                 Resources.Load<Sprite>("")
//             ));
//         }
               
//     } 
// }
