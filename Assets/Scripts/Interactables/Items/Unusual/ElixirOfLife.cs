using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Unusual/Elixir of Life")]
public class ElixirOfLife : Item
{
    public override void OnPickup(PlayerProfile Profile)
    {
        base.OnPickup(Profile);
        Profile.IncreaseStat(PlayerProfile.StatKey.MaxHealth, 1, 1);
        Profile.PlayerTransform.GetComponent<Player>().Heal(1);
        FindObjectOfType<GameManager>().UpdateHealth();
    }

    public override void OnDrop(PlayerProfile Profile)
    {
        base.OnDrop(Profile);
        Profile.IncreaseStat(PlayerProfile.StatKey.MaxHealth, -1, 1);
        Profile.PlayerTransform.GetComponent<Player>().Heal(-1);
        FindObjectOfType<GameManager>().UpdateHealth();
    }
}
