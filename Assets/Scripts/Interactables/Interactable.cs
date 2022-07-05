using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public float HoldTime;
    float TimeStamp;
    bool PlayerInside;
    Text PlayerText;

    void Start()
    {
        PlayerText = FindObjectOfType<Player>().GetComponentInChildren<Text>();
    }

    public virtual void Interact() { }

    void Update()
    {
        if (PlayerInside)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TimeStamp = Time.time + HoldTime;
            }
            if (Input.GetMouseButton(0))
            {
                PlayerText.text = (Mathf.Clamp(TimeStamp - Time.time, 0.0f, float.MaxValue) + 0.001f).ToString().Substring(0, 3);
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (Time.time >= TimeStamp)
                {
                    Interact();
                }

                PlayerText.text = "";
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            PlayerInside = true;
            other.GetComponentInChildren<Weapon>().NoCombat = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            PlayerInside = false;
            TimeStamp = float.MaxValue;
            other.GetComponentInChildren<Weapon>().NoCombat = false;
        }
    }
}
