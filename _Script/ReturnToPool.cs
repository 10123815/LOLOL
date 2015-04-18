using UnityEngine;
using System.Collections;

public class ReturnToPool : MonoBehaviour 
{

    public bool autoDeath;

    // destroy gameobject after this time
    public float deathTime;

	// Use this for initialization
	void Awake () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (autoDeath)
        {
            StartCoroutine(Return(deathTime));
        }
	}

    IEnumerator Return(float _t)
    {
        yield return new WaitForSeconds(_t);
        PoolManager.GetInstance().GetPool(gameObject.name).GivebackObject(gameObject);
    }
}
