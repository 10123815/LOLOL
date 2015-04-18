using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FuckLolLib;

public class BaseProperty : MonoBehaviour
{
    public bool canMove = true;

    [System.NonSerialized]
    public bool hasDead = false;

    // one character may have kinds of adnormal states 
    [System.NonSerialized]
    public List<AdnormalState> adnormalStates;
    private Vector3 m_height;
    // when in some adnormal state, the character cannont do any action

    // the gold a hero will gain when he or she kill this character
    public int valueExp;
    public int valueGold;

    public int curLife = 200;
    public int maxLife = 200;
    public int curMana = 0;
    public int maxMana = 0;
    public int atk = 20;
    public int maxAtk = 20;
    public int def = 5;
    public int maxDef = 100;
    public int crit = 20;
    public float critRate = 0;
    public float atkSpeed = 1;
    public float baseAtkSpeed = 1;
    public float moveSpeed = 3;
    public float baseMoveSpeed = 3;
    public float atkRange = 1;

    // Use this for initialization
    void Awake ( )
    {
        adnormalStates = new List<AdnormalState>();
        for (int i = 0; i < GameCode.numberAdStt; i++)
        {
            AdnormalState _admnStt = new AdnormalState((GameCode.AdnormalStateCode)i, 0.0f, 0.0f);
            adnormalStates.Add(_admnStt);
        }
    }

    // Update is called once per frame
    void Update ( )
    {

        BaseUpdate();

    }

    protected void BaseUpdate ( )
    {
        for (int i = 0; i < adnormalStates.Count; i++)
        {
            if (i == (int)GameCode.AdnormalStateCode.KnockUp)
            {
                if (adnormalStates[i].duration > 0.0f)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * (adnormalStates[i].duration - 0.2f) * 75.0f);
                }
            }
            else if (i == (int)GameCode.AdnormalStateCode.Slow)
            {
                // slow down state launched by the hero uses corespond skill
                // recover last speed
                if (adnormalStates[i].duration < 0.0f)
                {
                    adnormalStates[i].duration = 0.0f;
                    moveSpeed *= 1 / (1 - adnormalStates[i].effect);
                }
            }
            else if (i == (int)GameCode.AdnormalStateCode.Stun)
            {

            }

            // whatever state, the duration must decrease here
            if (adnormalStates[i].duration > 0.0f)
                adnormalStates[i].duration -= Time.deltaTime;

        }

        canMove =
            (adnormalStates[(int)GameCode.AdnormalStateCode.KnockUp].duration <= 0) &&
            (adnormalStates[(int)GameCode.AdnormalStateCode.Stun].duration <= 0) &&
            (adnormalStates[(int)GameCode.AdnormalStateCode.Teleport].duration <= 0) &&
            (adnormalStates[(int)GameCode.AdnormalStateCode.KnockBack].duration <= 0);
    }

    private void LifeChange (int value)
    {
        curLife += value;
        curLife = (curLife > maxLife) ? maxLife : curLife;
        if (curLife <= 0)
        {
            curLife = 0;
            hasDead = true;
        }
    }

    public void Damaged (int d)
    {
        d = (int)(d * (1 - (float)def / (float)maxDef));
        LifeChange(-d);
    }

    public void Cured (int _l, int _r)
    {
        
        LifeChange(_l);
        ManaChange(_r);
    }

    private void ManaChange (int value)
    {
        curMana += value;
        curMana = (curMana > maxMana) ? maxMana : curMana;
        curMana = (curMana < 0) ? 0 : curMana;
    }

    public void UseMana (int value)
    {
        ManaChange(-value);
    }

    public void SetAdnormalState (AdnormalState _adnmStt)
    {
        adnormalStates[(int)_adnmStt.adnmStt] = _adnmStt;
    }


}
