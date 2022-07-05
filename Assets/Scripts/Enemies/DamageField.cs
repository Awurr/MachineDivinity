using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    public float DamagePerSecond;
    float DamageTimestamp;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            if (Time.time >= DamageTimestamp)
            {
                DamageTimestamp = Time.time + 1;
                other.GetComponent<Player>().TakeDamage((int)DamagePerSecond, null);
            }
        }
    }
}
