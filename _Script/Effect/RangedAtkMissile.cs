using UnityEngine;
using System.Collections;

public class RangedAtkMissile : MonoBehaviour 
{
    public float moveSpeed;

    //[System.NonSerialized]
    public GameObject target;
    [System.NonSerialized]
    public Vector3 targetSize;
    private Vector3 targetPosition;
    [System.NonSerialized]
    public BaseProperty targetProperty;
    [System.NonSerialized]
    public int atk;

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        targetPosition = new Vector3(target.transform.position.x, target.transform.position.y + targetSize.y / 2, target.transform.position.z);
        if (!IsReached())
        {
            transform.LookAt(targetPosition);
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
        }
        else
        {
            targetProperty.Damaged(atk);
            PoolManager.GetInstance().GetPool(gameObject.name).GivebackObject(gameObject);
        }
	}

    private bool IsReached()
    {
        return Vector3.Distance(targetPosition, transform.position) < 1.0f;
    }

}
