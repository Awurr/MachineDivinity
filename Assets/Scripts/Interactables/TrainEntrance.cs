using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TrainEntrance : Interactable
{
    public Vector2 TeleportLocation;
    public float FadeTime;

    public override void Interact()
    {
        FindObjectOfType<CameraFollow>().Locked = false;
        StartCoroutine(FadeAndTP());
    }

    IEnumerator FadeAndTP()
    {
        // fade to black
        Image Underlay = GameObject.Find("MainCanvas").transform.Find("Underlay").GetComponent<Image>();
        float Time = FadeTime / 3;
        Underlay.DOColor(new Color(0, 0, 0, 1), Time);
        yield return new WaitForSeconds(Time);

        // teleport player
        GameObject Player = FindObjectOfType<Player>().gameObject;
        float TrailTime = Player.GetComponent<TrailRenderer>().time;
        Player.GetComponent<TrailRenderer>().time = 0;
        Player.transform.position = TeleportLocation;
        FindObjectOfType<GameManager>().MoveBGToPlayer();
        FindObjectOfType<GameManager>().SetBGMaterial(0);
        yield return new WaitForSeconds(Time);

        // fade to transparent
        Underlay.DOColor(new Color(0, 0, 0, 0), Time);
        Player.GetComponent<TrailRenderer>().time = TrailTime;
    }
}
