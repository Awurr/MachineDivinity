using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritemaskRandimator : MonoBehaviour
{
    [Header("Animation")]
    public float FrameDuration = 0.5f;
    private float FrameTimeStamp;

    [Header("Sprites")]
    public List<Sprite> Sprites;
    public SpriteMask[] SpriteMasks;

    void Start()
    {
        
    }

    void Update()
    {
        if (Time.time >= FrameTimeStamp)
        {
            FrameTimeStamp = Time.time + FrameDuration;

            for (int i = 0; i < SpriteMasks.Length; i++)
            {
                // Change Sprites
                SpriteMasks[i].sprite = Sprites[Random.Range(0, Sprites.Count)];
            }
        }
    }
}
