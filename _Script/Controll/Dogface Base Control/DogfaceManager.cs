using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DogfaceManager : MonoBehaviour
{

    public GameObject meleePrefab;
    public GameObject rangedPrefab;
    public int meleeNum = 3, rangedNum = 3;
    public int inter = 15;
    private bool m_hasCreated = false;

    // Use this for initialization
    void Awake ( )
    {
        
    }

    // Update is called once per frame
    void Update ( )
    {
        if ((int)Time.time % 15 == 0)
        {
            if (!m_hasCreated)
            {
                m_hasCreated = true;
                CreateDogface();
            }
        }
        else
            m_hasCreated = false;
        
    }

    private void CreateDogface ( )
    {
        for (int i = 0; i < meleeNum; i++)
        {
            GameObject _go = PoolManager.GetInstance().GetPool(meleePrefab.name, meleePrefab).GetObject(transform.position);
            _go.GetComponent<NavMeshAgent>().areaMask = (int)(Mathf.Pow(2.0f, (float)((i % 3) + 3))) + 64;
        }
        for (int i = 0; i < rangedNum; i++)
        {
            GameObject _go = PoolManager.GetInstance().GetPool(rangedPrefab.name, rangedPrefab).GetObject(transform.position);
            _go.GetComponent<NavMeshAgent>().areaMask = (int)(Mathf.Pow(2.0f, (float)((i % 3) + 3))) + 64;
        }
    }



}
