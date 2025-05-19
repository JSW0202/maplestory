// using Unity.VisualScripting;
// using UnityEngine;
// using static UnityEditor.Progress;

// [System.Serializable]
// public class SkillSlot
// {
//     public Skill skill;
//     public int cooldown;

//     public SkillSlot()
//     {
//         skill = null;
//         cooldown = 0;
//     }

//     public SkillSlot(Skill skill, int cooldown)
//     {
//         this.skill = skill;
//         this.cooldown = cooldown;
//     }

//     public bool IsEmpty()
//     {
//         return skill == null;
//     }

//     public bool CanAddSkill(Skill newSkill)
//     {
//         if (IsEmpty())
//             return true;
//         return false;
//     }

//     public void AddSkill(Skill newSkill, int cooldown)
//     {
//         if (IsEmpty())
//         {
//             skill = newSkill.Clone();
//             \
//         }
//     }
// }