using UnityEngine;
using System.Collections;

public class Chalice : ItemBase
{

    public int extraManaRecovery = 2;

    // Use this for initialization
    void Start ( )
    {

    }

    // Update is called once per frame
    void Update ( )
    {

    }

    public override void ItemStats ( )
    {
        if (m_property == null)
            GetHeroProperty();

        m_property.manaRecoveryRate += extraManaRecovery;
    }  

    public override void UndoItemStats ( )
    {
        if (m_property == null)
            GetHeroProperty();

        m_property.manaRecoveryRate -= extraManaRecovery;
    }
}
