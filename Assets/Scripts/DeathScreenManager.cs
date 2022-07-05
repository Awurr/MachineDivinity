using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public Text Txt;
    public float Delay;
    float Timestamp;

    void Start()
    {
        Timestamp = Time.time + Delay;
    }

    void Update()
    {
        if (Time.time >= Timestamp)
        {
            if (!Txt.gameObject.activeSelf)
            {
                Txt.gameObject.SetActive(true);
                Txt.GetComponent<Animator>().Play("PressKey");
            }
            if (Input.anyKey)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
