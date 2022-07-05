using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    public float TurnSpeed;
    public float MoveSpeed;
    public float MoveDelay;
    float MoveTimestamp;
    public float GiveUpAfter;
    float GiveUpTimestamp;
    public Transform Target;
    public GameObject TargetEffect;
    GameObject Instance;

    void Start()
    {
        Instance = Instantiate(TargetEffect, Target);
        Instance.transform.localPosition = Vector3.zero;
        float s = Random.Range(0.8f, 1.2f);
        Instance.transform.localScale = new Vector3(s, s, s);
        Instance.GetComponent<ConstantRotate>().RotateSpeed = Random.Range(-550f, 550f);
        GiveUpTimestamp = Time.time + GiveUpAfter;
        MoveTimestamp = Time.time + MoveDelay;
    }

    void Update()
    {
        // rotate
        if (Time.time >= MoveTimestamp && Target != null)
        {
            Vector3 Direction = Target.transform.position - transform.position;
            Quaternion Rotation = Quaternion.LookRotation(Vector3.forward, Direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, Time.deltaTime * TurnSpeed);
        }

        // move
        GetComponent<Rigidbody2D>().velocity = transform.up * MoveSpeed;

        // stop missile target
        if (GiveUpAfter > 0 && Time.time >= GiveUpTimestamp)
        {
            Target = null;

            if (Instance)
            {
                Destroy(Instance);
            }
        }

        if (!GetComponent<SpriteRenderer>().enabled)
        {
            Destroy(Instance);
        }
    }
}
