using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Sprite[] Sprites;
    private SpriteRenderer Renderer;
    public SpriteRenderer IconRenderer;
    public Item GiveItem;
    private int HitsRemaining;

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        HitsRemaining = Sprites.Length;
        Renderer.sprite = Sprites[0];
        IconRenderer.sprite = GiveItem.Icon;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() && collision.gameObject.layer == 9)
        {
            HitsRemaining--; 
        }

        if (HitsRemaining <= 0)
        {
            FindObjectOfType<Player>().Profile.AddItem(GiveItem);
            FindObjectOfType<GameManager>().ItemUpdate(GiveItem);
            Destroy(gameObject);
        }
        else
        {
            Renderer.sprite = Sprites[Sprites.Length - HitsRemaining];
            foreach (Transform c in transform)
            {
                if (c.GetComponent<ConstantRotate>())
                {
                    c.GetComponent<ConstantRotate>().RotateSpeed += Random.Range(-10f, 10f);
                }
            }
        }
    }
}
