using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed; 

    private void Awake()
    {
        rb.AddForce(new Vector3(-3, 4f, 6), ForceMode.Impulse);   
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 collisionNormal = collision.contacts[0].normal;
        rb.velocity = CalculateReflectDirection(collisionNormal) * speed;
    }

    //�浹 �ݻ� ���� ���
    private Vector3 CalculateReflectDirection(Vector3 _collisionNormalVecter)
    {
        Vector3 nowDir = rb.velocity.normalized;
        Vector3 reflectDir = Vector3.Reflect(nowDir, _collisionNormalVecter).normalized;
        return reflectDir;
    }
}
