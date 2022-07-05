using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profiles/Player Profile")]
public class PlayerProfile : ScriptableObject
{
    public Transform PlayerTransform;

    public enum StatKey
    {
        HorizontalMovement,
        MaxFlightTime,
        VerticalMovement,
        MaxHealth,
        Luck
    }

    public Stat HorizontalMovement;
    public Stat MaxFlightTime;
    public Stat VerticalMovement;
    public Stat MaxHealth;
    public Stat Luck;
    public Stat ItemChance;

    public float ST_HorizontalMovement;
    public float ST_MaxFlightTime;
    public float ST_VerticalMovement;
    public int ST_MaxHealth;
    public int ST_Luck;

    public List<Item> HeldItems = new List<Item>();

    public void ResetStats()
    {
        HeldItems.Clear();
        HorizontalMovement.ResetStats(ST_HorizontalMovement);
        MaxFlightTime.ResetStats(ST_MaxFlightTime);
        VerticalMovement.ResetStats(ST_VerticalMovement);
        MaxHealth.ResetStats(ST_MaxHealth);
    }

    public float GetStat(StatKey Key)
    {
        switch (Key)
        {
            case StatKey.HorizontalMovement:
                return HorizontalMovement.GetStat();
            case StatKey.MaxFlightTime:
                return MaxFlightTime.GetStat();
            case StatKey.VerticalMovement:
                return VerticalMovement.GetStat();
            case StatKey.MaxHealth:
                return MaxHealth.GetStat();
            default:
                return 0;
        }
    }

    /// <param name="Level">Must be between 1 and 3 (inclusive)
    /// 1 : Add to base
    /// 2 : Multiply base
    /// 3 : Exponent base</param>
    public void IncreaseStat(StatKey Key, float Amount, int Level)
    {
        switch (Key)
        {
            case StatKey.HorizontalMovement:
                IncStat(HorizontalMovement, Amount, Level);
                break;
            case StatKey.MaxFlightTime:
                IncStat(MaxFlightTime, Amount, Level);
                break;
            case StatKey.VerticalMovement:
                IncStat(VerticalMovement, Amount, Level);
                break;
            case StatKey.MaxHealth:
                IncStat(MaxHealth, Amount, Level);
                break;
        }
    }

    void IncStat(Stat S, float A, int L)
    {
        switch (L)
        {
            case 1:
                S.AddAmount = S.AddAmount + A;
                break;
            case 2:
                S.MultiplyAmount = S.MultiplyAmount + A;
                break;
            case 3:
                S.ExponentAmount = S.ExponentAmount + A;
                break;
        }
    }

    public void AddItem(Item ItemAdd)
    {
        HeldItems.Add(ItemAdd);
        ItemAdd.OnPickup(this);
    }

    public void ProcOnHit(GameObject Attacker)
    {
        foreach (Item I in HeldItems)
        {
            I.OnHit(this, Attacker);
        }
    }
}
