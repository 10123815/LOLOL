using UnityEngine;
using System.Collections;
using FuckLolLib;

public class SkillBase : MonoBehaviour
{

    // if this skill is availabled;
    // hero may be stunned/silenced....
    public bool enabled = true;

    // when press on UI
    [System.NonSerialized]
    public bool launched = false;

    public int level = 1;
    public float cd;
    public float curCd = 0;
    public int manaCost;
    public float range;
    public string skillDiscribe;

    [System.NonSerialized]
    protected Animator m_animator;
    
    // many skills are related to the properties of heros
    [System.NonSerialized]
    protected BaseProperty m_property;

    public enum SkillTargetType
    {
        None = 0,
        Single = 1,
        RoundArea = 2,
        FanArea = 3,
        LineArea = 4
    }

    public SkillTargetType tagetType;

    // Use this for initialization
    void Start ( )
    {

    }

    // Update is called once per frame
    void Update ( )
    {

    }

    public virtual void LevelUp()
    {

    }

    public virtual void Effect()
    {
    
    }

}
