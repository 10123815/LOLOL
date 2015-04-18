using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//-------------------------------------------------------
// PoolManager is a singleton, which holds some object pool,
// if we need a pool to get object, we must know the name
// of the pool, so we can use this name to get the pool we
// want from PoolManager
//-------------------------------------------------------
public class PoolManager
{

    static private PoolManager m_instance;

    static private Dictionary<string, ObjectPool> m_pools;

    // use this method to get the singleton
    static public PoolManager GetInstance ( )
    {
        if (m_instance == null)
        {
            m_instance = new PoolManager();
            m_pools = new Dictionary<string, ObjectPool>();
        }
        return m_instance;
    }

    public ObjectPool GetPool (string poolName, GameObject o)
    {
        if (!m_pools.ContainsKey(poolName))
        {
            // if it is a new object, we must create a new pool for it
            ObjectPool _pool = new ObjectPool(poolName, o);
            m_pools.Add(poolName, _pool);
            return _pool;
        }
        return m_pools[poolName];
    }

    // we should explicitly know this pool exiting
    public ObjectPool GetPool (string poolName)
    {
        return m_pools[poolName];
    }



    //--------------------------------------------------------------
    // A ObjectPool holds some GameObject with same type, by a List. 
    // Active GameObjects are in first part of the List, unactive 
    // GameObjects, the availables, are in second part, so if we need
    // a GameObjects, we get it from the end of the List
    //--------------------------------------------------------------
    public class ObjectPool
    {
        // this list is like this:
        // ---------------------|---------------------
        //  active(unavailable)   unactive(available)
        private List<GameObject> m_objects;
        private int m_total;
        private int m_activeNum;

        private string m_name;
        private GameObject m_original;

        public ObjectPool (string _n, GameObject _o)
        {
            m_objects = new List<GameObject>();
            m_name = _n;
            m_original = _o;
            m_total = 0;
            m_activeNum = 0;
        }

        private void CreateObject ( )
        {
            m_total++;
            GameObject _go = (GameObject)Object.Instantiate(m_original);
            int _l = _go.name.Length;
            if (_go.name[_l - 1].Equals(')'))
                _go.name = _go.name.Remove(_l - 7);
            _go.SetActive(false);
            m_objects.Add(_go);
        }

        public GameObject GetObject ( )
        {
            if (m_activeNum >= m_total)
                CreateObject();

            m_activeNum++;
            m_objects[m_activeNum - 1].SetActive(true);
            return m_objects[m_activeNum - 1];
        }

        // get a object and set it to given position before activing it
        public GameObject GetObject (Vector3 _orgPos)
        {
            if (m_activeNum >= m_total)
                CreateObject();

            m_activeNum++;
            m_objects[m_activeNum - 1].transform.position = _orgPos;
            m_objects[m_activeNum - 1].SetActive(true);
            return m_objects[m_activeNum - 1];
        }

        // return the object back to the pool
        public void GivebackObject (GameObject _go)
        {
            _go.SetActive(false);
            m_activeNum--;
            if (_go == m_objects[m_activeNum])
                return;

            GameObject _tmp = _go;
            _go = m_objects[m_activeNum];
            m_objects[m_activeNum] = _tmp;
        }

        public List<GameObject> GetObjects ( )
        {
            return m_objects;
        }

        public void Delete (GameObject _go)
        {
            m_objects.Remove(_go);
            _go = null;
        }
    }

}




