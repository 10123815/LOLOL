using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeAndManaOnHead : LifeAndMana 
{

    private float m_heroHeight;
    private Transform m_heroTf;
    private Text m_lv;

	// Use this for initialization
	void Start () 
    {
        Get();
        m_heroHeight = hero.GetComponent<Collider>().bounds.size.y;
        m_heroTf = hero.transform;
        m_lv = GetComponentsInChildren<Text>()[2];
	}
	
	// Update is called once per frame
	void Update () 
    {
        Display();
        Vector3 _p = Camera.main.WorldToScreenPoint(new Vector3(m_heroTf.position.x, m_heroTf.position.y + m_heroHeight * 1.5f, m_heroTf.position.z));
        transform.position = Vector3.Lerp(transform.position, _p, Time.deltaTime);
        m_lv.text = property.level.ToString();
	}

}
