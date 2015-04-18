using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{

    private Vector3 m_dragOffset;

    // Use this for initialization
    void Start ( )
    {

    }

    // Update is called once per frame
    void Update ( )
    {

    }

    public void OnDragBegin ( )
    {
        m_dragOffset = Input.mousePosition - transform.position;
    }

    public void OnDrag ( )
    {
        transform.position = Vector3.Lerp(transform.position, Input.mousePosition - m_dragOffset, Time.deltaTime * 10.0f);
    }
}
