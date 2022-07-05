using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Collider2D Player;
    private Collider2D Self;
    public float PlatformCooldown;
    float CooldownTimestamp;

    void Start()
    {
        Player = FindObjectOfType<Player>().GetComponent<Collider2D>();
        Self = GetComponent<CompositeCollider2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            Physics2D.IgnoreCollision(Self, Player, true);
            CooldownTimestamp = Time.time + PlatformCooldown;
        }
        else if (Time.time > CooldownTimestamp)
        {
            Physics2D.IgnoreCollision(Self, Player, false);
        }
    }
}
