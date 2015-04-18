using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDetector : MonoBehaviour 
{

    public GameObject enemyBase;
    private List<GameObject> m_targets;
    private DogfaceController m_ctrl;
    private string myTag;

    private bool hasInit;

	// Use this for initialization
	void Start () 
    {
        m_targets = new List<GameObject>();
        m_targets.Add(enemyBase);
        m_ctrl = transform.parent.gameObject.GetComponent<DogfaceController>();
        m_ctrl.SetTarget(enemyBase);
        myTag = transform.parent.tag;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_ctrl.hasDead)
        {
            m_targets.RemoveRange(1, m_targets.Count - 1);
            m_ctrl.SetTarget(m_targets[0]);
        }

        if (RemoveDeadEnemy())
        {
            SelectEnemy();
        }
        
    }

    // if there have any gameObject dead at m_targets
    private bool RemoveDeadEnemy()
    {
        bool d = false;
        for (int i = 0; i < m_targets.Count; i++)
        {
            if (m_targets[i].tag.Equals("dead"))
            {
                d = true;
                m_targets.RemoveAt(i);
            }
        }
        return d;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag != null && AtDifferentGroup(myTag, c.tag))
        {
            m_targets.Add(c.gameObject);
            SelectEnemy();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (m_targets.Contains(c.gameObject))
        {
            m_targets.Remove(c.gameObject);
            SelectEnemy();
        }
    }

    public void SelectEnemy()
    {
        GameObject _go = m_targets[0];
        float _minDis = Vector3.Distance(_go.transform.position, transform.position);
        for (int i = 1; i < m_targets.Count; i++)
        {
            if (!m_targets[i].activeSelf)
                continue;
            float _dis = Vector3.Distance(m_targets[i].transform.position, transform.position);
            if (m_targets[i] == m_ctrl.target)
                _dis *= 0.5f;
            if (_dis < _minDis)
            {
                _minDis = _dis;
                _go = m_targets[i];
            }
        }
        m_ctrl.SetTarget(_go);
    }

    private bool AtDifferentGroup (string a, string b)
    {
        if (a == null || b == null)
            return false;
        return (a.Equals("blue") && b.Equals("red")) || (b.Equals("blue") && a.Equals("red"));
    }
}
