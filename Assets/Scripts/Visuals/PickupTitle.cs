using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupTitle : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    float Delay;
    public float TextDelay;
    float Timestamp;
    float TextTimestamp;
    int Index;
    Image Title;
    Text Description;
    public Color[] Colors;

    void Start()
    {
        Description = transform.parent.parent.GetComponent<Text>();
        Title = GetComponent<Image>();
        //transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(40, transform.parent.GetComponent<RectTransform>().sizeDelta.y);
        transform.parent.parent.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Time.time >= Timestamp)
        {
            Timestamp = Time.time + Delay;

            Index++;
            if (Index >= Sprites.Count)
            {
                Index = 0;
            }

            Title.sprite = Sprites[Index];
        }

        if (Time.time >= TextTimestamp)
        {
            TextTimestamp = Time.time + TextDelay;
            Description.enabled = !Description.enabled;
        }
    }

    public void SetTitle(Sprite[] S)
    {
        Sprites.Clear();
        foreach (Sprite Spr in S)
        {
            Sprites.Add(Spr);
        }

        Delay = 2.9f / S.Length;
        Index = 0;
        Title.sprite = S[0];
        Timestamp = Time.time + Delay;
        TextTimestamp = Time.time + TextDelay;
    }
}
