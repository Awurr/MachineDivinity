using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profiles/Weapon Profile")]
public class WeaponProfile : ScriptableObject
{
    public int Damage;
    public GameObject Projectile;
    public float ProjectileSpeed;
    public float CooldownTime;
}
