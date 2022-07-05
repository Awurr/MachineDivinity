using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randimator : MonoBehaviour
{
    [Header("Animation")]
    public float FrameDuration = 0.5f;
    private float FrameTimeStamp;

    [Header("Sprites")]
    public List<Sprite> Sprites;
    public SpriteRenderer[] SpriteRenderers;

    [Header("Colors")]
    public Color BaseColor;
    public enum ColorModes
    {
        Static,
        RandomStatic,
        RandomFrame,
        Rainbow
    }
    public ColorModes ColorMode;
    public float ColorVariance; // Should be < 1.0

    void Start()
    {
        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            switch (ColorMode)
            {
                case ColorModes.Static:
                    SpriteRenderers[i].color = BaseColor;
                    break;
                case ColorModes.RandomStatic:
                    AssignRandomColor(SpriteRenderers[i]);
                    break;
            }
        }
    }

    void Update()
    {
        if (Time.time >= FrameTimeStamp)
        {
            FrameTimeStamp = Time.time + FrameDuration;

            for (int i = 0; i < SpriteRenderers.Length; i++)
            {
                // Change Sprites
                if (Sprites.Count != 0)
                {
                    SpriteRenderers[i].sprite = Sprites[Random.Range(0, Sprites.Count)];
                }

                // Change colors
                switch (ColorMode)
                {
                    case ColorModes.RandomFrame:
                        AssignRandomColor(SpriteRenderers[i]);
                        break;
                    case ColorModes.Rainbow:
                        AssignRainbowColor(SpriteRenderers[i]);
                        break;
                }
            }
        }
    }

    void AssignRandomColor(SpriteRenderer SpriteRend)
    {
        float RandR = Random.Range(-ColorVariance, ColorVariance);
        float RandG = Random.Range(-ColorVariance, ColorVariance);
        float RandB = Random.Range(-ColorVariance, ColorVariance);
        SpriteRend.color = new Color(BaseColor.r + RandR, BaseColor.g + RandG, BaseColor.b + RandB, BaseColor.a);
    }
    
    void AssignRainbowColor(SpriteRenderer SpriteRend)
    {
        float RandH = Random.Range(0f, 1f);
        SpriteRend.color = Color.HSVToRGB(RandH, 1, 1);
    }

    public static Color GetRandomColor(Color InColor, float Variance)
    {
        float RandR = Random.Range(-Variance, Variance);
        float RandG = Random.Range(-Variance, Variance);
        float RandB = Random.Range(-Variance, Variance);
        return new Color(InColor.r + RandR, InColor.g + RandG, InColor.b + RandB, InColor.a);
    }

    public static Color GetRainbowColor()
    {
        float RandH = Random.Range(0f, 1f);
        return Color.HSVToRGB(RandH, 1, 1);
    }
}
