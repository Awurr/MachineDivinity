using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    public float RotateSpeed;
    void Update()
    {
        transform.Rotate(0, 0, RotateSpeed * Time.deltaTime);
    }
}
