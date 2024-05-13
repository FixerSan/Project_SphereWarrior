using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public new Transform transform;
    public Vector3 testRotateDiection;


    private void Awake()
    {
        transform = base.transform.parent;
        testRotateDiection = new Vector3();
    }

    private void FixedUpdate()
    {
        Rotation();
    }

    private void Rotation()
    {
        testRotateDiection.y = Managers.Input.swipeDirection.x * Managers.Input.swipeForce;
        testRotateDiection.x = -Managers.Input.swipeDirection.y * Managers.Input.swipeForce;
        testRotateDiection.z = 0;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x + testRotateDiection.x, transform.eulerAngles.y + testRotateDiection.y, 0);
    }
}
