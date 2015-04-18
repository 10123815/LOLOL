using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// this manager is indepented from specific skill
public class SkillManager : MonoBehaviour
{

    private GameObject m_hero;
    private BaseProperty m_property;
    private SkillBase[] m_skills;

    public Image[] cds;
    //public Button[] skill;

    // Use this for initialization
    void Start ( )
    {
        m_hero = GameObject.Find(PlayerPrefs.GetString("player name"));
        m_property = m_hero.GetComponent<BaseProperty>();
        m_skills = m_hero.GetComponents<SkillBase>();
    }

    // Update is called once per frame
    void Update ( )
    {
        for (int i = 0; i < 4; i++)
        {
            cds[i].fillAmount = m_skills[i].curCd / m_skills[i].cd;
        }
    }

    public void OnClick(int _i)
    {
        m_skills[_i].launched = true;
    }
}
