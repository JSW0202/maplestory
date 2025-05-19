using UnityEngine;

public class SkillManager : MonoBehaviour
{

    public static SkillManager Instance;


    public Skill_Meteor meteor { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        meteor = GetComponent<Skill_Meteor>();
    }

}
