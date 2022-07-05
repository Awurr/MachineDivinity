using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage;
    public GameObject Owner;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.layer == 9)
        {
            if (!collision.gameObject.GetComponent<Player>())
            {
                if (collision.gameObject.GetComponent<Damageable>())
                {
                    collision.gameObject.GetComponent<Damageable>().TakeDamage(Damage, transform);
                }
            }
        }
        else if (gameObject.layer == 13)
        {
            if (collision.gameObject.GetComponent<Player>())
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(Damage, Owner);
            }
        }

        GetComponent<SpriteRenderer>().enabled = false;
    }
}
