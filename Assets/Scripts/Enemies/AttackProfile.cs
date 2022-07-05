using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profiles/Attack Profile")]
public class AttackProfile : ScriptableObject
{
    public GameObject Projectile;
    public float ProjectileSpeed;
    public int ProjectileDamage;
    public float AttackCooldown;
}
