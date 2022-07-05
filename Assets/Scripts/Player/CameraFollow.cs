using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraFollow : MonoBehaviour
{
    public bool Locked;
    public Room Room;
    public float XBound;
    public float YBound;
    Transform Background;
    Transform PlayerTransform;
    Vector2 CameraUpperBound;
    Vector2 CameraLowerBound;

    void OnGUI()
    {
        GetComponent<PixelPerfectCamera>().assetsPPU = (int)(Screen.height / 1080f * 50);
    }

    void Start()
    {
        PlayerTransform = FindObjectOfType<Player>().transform;
        Background = FindObjectOfType<GameManager>().Background.transform;

        SetCameraLimits(Room);
    }

    void Update()
    {
        float x;
        float y;

        if (Locked)
        {
            x = Mathf.Clamp(PlayerTransform.position.x, CameraLowerBound.x, CameraUpperBound.x);
            y = Mathf.Clamp(PlayerTransform.position.y, CameraLowerBound.y, CameraUpperBound.y);
        }
        else
        {
            x = PlayerTransform.position.x;
            y = PlayerTransform.position.y;
            Background.transform.position = PlayerTransform.position;
        }

        transform.position = new Vector3(x, y, -10);
    }

    public void SetCameraLimits(Room SetRoom)
    {
        Vector2 UpperBound = SetRoom.UpperBound.position;
        Vector2 LowerBound = SetRoom.LowerBound.position;

        CameraUpperBound = new Vector2(UpperBound.x - XBound, UpperBound.y - YBound);
        CameraLowerBound = new Vector2(LowerBound.x + XBound, LowerBound.y + YBound);
    }
}
