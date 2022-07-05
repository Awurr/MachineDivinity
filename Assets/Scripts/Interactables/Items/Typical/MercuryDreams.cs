using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Typical/Mercury Dreams")]
public class MercuryDreams : Item
{
    public float SpeedIncrease;
    public override void OnPickup(PlayerProfile Profile)
    {
        base.OnPickup(Profile);
        Profile.IncreaseStat(PlayerProfile.StatKey.HorizontalMovement, SpeedIncrease, 1);
    }

    public override void OnDrop(PlayerProfile Profile)
    {
        base.OnDrop(Profile);
        Profile.IncreaseStat(PlayerProfile.StatKey.HorizontalMovement, -SpeedIncrease, 1);
    }
}
