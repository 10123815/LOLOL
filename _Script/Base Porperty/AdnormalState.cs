using UnityEngine;
using System.Collections;
using FuckLolLib;

public class AdnormalState
{
    public GameCode.AdnormalStateCode adnmStt = GameCode.AdnormalStateCode.None;
    public float duration = 0;
    public float effect = 0;

    public AdnormalState(GameCode.AdnormalStateCode _stt, float _dur, float _e)
    {
        adnmStt = _stt;
        duration = _dur;
        effect = _e;
    }
}
