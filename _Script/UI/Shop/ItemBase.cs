using UnityEngine;
using System.Collections;

public abstract class ItemBase : MonoBehaviour
{
    // inventory
    private Transform m_parent;

    // 6 inventory tab, each hold a equipment puschased from shop
    [System.NonSerialized]
    public GameObject[] tabs;

    // offset between mouse position and transform.position
    private Vector3 m_dragOffset;
    // init position
    private Vector3 m_initPosition;
    [System.NonSerialized]
    // original parent
    public int parentTabIndex;

    // the price of this item
    public int price;

    // user property
    protected GameObject m_hero;
    protected HeroProperty m_property;
    protected void GetHeroProperty ( )
    {
        m_hero = GameObject.Find(PlayerPrefs.GetString("player name"));
        m_property = m_hero.GetComponent<HeroProperty>();
    }

    // the property of this time
    // launched when purchased
    abstract public void ItemStats ( );

    // launched when solded
    abstract public void UndoItemStats ( );

    public void OnDragBegin ( )
    {
        m_initPosition = transform.position;
        m_dragOffset = Input.mousePosition - transform.position;
        // set the parent the root, so that the draged item will be at the top of any other item
        Vector3 _p = transform.position;
        transform.SetParent(transform.parent.parent, false);
        transform.position = _p;
    }

    public void OnDrag ( )
    {
        transform.position = Vector3.Lerp(transform.position, Input.mousePosition - m_dragOffset, Time.deltaTime * 20.0f);
    }

    // if the UI is draged too far from its init position, we put it on its init position
    // otherwise, we put it on the nearest inventory tab
    public void OnDragEnd ( )
    {
        if (transform.localPosition.x > 0 || transform.localPosition.x < -160 ||
            transform.localPosition.y < 0 || transform.localPosition.y > 110)
        {
            // put it to its original position
            transform.position = m_initPosition;
            transform.SetParent(tabs[parentTabIndex].transform, false);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            
            // find the nearest tab
            int _nearest = 0;
            float _nd = Vector3.Distance(transform.position, tabs[0].transform.position);
            for (int i = 1; i < 6; i++)
            {
                float _d = Vector3.Distance(transform.position, tabs[i].transform.position);
                if (_d < _nd )
                {
                    _nd = _d;
                    _nearest = i;
                }
            }

            if (tabs[_nearest].transform.childCount > 0)
            {
                // swap tab
                Transform _child = tabs[_nearest].transform.GetChild(0);
                //_nearest.transform.DetachChildren();
                _child.SetParent(tabs[parentTabIndex].transform, false);
                _child.gameObject.GetComponent<ItemBase>().parentTabIndex = parentTabIndex;
            }
            //transform.localPosition = _nearest.transform.localPosition;
            transform.SetParent(tabs[_nearest].transform, false);
            transform.localPosition = Vector3.zero;
            parentTabIndex = _nearest;
        }
    }

}
