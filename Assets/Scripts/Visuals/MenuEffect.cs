using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEffect : MonoBehaviour
{
    public GameObject Effect;
    public float EffectForce;
    public float ForceVariance;
    public float EffectRate;
    float EffectTimestamp;
    Vector3 PrevMousePos;

    void Start()
    {
        PrevMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        EffectTimestamp = Time.time + EffectRate;
    }

    void Update()
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition) != PrevMousePos)
        {
            if (Time.time >= EffectTimestamp)
            {
                Vector3 Diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - PrevMousePos;
                Vector3 Location = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 10);
                GameObject Instance = Instantiate(Effect, Location, Quaternion.identity);

                float XVariance = Random.Range(-ForceVariance, ForceVariance);
                float YVariance = Random.Range(-ForceVariance, ForceVariance);
                Vector2 Force = Diff.normalized * EffectForce;

                Instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(Force.x + XVariance, Force.y + YVariance));

                float RandScale = Random.Range(Diff.magnitude / 2, Diff.magnitude / 2 + 1);
                Instance.transform.localScale = Vector3.one * RandScale;

                EffectTimestamp = Time.time + EffectRate;
            }

            PrevMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
