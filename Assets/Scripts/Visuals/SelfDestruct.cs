using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public enum DestructTypes
    {
        Timer,
        Contact,
        ContactFreeze
    }
    public DestructTypes DestructType;

    public float SelfDestructTime;

    void Start()
    {
        if (DestructType == DestructTypes.Timer)
        {
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(SelfDestructTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (DestructType == DestructTypes.Contact)
        {
            StartCoroutine(Timer());
        }
        else if (DestructType == DestructTypes.ContactFreeze)
        {
            GetComponent<Rigidbody2D>().simulated = false;
            StartCoroutine(Timer());
        }
    }
}
