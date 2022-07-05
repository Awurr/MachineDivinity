using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite[] Titles;
    public GameManager.ItemTypes Type;
    public Sprite Icon;
    public Item InvertTo;
    public Color DescriptionColor;

    /// <summary>
    /// <c>OnPickup</c> is called when item is added to the player's inventory (i.e. stat changes)
    /// </summary>
    public virtual void OnPickup(PlayerProfile Profile)
    {

    }

    /// <summary>
    /// <c>OnHold</c> is called every frame the item is held
    /// </summary>
    public virtual void OnHold(PlayerProfile Profile)
    {

    }

    /// <summary>
    /// <c>OnDrop</c> is called when item is removed from the player's inventory (i.e. stat changes)
    /// </summary>
    public virtual void OnDrop(PlayerProfile Profile)
    {

    }

    /// <summary>
    /// <c>OnHit</c> is called when the player takes damage
    /// </summary>
    public virtual void OnHit(PlayerProfile Profile, GameObject HitBy)
    {

    }

    /// <summary>
    /// <c>OnKill</c> is called when the player defeats an enemy
    /// </summary>
    public virtual void OnKill(PlayerProfile Profile, GameObject HitBy)
    {

    }

    /// <summary>
    /// <c>OnClear</c> is aclled when a room is cleared
    /// </summary>
    public virtual void OnClear(PlayerProfile Profile)
    {
        
    }
}
