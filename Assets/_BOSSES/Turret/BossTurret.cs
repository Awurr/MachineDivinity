using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurret : Damageable
{
    public Transform Rotator;
    public float RotateSpeed;
    GameObject Player;
    public Transform[] MissileLocations;
    public int MissileAmounts;
    public float MissileDelay;
    public float BeamTime;
    public Transform BeamLocation;

    public override void Start()
    {
        base.Start();

        Player = FindObjectOfType<Player>().gameObject;
    }

    public override void Update()
    {
        base.Update();

        // Rotation
        Vector3 Direction = (Player.transform.position - transform.position).normalized;
        Quaternion Rotation = Quaternion.LookRotation(Vector3.forward, Direction);
        Rotator.rotation = Quaternion.Lerp(Rotator.rotation, Rotation, Time.deltaTime * RotateSpeed);

        // Attacking
        if (ReadyToAttack)
        {
            int Rand = Random.Range(0, 2);

            switch (Rand)
            {
                case 0:
                    StartCoroutine(MissileAttack());
                    SetAttackTimer(Attacks[0].AttackCooldown);
                    break;
                case 1:
                    StartCoroutine(BeamAttack());
                    SetAttackTimer(Attacks[1].AttackCooldown);
                    break;
            }
        }
    }

    IEnumerator MissileAttack()
    {
        for (int i = 0; i < MissileAmounts; i++)
        {
            foreach (Transform Location in MissileLocations)
            {
                GameObject Instance = Instantiate(Attacks[0].Projectile, Location.position, Location.rotation);
                Instance.GetComponent<Missile>().Damage = Attacks[0].ProjectileDamage;
                Instance.GetComponent<Missile>().MoveSpeed = Attacks[0].ProjectileSpeed;
                Instance.GetComponent<Missile>().TurnSpeed = 5;
                Instance.GetComponent<Missile>().Target = Player.transform;
                Instance.GetComponent<Missile>().Owner = gameObject;

                yield return new WaitForSeconds(MissileDelay);
            }
        }
    }

    IEnumerator BeamAttack()
    {
        float Speed = RotateSpeed;
        RotateSpeed = 0;

        GameObject Instance = Instantiate(Attacks[1].Projectile, BeamLocation.position, BeamLocation.rotation);
        Instance.GetComponentInChildren<DamageField>().DamagePerSecond = Attacks[1].ProjectileDamage;
        Instance.GetComponent<Beam>().BeamUptime = BeamTime;
        Instance.GetComponent<Beam>().Owner = gameObject;

        yield return new WaitForSeconds(BeamTime);

        RotateSpeed = Speed;
    }
}
