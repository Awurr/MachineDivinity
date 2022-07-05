using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuEffectPrefab : MonoBehaviour
{
    public float Uptime;
    public float Downtime;
    float Timestamp;

    void Start()
    {
        Timestamp = Time.time + Uptime;
    }

    void Update()
    {
        if (Time.time >= Timestamp)
        {
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        transform.DOScale(Vector3.zero, Downtime);
        yield return new WaitForSeconds(Downtime + 0.5f);
        //Destroy(gameObject);
    }
}
