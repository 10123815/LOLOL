using UnityEngine;
using System.Collections;
using FuckLolLib;

public class ZhaoxinE : SkillBase
{

    [System.NonSerialized]
    public bool hasPrepared = false;

    // the target of zhaoxin
    private GameObject m_target;
    private HeroController m_heroCtrl;

    public int damage = 100;
    public float duration = 4f;
    public float slowDown = 0.3f;

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
                                                   * 
                                                     * 
                                                        * 
                                                           *  
                                                               * 
                                                                  * 
                                                                     * 
          ---press E, set cursor---click a enemy, CD--------------------
         */

        if (enabled &&
            (Input.GetKeyDown(KeyCode.E) || launched) &&
            curCd <= 0 && manaCost <= m_property.curMana)
        {
            m_heroCtrl.SetCursor(GameCode.CursorCode.SingleSkill);
            hasPrepared = true;
            launched = false;
        }

        if (Input.GetMouseButtonDown(1))
            hasPrepared = false;

        if (hasPrepared && Input.GetMouseButtonDown(0))
        {
            if (m_heroCtrl.leftMouseClkGo != null &&
                range > Vector3.Distance(m_heroCtrl.leftMouseClkGo.transform.position, transform.position))
            {
                string _group = m_heroCtrl.leftMouseClkGo.tag;
                if (m_heroCtrl.AtDifferentGroup(_group, gameObject.tag))
                {
                    m_target = m_heroCtrl.leftMouseClkGo;
                    m_property.UseMana(manaCost);
                    hasPrepared = false;
                    curCd = cd;
                    //print("zhaoxin use E!!!!!!!!!" + m_heroCtrl.leftMouseClkGo.name); 
                    AdnormalState _teleport = new AdnormalState(GameCode.AdnormalStateCode.Teleport, 0.2f, 0.0f);
                    m_property.SetAdnormalState(_teleport);
                    m_heroCtrl.rightMouseClkGo = m_target;
                    m_heroCtrl.target = m_target;
                    m_animator.SetBool("atk1", true);
                    m_heroCtrl.SetCursor(GameCode.CursorCode.Normal);

                    // slow the target
                    BaseProperty _p = m_target.GetComponent<BaseProperty>();
                    _p.Damaged(damage);
                    AdnormalState _slow = new AdnormalState(GameCode.AdnormalStateCode.Slow, duration, slowDown);
                    _p.SetAdnormalState(_slow);
                    _p.moveSpeed *= (1 - slowDown);
                }
            }
            hasPrepared = false;
        }

        if (curCd > 0)
        {
            curCd -= Time.deltaTime;
            // dash to enemy
            if (m_property.adnormalStates[(int)GameCode.AdnormalStateCode.Teleport].duration >= 0)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    m_target.transform.position,
                    Time.deltaTime * 5);
            }
        }
    }
}
