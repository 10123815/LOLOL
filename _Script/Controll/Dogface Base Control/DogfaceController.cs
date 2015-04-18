using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FuckLolLib;

public class DogfaceController : BaseController
{

    public GameObject rangedAtkEffectPrefab;
    public bool isRanged;

    // attack range, a const float
    public float atkRange;

    private NavMeshAgent m_agent;
    private Animator m_animator;
    // atkRange + target size
    private float m_minDis;

    // attacking the target need target's BaseProperty 
    private bool m_hasAttack;

    #region charactor state
    // life bar UI on top of its head
    private GameObject m_lifeBar;
    public GameObject lifeBarPrefab;
    private Slider m_lifeSider;
    // parent of all UIs
    private Transform m_canvas;
    // base property of this go
    private BaseProperty m_property;
    // size
    private Vector3 m_size;
    // group
    private string m_tag;
    #endregion

    // when we get gameObject from pool again, we should init it again
    private bool hasInit = false;

    // Use this for initialization
    // this method will not execute again when we get the gameObject from pool
    void Start ( )
    {
        m_tag = gameObject.tag;
        Init();
    }

    // Update is called once per frame
    void Update ( )
    {
        if (!hasInit)
        {
            Init();
            return;
        }

        hasDead = m_property.curLife <= 0;
        m_animator.SetBool("death", hasDead);

        m_animator.SetBool("stun", !m_property.canMove);
        if (!m_property.canMove && m_agent.enabled)
            m_agent.Stop();
        m_agent.enabled = m_property.canMove;


        if (target == null || !m_agent.enabled || !m_property.canMove)
        {
            return;
        }

        if (m_agent.enabled)
            DisplayAnimation();
        SetUI();
    }

    private void Init ( )
    {
        gameObject.tag = m_tag;

        hasDead = false;

        m_agent = GetComponent<NavMeshAgent>();
        m_agent.radius = 0.3f;
        m_animator = GetComponent<Animator>();

        m_canvas = GameObject.Find("Canvas").transform;
        m_lifeBar = (GameObject)PoolManager.GetInstance().GetPool(lifeBarPrefab.name, lifeBarPrefab).GetObject();
        m_lifeBar.transform.SetParent(m_canvas);
        m_lifeSider = m_lifeBar.GetComponent<Slider>();
        m_lifeSider.value = 1;

        m_property = GetComponent<BaseProperty>();
        m_property.curLife = m_property.maxLife;
        m_property.hasDead = false;
        m_size = GetComponent<Collider>().bounds.size;

        hasInit = true;
    }

    public void SetTarget (GameObject _t)
    {
        target = _t;
        m_targetProperty = target.GetComponent<BaseProperty>();
        m_targetSize = target.GetComponent<Collider>().bounds.size;
        m_minDis = atkRange + m_targetSize.x;
        m_agent.stoppingDistance = atkRange;
    }

    private void DisplayAnimation ( )
    {
        AnimatorStateInfo _anmStt = m_animator.GetCurrentAnimatorStateInfo(0);
        AnimatorTransitionInfo _anmTrs = m_animator.GetAnimatorTransitionInfo(0);

        m_animator.SetBool("atk", Vector3.Distance(target.transform.position, transform.position) < m_minDis);
        if (_anmStt.IsName("Base.run"))
        {
            m_animator.speed = m_property.moveSpeed / m_property.baseMoveSpeed;
            m_agent.speed = 2 * m_animator.speed;
            if (!hasDead)
                m_agent.destination = target.transform.position;
        }
        else if (_anmStt.IsName("Base.atk"))
        {
            m_agent.ResetPath();
            float _t = _anmStt.normalizedTime;
            if (_t < 0.5f && _t > 0.3f)
            {
                if (!m_hasAttack && m_targetProperty != null)
                {
                    m_hasAttack = true;
                    if (isRanged)
                        DisplayRangedAtkEffect(rangedAtkEffectPrefab, target, m_targetSize, m_targetProperty, m_property.atk, m_size.y);
                    else
                        m_targetProperty.Damaged(m_property.atk);
                }
            }
        }
        else if (_anmStt.IsName("Base.idle"))
        {
            m_hasAttack = false;
            m_agent.ResetPath();
        }
        else if (_anmStt.IsName("Base.death"))
        {
            gameObject.tag = "dead";
            m_agent.radius = 0;
            m_animator.SetBool("atk", false);
            m_animator.SetBool("death", false);
            if (_anmStt.normalizedTime > 0.9f)
            {
                transform.position = Vector3.zero;
                hasInit = false;
                PoolManager.GetInstance().GetPool(m_lifeBar.name).GivebackObject(m_lifeBar);
                PoolManager.GetInstance().GetPool(gameObject.name).GivebackObject(gameObject);
            }
        }
        else if (_anmStt.IsName("Base.stun"))
        {
            //m_agent.Stop();
        }

        if (_anmTrs.IsUserName("2a"))
        {
            transform.LookAt(target.transform);
        }
    }

    // set life bar on its head
    private void SetUI ( )
    {
        m_lifeBar.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + m_size.y * 1.2f, transform.position.z));
        m_lifeSider.value = ((float)m_property.curLife) / ((float)m_property.maxLife);
    }
}
