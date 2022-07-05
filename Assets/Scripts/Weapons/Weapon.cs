using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponProfile ActiveProfile;
    public Transform ShootPoint;
    private float CooldownTimeStamp;
    public bool NoCombat;

    void Update()
    {
        if (NoCombat)
            return;

        if (Input.GetMouseButton(0) && Time.time >= CooldownTimeStamp)
        {
            CooldownTimeStamp = Time.time + ActiveProfile.CooldownTime;

            Shoot();
        }
    }

    public void Shoot()
    {
        GameObject Instance = Instantiate(ActiveProfile.Projectile, ShootPoint.position, Quaternion.identity);
        Instance.GetComponent<Rigidbody2D>().AddForce(transform.up * ActiveProfile.ProjectileSpeed);
        Instance.GetComponent<Projectile>().Damage = ActiveProfile.Damage;
    }
}
