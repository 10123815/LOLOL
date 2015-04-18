using UnityEngine;
using System.Collections;

public class HeroProperty : BaseProperty
{

    public GameObject levelUpEffect;

    public int exp = 0;
    public int m_needExp = 100;
    public int level = 1;
    public int gold = 1000;

    public int lifeRecoveryRate = 1;
    public int manaRecoveryRate = 1;

    // Use this for initialization
    void Start ( )
    {

    }

    // Update is called once per frame
    void Update ( )
    {
        BaseUpdate();

        if (exp >= m_needExp)
            LevelUp();

        Cured(lifeRecoveryRate, manaRecoveryRate);
    }

    private void LevelUp()
    {
        m_needExp += 100 + level * 10; 
        level++;
        GameObject _lvupE = PoolManager.GetInstance().GetPool(levelUpEffect.name, levelUpEffect).GetObject(transform.position);
        _lvupE.transform.parent = transform;
        _lvupE.transform.localPosition = Vector3.zero;
    }
}
