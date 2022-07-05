using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextRandimator : MonoBehaviour
{
    [Header("Animation")]
    public float FrameDuration = 0.5f;
    private float FrameTimeStamp;
    public Text[] Texts;

    int PrevSprite = 0;

    void Start()
    {

    }

    void Update()
    {
        if (Time.time >= FrameTimeStamp)
        {
            FrameTimeStamp = Time.time + FrameDuration;

            foreach (Text T in Texts)
            {
                T.color = Randimator.GetRainbowColor();
            }
        }
    }
}
