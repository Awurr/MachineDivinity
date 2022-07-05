using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRandimator : MonoBehaviour
{
    [Header("Animation")]
    public float FrameDuration = 0.5f;
    private float FrameTimeStamp;

    [Header("Sprites")]
    public List<Sprite> Sprites;
    public Image[] Images;

    int PrevSprite = 0;

    void Start()
    {

    }

    void Update()
    {
        if (Time.time >= FrameTimeStamp)
        {
            FrameTimeStamp = Time.time + FrameDuration;

            int RandomInt = Random.Range(0, Sprites.Count);
            while (RandomInt == PrevSprite)
            {
                RandomInt = Random.Range(0, Sprites.Count);
            }

            for (int i = 0; i < Images.Length; i++)
            {
                // Change Sprites
                Images[i].sprite = Sprites[RandomInt];
                PrevSprite = RandomInt;
            }
        }
    }
}
