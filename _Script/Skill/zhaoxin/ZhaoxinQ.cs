using UnityEngine;
using System.Collections;
using FuckLolLib;

// if we press "q", and the curCd == 0, this skill will be launched
// in duration, the third atk will knock up his target, and set the duration to 0
// whenever duration setted to 0, this skill will enter into cd
public class ZhaoxinQ : SkillBase
{

    public float duration = 10.0f;
    public int atkCount = 3;
    public bool inUse = false;

    // the left time we have since we press "q"
    // from duration down to 0
    public float m_curDuration = 0.0f;
    public bool m_hasAttack = false;

    // the target of zhaoxin
    private GameObject m_target;
    private HeroController m_heroCtrl;

    // the damage of this skill is the atk of zhaoxin
    //private int DMG;

    // effect
    public GameObject targetEffectPrefab;
    public GameObject zhaoxinEffectPrefab;

    // Use this for initialization
    void Start ( )
    {
        m_animator = GetComponent<Animator>();
        m_property = GetComponent<BaseProperty>();
        m_heroCtrl = GetComponent<HeroController>();
    }

    // Update is called once per frame
    void Update ( )
    {

        /*
                           *             *
                             *             *
                                *              *
                                   *                *
                                     *                  *
                                       *                     *
                                         *                         *
         ----------------   duration     -           cd                ---------------
         *                 3 atk here      3th atk completed or duration=0
         */


        // we can use this skill only we haven't press it and it is not in CD
        if (enabled && 
            (Input.GetKeyDown(KeyCode.Q) || launched) && 
            curCd <= 0 && m_curDuration <= 0 && manaCost <= m_property.curMana)
        {
            m_curDuration = duration;
            m_property.UseMana(manaCost);
            m_hasAttack = false;
            launched = false;
            atkCount = 3;
            //print("zhaoxin use q");
        }

        AnimatorStateInfo _anmStt = m_animator.GetCurrentAnimatorStateInfo(0);
        if (m_curDuration > 0)
        {
            m_curDuration -= Time.deltaTime;
            curCd = 0;
            if (_anmStt.IsTag("a"))
            {
                if (_anmStt.normalizedTime > 0.25f && _anmStt.normalizedTime < 0.45f && !m_hasAttack)
                {
                    atkCount--;
                    m_hasAttack = true;
                    //print(atkCount);
                }

                if (_anmStt.normalizedTime > 0.5f)
                    m_hasAttack = false;
            }
            else if (_anmStt.IsName("Base.batk"))
            {
                if (atkCount == 1)
                {
                    m_animator.SetBool("q", true);
                    m_hasAttack = false;
                }
            }
        }
        else if (m_curDuration < 0)
        {
            curCd = cd;
            m_curDuration = 0;
        }

        if (_anmStt.IsName("Base.q"))
        {
            if (m_animator.GetBool("q"))
            {
                m_curDuration = 0;
                m_animator.speed = m_property.atkSpeed / m_property.baseAtkSpeed;
                m_animator.SetBool("q", false);
                curCd = cd;
                m_target = m_heroCtrl.rightMouseClkGo;
                BaseProperty _property = m_target.GetComponent<BaseProperty>();
                if (_property != null)
                {
                    // knock up enemy for 0.5 second
                    AdnormalState _adnmStt = new AdnormalState(GameCode.AdnormalStateCode.KnockUp, 0.5f, 0.0f);
                    _property.SetAdnormalState(_adnmStt);
                    _property.Damaged((int)((float)m_property.atk * (1 + (float)level / 10)));
                    // effect of this skill
                    PoolManager.GetInstance().
                        GetPool(targetEffectPrefab.name, targetEffectPrefab).
                        GetObject(m_target.transform.position + new Vector3(0, m_target.GetComponent<Collider>().bounds.size.y / 2, 0));
                }
                //print("zhaoxin Q !!!!!!!!!!");
            }
        }

        if (curCd > 0)
        {
            curCd -= Time.deltaTime;
        }

    }

    public override void LevelUp ( )
    {
        level += 1;
    }

    public override void Effect ( )
    {

    }
}
