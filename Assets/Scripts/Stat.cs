using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Stat")]
public class Stat : ScriptableObject
{
    public float BaseAmount;
    public float AddAmount;
    public float MultiplyAmount;
    public float ExponentAmount;

    public void ResetStats(float Base)
    {
        BaseAmount = Base;
        AddAmount = 0;
        MultiplyAmount = 1;
        ExponentAmount = 1;
    }

    public float GetStat()
    {
        return Mathf.Pow((BaseAmount + AddAmount) * MultiplyAmount, ExponentAmount);
    }
}
