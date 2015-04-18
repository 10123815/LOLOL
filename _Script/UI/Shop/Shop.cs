using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{

    // the player
    private GameObject m_hero;
    private HeroProperty m_property;

    // offset between mouse position and transform.position
    private Vector3 m_dragOffset;

    // 6 inventory tab, each hold a equipment puschased from shop
    public GameObject[] tabs;
    [System.NonSerialized]
    public int count;

    private Text m_gold;

    // all item prefabs holded by shop
    public GameObject[] itemPrefabs;

    // Use this for initialization
    void Start ( )
    {
        m_hero = GameObject.Find(PlayerPrefs.GetString("player name"));
        m_property = m_hero.GetComponent<HeroProperty>();
        m_gold = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update ( )
    {
        m_gold.text = "GOLD:" + m_property.gold.ToString();
    }

    public void OnDragBegin()
    {
        m_dragOffset = Input.mousePosition - transform.position;
    }

    public void OnDrag()
    {
        transform.position = Vector3.Lerp(transform.position, Input.mousePosition - m_dragOffset, Time.deltaTime * 10.0f);
    }

    // _index : the index of a item
    public void Purchase(int _index)
    {
        if (count == 6)
        {
            print("The inventory if full");
            return;
        }

        GameObject _item = itemPrefabs[_index];


        // add a item to inventory tabs
        GameObject _equipment = PoolManager.GetInstance().GetPool(_item.name, _item).GetObject();
        ItemBase _itemProperty = _equipment.GetComponent<ItemBase>();
        if (_itemProperty.price > m_property.gold)
        {
            print("We need more gold!");
            PoolManager.GetInstance().GetPool(_item.name).GivebackObject(_equipment);
            return;
        }
        int _validIndex = 0;
        for (int i = 0; i < 6; i++)
        {
            if (tabs[i].transform.childCount == 0)
            {
                _validIndex = i;
                break;
            }
        }
        _equipment.transform.SetParent(tabs[_validIndex].transform, false);
        _equipment.transform.localPosition = Vector3.zero;

        m_property.gold -= _itemProperty.price;
        _itemProperty.ItemStats();
        _itemProperty.tabs = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            _itemProperty.tabs[i] = tabs[i];
        }
        _itemProperty.parentTabIndex = _validIndex;
        count++;
    
    }

}
