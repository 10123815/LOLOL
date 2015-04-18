using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FuckLolLib;

public class ZhaoxinR : SkillBase
{

    public int atk = 50;
    public float knockBackDis = 5.0f;

    private ExpGoldCollector m_collector;
    private List<Transform> m_targets;
    private List<Vector3> m_targetPosition;
    private NavMeshAgent m_agent;

    // Use this for initialization
    void Start ( )
    {
        m_collector = GetComponentInChildren<ExpGoldCollector>();
        m_targets = new List<Transform>();
        m_targetPosition = new List<Vector3>();
        m_animator = GetComponent<Animator>();
        m_property = GetComponent<BaseProperty>();
        m_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update ( )
    {
        /*     *
                 * 
                    * 
                       * 
                          * 
                              * 
                                  *  
            'R'                      ---------cd---------
         */

        if ((Input.GetKeyDown(KeyCode.R) || launched) && m_property.curMana > manaCost && curCd <= 0)
        {
            launched = false;
            curCd = cd;
            List<GameObject> _keys = new List<GameObject>(m_collector.enemies.Keys);
            m_property.UseMana(manaCost);
            m_animator.SetBool("r", true);
            for (int i = 0; i < _keys.Count; i++)
            {
                float _dis = Vector3.Distance(_keys[i].transform.position, transform.position);
                if (_dis < range)
                {
                    m_targets.Add(_keys[i].transform);
                    // the direction of knocking back
                    Vector3 _dir = (_keys[i].transform.position - transform.position).normalized;
                    // the enemy will be knocked back to here
                    m_targetPosition.Add(_dir * knockBackDis + m_targets[i].position);

                    // Knockback adnormalstate
                    AdnormalState _adstt = new AdnormalState(GameCode.AdnormalStateCode.KnockBack, 0.5f, 0.0f);
                    m_collector.enemies[_keys[i]].SetAdnormalState(_adstt);
                    m_collector.enemies[_keys[i]].Damaged(atk);
                }
            }
        }


        if (curCd > 0)
        {
            curCd -= Time.deltaTime;
        }

        AnimatorStateInfo _asi = m_animator.GetCurrentAnimatorStateInfo(0);
        if (_asi.IsName("Base.r"))
        {
            m_animator.SetBool("r", false);
            m_agent.Stop();
            // enemy that closed to zhaoxin will be knocked back
            if (_asi.normalizedTime > 0.4f)
            {
                for (int i = 0; i < m_targets.Count; i++)
                {
                    m_targets[i].position = Vector3.Lerp(m_targets[i].position, m_targetPosition[i], 0.5f * Time.deltaTime);
                }
            }
        }
    }
}
