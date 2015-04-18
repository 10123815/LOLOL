//-------------------------------------------------
// Basis and generic action of a character controled 
// by player, such as stop, move, tp, 
//-------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using FuckLolLib;

public class HeroController : BaseController
{

    public Texture2D[] cursors;
    private GameCode.CursorCode m_cursor = GameCode.CursorCode.Normal;

    // kinds of effect prefab
    public GameObject hit;

    public float stopDis = 2.0f;

    private NavMeshAgent m_agent;
    private float m_initSpeed;
    private Animator m_animator;
    private RaycastHit m_mouseClick;
    private HeroProperty m_property;

    private string m_tag;
    private Vector3 m_size;

    // set the position of Camera
    private Camera m_camera;
    private Transform m_mainCameraTf;
    public Vector3 offsetCamera = new Vector3(-6f, 11.0f, -0.2f);

    // leftMouseClkGo may be shop, termmate;
    // rightMouseClkGo must be enemy
    public GameObject clickGreen;
    public GameObject clickRed;
    //[System.NonSerialized]
    public GameObject leftMouseClkGo;
    //[System.NonSerialized]
    public GameObject rightMouseClkGo;
    public Vector3 targetSize;
    private Vector3 targetPosition;

    // flag of if hero has attacked
    private bool m_hasAttack = false;
    // is this hero atk ranged
    public bool isRanged = false;
    // ranged atk effect
    public GameObject rangedAtkEffectPrefab;

    // Use this for initialization
    void Start ( )
    {
        m_tag = gameObject.tag;
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        m_camera = Camera.main;
        m_mainCameraTf = Camera.main.transform;
        m_mainCameraTf.position = transform.position + offsetCamera;
        m_property = GetComponent<HeroProperty>();
        m_initSpeed = m_agent.speed;
        m_size = GetComponent<Collider>().bounds.size;

        Init();

    }

    // Update is called once per frame
    void Update ( )
    {

        if (!m_property.canMove)
            return;

        m_animator.SetBool("dead", m_property.curLife <= 0);
        hasDead = m_property.curLife <= 0;

        if (m_targetProperty != null && m_targetProperty.hasDead)
        {
            m_animator.SetBool("tgdead", true);
            m_animator.SetBool("haspath", false);
            m_animator.SetBool("fight", false);
            StopAttack();
            m_agent.Stop();
            m_targetProperty = null;
            rightMouseClkGo = null;
        }

        // smooth move the main Camera to take it follow the hero
        m_mainCameraTf.position = Vector3.Lerp(m_mainCameraTf.position, transform.position + offsetCamera, Time.deltaTime * 5);

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // if click on UI
            if (!EventSystem.current.IsPointerOverGameObject())
                HandleMouseClick();
        }

        m_animator.SetBool("stun", !m_property.canMove);
        if (!m_property.canMove && m_agent.enabled)
            m_agent.Stop();
        m_agent.enabled = m_property.canMove;


        if (!m_agent.enabled || !m_property.canMove)
        {
            return;
        }

        if (m_agent.enabled)
            DisplayAnimation();
    }



    #region private method

    private void Init ( )
    {
        gameObject.tag = m_tag;
        m_property.hasDead = false;
        hasDead = false;
        m_property.curLife = m_property.maxLife;
    }

    private void HandleMouseClick ( )
    {
        // generate a ray from main camera to mouse
        Ray _ray = m_camera.ScreenPointToRay(Input.mousePosition);
        // this ray will hit some colliders
        RaycastHit[] _hits = Physics.RaycastAll(_ray);

        #region left button click
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < _hits.Length; i++)
            {
                string _group = _hits[i].collider.tag;
                int _layer = _hits[i].collider.gameObject.layer;
                // click a shop
                if (_group.Equals("shop"))
                {
                    leftMouseClkGo = _hits[i].collider.gameObject;
                    // open shop UI there
                    break;
                }
                // only hero and dogface can be affected by a skill
                // user can not click on character and shop at the same time
                if ((_layer == 8 || _layer == 9) && (_group == "red" || _group == "blue"))
                {
                    leftMouseClkGo = _hits[i].collider.gameObject;
                    break;
                }
                // close all dynamic UI when click on terrain
                if (_group == "terrain")
                {
                    // close UI there
                    continue;
                }
            }
        }
        #endregion

        #region right button click
        else if (Input.GetMouseButtonDown(1))
        {
            int i = 0;
            for (i = 0; i < _hits.Length; i++)
            {
                string _group = _hits[i].collider.tag;
                // right click on terrain
                if (_group == "terrain")
                {
                    targetPosition = _hits[i].point;
                    rightMouseClkGo = _hits[i].collider.gameObject;
                    SetCursor(GameCode.CursorCode.Normal);
                    continue;
                }
                // right click on a enemy
                if (AtDifferentGroup(_group, gameObject.tag))
                {
                    BaseController _ctrl = _hits[i].collider.gameObject.GetComponent<BaseController>();
                    if (_ctrl == null || _ctrl.hasDead)
                        continue;

                    // click effect
                    GameObject _clk = PoolManager.GetInstance().GetPool(clickRed.name, clickRed).GetObject(_hits[i].point);

                    targetPosition = _hits[i].collider.transform.position;
                    rightMouseClkGo = _hits[i].collider.gameObject;
                    targetSize = _hits[i].collider.bounds.size;
                    stopDis = (targetSize.x + targetSize.z) / 3.0f + m_property.atkRange;

                    if (Vector3.Distance(rightMouseClkGo.transform.position, transform.position) > stopDis)
                    {
                        m_agent.SetDestination(rightMouseClkGo.transform.position);
                        m_animator.SetBool("fight", true);
                        transform.LookAt(rightMouseClkGo.transform);
                        target = rightMouseClkGo;
                    }
                    else if (!m_animator.GetCurrentAnimatorStateInfo(0).IsTag("a"))
                    {
                        m_agent.Stop();
                        if (Random.value < m_property.critRate)
                        {
                            m_animator.SetBool("atk1", false);
                            m_animator.SetBool("crit", true);
                        }
                        else
                        {
                            m_animator.SetBool("atk1", true);
                            m_animator.SetBool("crit", false);
                        }
                        transform.LookAt(rightMouseClkGo.transform);
                        m_agent.stoppingDistance = stopDis;
                    }
                    return;
                }
            }

            if (rightMouseClkGo.tag == "terrain")
            {
                // click effect
                Vector3 _pos = new Vector3(targetPosition.x, targetPosition.y + 1, targetPosition.z);
                GameObject _clk = PoolManager.GetInstance().GetPool(clickGreen.name, clickGreen).GetObject(_pos);

                m_agent.SetDestination(targetPosition);
                m_animator.SetBool("haspath", m_agent.hasPath);
                m_animator.SetBool("stopfight", true);
                m_animator.SetBool("fight", false);
                StopAttack();
            }


        }
        #endregion

        return;
    }


    private void DisplayAnimation ( )
    {
        AnimatorStateInfo _anmStt = m_animator.GetCurrentAnimatorStateInfo(0);
        AnimatorTransitionInfo _anmTrs = m_animator.GetAnimatorTransitionInfo(0);

        #region animator state

        if (_anmStt.IsName("Base.idle"))
        {
            m_animator.speed = 1;
            m_animator.SetBool("tgdead", false);
            m_animator.SetBool("stopfight", false);
            m_animator.SetBool("haspath", m_agent.hasPath);
        }
        else if (_anmStt.IsName("Base.run"))
        {
            m_animator.SetBool("stopfight", false);
            m_animator.SetBool("haspath", m_agent.hasPath);
            m_agent.SetDestination(targetPosition);
            m_agent.stoppingDistance = 0;
        }
        else if (_anmStt.IsName("Base.fight"))
        {
            m_hasAttack = false;
            m_agent.stoppingDistance = 0;
            m_animator.SetBool("fight", false);
            StopAttack();

            m_agent.stoppingDistance = stopDis;
            if (rightMouseClkGo != null)
            {
                m_agent.SetDestination(rightMouseClkGo.transform.position);

                if (Vector3.Distance(rightMouseClkGo.transform.position, transform.position) < stopDis)
                {
                    m_agent.Stop();
                    if (Random.value < m_property.critRate)
                    {
                        m_animator.SetBool("atk1", false);
                        m_animator.SetBool("crit", true);
                    }
                    else
                    {
                        m_animator.SetBool("atk1", true);
                        m_animator.SetBool("crit", false);
                    }
                    transform.LookAt(rightMouseClkGo.transform);
                }
            }
            else
                m_animator.SetBool("tgdead", true);
        }
        else if (_anmStt.IsTag("a") || _anmStt.IsName("Base.batk"))
        {
            if (target == null || m_targetProperty == null)
            {
                m_animator.SetBool("tgdead", true);
                return;
            }

            m_agent.Stop();

            // atk speed is same as display speed of 'atk' animation
            m_animator.speed = m_property.atkSpeed / m_property.baseAtkSpeed;
            if (rightMouseClkGo.tag != "terrain")
            {
                if (Vector3.Distance(rightMouseClkGo.transform.position, transform.position) > stopDis)
                {
                    m_animator.SetBool("fight", true);
                }
            }
            else
            {
                m_animator.SetBool("fight", false);
                m_animator.SetBool("stopfight", true);
                StopAttack();
            }

            if (_anmStt.IsName("Base.atk1"))
            {
                m_animator.SetBool("atk1", false);
                // this method will be called only one time during one loop of 'atk' animation
                Attack(m_property.atk, _anmStt.normalizedTime);
            }
            else if (_anmStt.IsName("Base.atk2"))
            {
                m_animator.SetBool("atk2", false);
                Attack(m_property.atk, _anmStt.normalizedTime);
            }
            else if (_anmStt.IsName("Base.crit"))
            {
                m_animator.SetBool("crit", false);
                Attack(m_property.crit, _anmStt.normalizedTime, true);
            }
            else if (_anmStt.IsName("Base.batk"))
            {
                // between 2 times of attack
                m_hasAttack = false;
                // trans to atk1 or atk2 or crit
                if (Random.value < m_property.critRate)
                {
                    m_animator.SetBool("crit", true);
                    m_animator.SetBool("atk1", false);
                    m_animator.SetBool("atk2", false);
                }
                else
                {
                    if (Random.value < 0.5f)
                    {
                        m_animator.SetBool("atk1", true);
                        m_animator.SetBool("atk2", false);
                        m_animator.SetBool("crit", false);
                    }
                    else
                    {
                        m_animator.SetBool("atk2", true);
                        m_animator.SetBool("atk1", false);
                        m_animator.SetBool("crit", false);
                    }
                }

            }
        }
        else if (_anmStt.IsName("Base.death"))
        {
            gameObject.tag = "dead";
            StopAttack();
            m_agent.Stop();
            m_animator.SetBool("dead", false);
            m_animator.SetBool("haspath", false);
            m_animator.SetBool("fight", false);
            m_animator.SetBool("stopfight", false);
            m_animator.SetBool("tgdead", false);
            if (_anmStt.normalizedTime > 0.95f)
            {

            }
        }

        if (_anmStt.IsTag("r"))
        {
            m_hasAttack = false;
            m_agent.Resume();
            // display speed of "run" animation, is not equals to move speed of gameobject
            m_animator.speed = m_property.moveSpeed / m_property.baseMoveSpeed;
            m_agent.speed = m_initSpeed * m_animator.speed;
        }
        #endregion

        #region transition info
        if (_anmTrs.IsUserName("2r"))
        {
            m_agent.stoppingDistance = 0;
        }
        else if (_anmTrs.IsUserName("2a"))
        {
            m_agent.stoppingDistance = stopDis;
            if (rightMouseClkGo != null && rightMouseClkGo.activeSelf)
                target = rightMouseClkGo;
            transform.LookAt(target.transform);
            BaseProperty _p = target.GetComponent<BaseProperty>();
            if (_p != m_targetProperty)
                m_targetProperty = _p;
        }
        #endregion
    }

    private void StopAttack ( )
    {
        m_animator.SetBool("atk1", false);
        m_animator.SetBool("atk2", false);
        m_animator.SetBool("crit", false);
    }


    // _t is the normalized progress of 'atk' animation
    private void Attack (int _v, float _t, bool isCrit = false)
    {
        if (_t > 0.22f && _t < 0.8f)
        {
            if (!m_hasAttack && m_targetProperty != null)
            {
                m_hasAttack = true;
                if (!isRanged)
                {
                    m_targetProperty.Damaged(_v);
                    if (m_targetProperty.hasDead)
                        m_property.gold += m_targetProperty.valueGold;
                    DisplayAttackEffect(isCrit);
                }
                else
                    DisplayRangedAtkEffect(rangedAtkEffectPrefab, target, targetSize, m_targetProperty, _v, m_size.y);
            }
        }
    }

    // play the particle effect
    private void DisplayAttackEffect (bool isCrit)
    {
        // atk and crit has different effect
        GameObject _hit = (GameObject)PoolManager.GetInstance().GetPool(hit.name, hit).GetObject();
        Vector3 _pos = target.transform.position;
        _hit.transform.position = new Vector3(_pos.x, _pos.y + targetSize.y / 2, _pos.z);
    }
    #endregion

    #region public method

    public void SetCursor (GameCode.CursorCode _code)
    {
        m_cursor = _code;
        Cursor.SetCursor(cursors[(int)m_cursor], Vector2.zero, CursorMode.Auto);
    }


    #endregion

}
