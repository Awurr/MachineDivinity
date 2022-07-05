using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Damageable
{
    public Transform Aimer;
    GameObject Player;
    public Transform Base;
    public float RotateSpeed;
    public bool Attacking;
    public float TimeBetweenShots;
    public int BurstAmount;

    public override void Start()
    {
        base.Start();

        Player = FindObjectOfType<Player>().gameObject;
        SetAttackTimer(Attacks[0].AttackCooldown);
    }

    public override void Update()
    {
        base.Update();

        // Rotation
        Vector3 Direction = (Player.transform.position - transform.position).normalized;
        Quaternion Rotation = Quaternion.LookRotation(Vector3.forward, Direction);
        Aimer.transform.rotation = Quaternion.Lerp(Aimer.transform.rotation, Rotation, Time.deltaTime * RotateSpeed);

        // Movement
        Base.Rotate(new Vector3(0, 0, (MaxHealth / Health) * 250 * Time.deltaTime));

        // Attacking
        if (ReadyToAttack)
        {
            SetAttackTimer(Attacks[0].AttackCooldown);
            StartCoroutine(BurstAttack());
        }
    }

    IEnumerator BurstAttack()
    {
        for (int i = 0; i < BurstAmount; i++)
        {
            GameObject Instance = Instantiate(Attacks[0].Projectile, transform.position, Quaternion.identity);
            Instance.GetComponent<Projectile>().Damage = Attacks[0].ProjectileDamage;
            Instance.GetComponent<Rigidbody2D>().velocity = Aimer.transform.up * Attacks[0].ProjectileSpeed;
            Instance.GetComponent<Projectile>().Owner = gameObject;

            yield return new WaitForSeconds(TimeBetweenShots);
        }
    }
}
