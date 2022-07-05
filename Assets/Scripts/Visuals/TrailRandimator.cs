using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRandimator : MonoBehaviour
{
    [Header("Animation")]
    public float FrameDuration = 0.5f;
    private float FrameTimeStamp;

    [Header("Trails")]
    public TrailRenderer[] TrailRenderers;

    void Start()
    {

    }

    void Update()
    {
        if (Time.time >= FrameTimeStamp)
        {
            FrameTimeStamp = Time.time + FrameDuration;

            for (int i = 0; i < TrailRenderers.Length; i++)
            {
                Color Col = Randimator.GetRainbowColor();

                // Change Sprites
                TrailRenderers[i].startColor = Col;
                TrailRenderers[i].endColor = Col;
            }
        }
    }
}
