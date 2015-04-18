using UnityEngine;
using System.Collections;

public class BerserkerGreaves : ItemBase
{

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

        m_property.moveSpeed *= 1.2f;
        m_property.atkSpeed *= 1.2f;
    }

    public override void UndoItemStats ( )
    {
        if (m_property == null)
            GetHeroProperty();

        m_property.moveSpeed *= 0.833f;
        m_property.atkSpeed *= 0.833f;
    }
}
