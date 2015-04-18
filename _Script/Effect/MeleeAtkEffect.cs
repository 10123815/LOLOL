using UnityEngine;
using System.Collections;

public class MeleeAtkEffect : MonoBehaviour
{

    private MeleeWeaponTrail m_meleeWeaponTrail;
    private Animator m_animator;

    // Use this for initialization
    void Start ( )
    {
        m_meleeWeaponTrail = GetComponentInChildren<MeleeWeaponTrail>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ( )
    {
        AnimatorStateInfo _asi = m_animator.GetCurrentAnimatorStateInfo(0);
        if (_asi.IsTag("a") || _asi.IsName("Base.r") && _asi.normalizedTime < 0.7f)
        {
            m_meleeWeaponTrail._emit = true;
        }
        else
        {
            m_meleeWeaponTrail._emit = false;
        }
    }
}
