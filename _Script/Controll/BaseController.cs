using UnityEngine;
using System.Collections;

public class BaseController : MonoBehaviour
{

    [System.NonSerialized]
    public bool hasDead;

    public GameObject target;
    protected BaseProperty m_targetProperty;
    protected Vector3 m_targetSize;


    protected void DisplayRangedAtkEffect (GameObject _effectPrefab, GameObject _target, Vector3 _size, BaseProperty _property, int _atk, float _y)
    {
        GameObject _eft = PoolManager.GetInstance().GetPool(_effectPrefab.name, _effectPrefab).GetObject();
        RangedAtkMissile _mt = _eft.GetComponent<RangedAtkMissile>();
        _mt.target = _target;
        _mt.targetSize = _size;
        _mt.targetProperty = _property;
        _mt.atk = _atk;
        _eft.transform.position = new Vector3(transform.position.x, transform.position.y + _y / 2, transform.position.z);
    }


    // if 2 gameObject in the same group
    // deciding by their tag
    public bool AtDifferentGroup (string a, string b)
    {

        return (a.Equals("blue") && b.Equals("red")) || (b.Equals("blue") && a.Equals("red"));
    }

}
