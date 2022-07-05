using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorRandimator : MonoBehaviour
{
    [Header("Animation")]
    public float FrameDuration = 0.5f;
    public Transform RotateObject;
    private float FrameTimeStamp;

    void Start()
    {

    }

    void Update()
    {
        if (Time.time >= FrameTimeStamp)
        {
            FrameTimeStamp = Time.time + FrameDuration;

            RotateObject.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }
    }
}
