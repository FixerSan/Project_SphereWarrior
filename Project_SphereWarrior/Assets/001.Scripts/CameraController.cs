using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 rotate;
    public float rotateForce;


    private void Awake()
    {
        rotate = new Vector3();
        rotate.x = transform.localEulerAngles.x;
        rotate.y = transform.localEulerAngles.y;
    }

    private void LateUpdate()
    {
        Rotation();
    }

    private void Rotation()
    {
        rotate.x = math.clamp(rotate.x - (Managers.Input.swipeDirection.y * Managers.Input.swipeForce) * Time.deltaTime * rotateForce, -90f, 90f);
        rotate.y += Managers.Input.swipeDirection.x * Managers.Input.swipeForce * Time.deltaTime * rotateForce;

        transform.localEulerAngles = rotate;
    }
}
