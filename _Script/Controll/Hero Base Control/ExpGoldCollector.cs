using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExpGoldCollector : MonoBehaviour
{

    private HeroProperty m_property;
    [System.NonSerialized]
    public Dictionary<GameObject, BaseProperty> enemies;
    private List<GameObject> m_leaveGo;
    private float m_radius;

    // Use this for initialization
    void Start ( )
    {
        m_property = GetComponentInParent<HeroProperty>();
        enemies = new Dictionary<GameObject, BaseProperty>();
        m_leaveGo = new List<GameObject>();
        m_radius = GetComponent<SphereCollider>().radius + 2;
    }

    // Update is called once per frame
    void Update ( )
    {
        foreach (var _kv in enemies)
        {
            float _dis = Vector3.Distance(_kv.Key.transform.position, transform.position);
            if (_dis > m_radius)
            {
                m_leaveGo.Add(_kv.Key);
            }
            else if (_kv.Value.hasDead)
            {
                //m_property.gold += _kv.Value.valueGold;
                m_property.exp += _kv.Value.valueExp;
                m_leaveGo.Add(_kv.Key);
            }
        }
        for (int i = 0; i < m_leaveGo.Count; i++)
        {
            enemies.Remove(m_leaveGo[i]);
            m_leaveGo.RemoveAt(i);
        }
    }

    public void OnTriggerEnter (Collider _c)
    {
        if ((transform.parent.tag == "blue" && _c.tag == "red") ||
            (transform.parent.tag == "red" && _c.tag == "blue"))
        {
            BaseProperty _p = _c.gameObject.GetComponent<BaseProperty>();
            if (_p != null && !_p.hasDead && !enemies.ContainsKey(_c.gameObject))
            {
                enemies.Add(_c.gameObject, _p);
                print(_c.gameObject.name);
            }
        }
    }


}
