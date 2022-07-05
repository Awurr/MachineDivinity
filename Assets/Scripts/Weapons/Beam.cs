using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Beam : MonoBehaviour
{
    public Transform BeamTransform;
    public GameObject Owner;
    public float BeamLength;
    public float BeamWidth;
    public float BeamUptime;
    public float BeamFadeTime;
    private float UpTimestamp;
    private float FadeTimestamp;
    private bool Active = true;

    void Start()
    {
        UpdateBeamPosition();
        UpTimestamp = Time.time + BeamUptime;
    }

    void Update()
    {
        if (Active)
        {
            if (Time.time >= UpTimestamp)
            {
                Active = false;
                FadeTimestamp = Time.time + BeamFadeTime;
                BeamTransform.DOScaleY(0f, BeamFadeTime);
            }
        }
        else
        {
            if (Time.time >= FadeTimestamp)
            {
                Destroy(gameObject);
            }
            else if (Time.time >= UpTimestamp + (BeamFadeTime * 0.2f))
            {
                GetComponentInChildren<BoxCollider2D>().enabled = false;
            }
        }
    }

    void UpdateBeamPosition()
    {
        Quaternion Rotation = transform.rotation;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        BeamTransform.localPosition = new Vector2(BeamLength / 4f, BeamTransform.localPosition.y);
        BeamTransform.localScale = new Vector2(BeamLength, BeamTransform.localScale.y);
        transform.rotation = Rotation;
    }
}
