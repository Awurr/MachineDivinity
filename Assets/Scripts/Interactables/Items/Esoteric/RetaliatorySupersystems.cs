using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Esoteric/Retaliatory Supersystems")]
public class RetaliatorySupersystems : Item
{
    public GameObject Missile;
    public int MissileAmount;
    public float MissileDelay;
    private float TimeStamp;

    public override void OnHit(PlayerProfile Profile, GameObject HitBy)
    {
        base.OnHit(Profile, HitBy);
        Profile.PlayerTransform.GetComponent<Player>().StartCoroutine(Profile.PlayerTransform.GetComponent<Player>().MissileFlurry(Missile, MissileAmount, MissileDelay, HitBy.transform));
    }
}
