using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;

    private void Awake()
    {
        rb.AddForce(new Vector3(-3, 4f, 5), ForceMode.Impulse);   
    }

    private void Update()
    {
        Debug.Log(rb.velocity);
    }
}
