using UnityEngine;
using System.Collections;

public class SapphireCrystal : ItemBase
{

    public int extraMana = 200;

    // Use this for initialization
    void Start ( )
    {
        GetHeroProperty();
    }

    // Update is called once per frame
    void Update ( )
    {

    }

    public override void ItemStats ( )
    {
        if (m_property == null)
            GetHeroProperty();

        float _r = (float)m_property.curMana / (float)m_property.maxMana;
        m_property.maxMana += extraMana;
        m_property.curMana = (int)(m_property.maxMana * _r);
    }

    public override void UndoItemStats ( )
    {
        if (m_property == null)
            GetHeroProperty();

        float _r = (float)m_property.curMana / (float)m_property.maxMana;
        m_property.maxMana -= extraMana;
        m_property.curMana = (int)(m_property.maxMana * _r);

    }
}
