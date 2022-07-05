using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float MaxHealth;
    [HideInInspector] public float Health;
    public GameObject[] DeathDebris;
    public GameObject DebrisParticle;
    public float BloodColorVariance;
    public Color BloodColor;
    public int DeathParticles;
    public float DebrisForce;

    public AttackProfile[] Attacks;
    float AttackTimestamp;
    [HideInInspector] public bool ReadyToAttack;

    public virtual void Start()
    {
        Health = MaxHealth;

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public virtual void Update()
    {
        if (Time.time >= AttackTimestamp)
        {
            ReadyToAttack = true;
        }
    }

    public void TakeDamage(float Damage, Transform HitFrom)
    {
        Health -= Damage;

        float RemainingDamage = Damage;
        while (RemainingDamage >= 0)
        {
            CreateAndThrowParticle(DebrisParticle, HitFrom);
            RemainingDamage -= 5;
        }

        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Debris
        foreach (GameObject Debris in DeathDebris)
        {
            GameObject Instance = CreateAndThrowParticle(Debris);

            Instance.transform.rotation = new Quaternion(0, 0, Random.rotation.z, Random.rotation.w);
            Instance.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-1f, 1f) * DebrisForce * 40;
        }
        for (int i = 0; i < DeathParticles; i++)
        {
            CreateAndThrowParticle(DebrisParticle);
        }

        Destroy(gameObject);
    }

    GameObject CreateAndThrowParticle(GameObject Particle)
    {
        GameObject Instance = Instantiate(Particle, transform.position, Quaternion.identity);
        Instance.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * DebrisForce;

        Color RandColor = Randimator.GetRandomColor(BloodColor, BloodColorVariance);
        Instance.GetComponent<TrailRenderer>().startColor = RandColor;
        Instance.GetComponent<TrailRenderer>().endColor = RandColor;

        return Instance;
    }

    public static GameObject CreateAndThrowParticle(GameObject Particle, Transform transform, float DebrisForce, Color BloodColor, float BloodColorVariance)
    {
        GameObject Instance = Instantiate(Particle, transform.position, Quaternion.identity);
        Instance.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * DebrisForce;

        Color RandColor = Randimator.GetRandomColor(BloodColor, BloodColorVariance);
        Instance.GetComponent<TrailRenderer>().startColor = RandColor;
        Instance.GetComponent<TrailRenderer>().endColor = RandColor;

        return Instance;
    }

    public virtual void SpawnAnim() { }

    GameObject CreateAndThrowParticle(GameObject Particle, Transform AwayFrom)
    {
        GameObject Instance = Instantiate(Particle, transform.position, Quaternion.identity);
        Vector3 RandomVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        Vector2 ForceVector = ((transform.position - AwayFrom.position) + RandomVector).normalized;
        Instance.GetComponent<Rigidbody2D>().velocity = ForceVector * DebrisForce;

        Color RandColor = Randimator.GetRandomColor(BloodColor, BloodColorVariance);
        Instance.GetComponent<TrailRenderer>().startColor = RandColor;
        Instance.GetComponent<TrailRenderer>().endColor = RandColor;

        return Instance;
    }

    public void SetAttackTimer(float AttackCooldown)
    {
        ReadyToAttack = false;
        AttackTimestamp = Time.time + AttackCooldown;
    }
}
