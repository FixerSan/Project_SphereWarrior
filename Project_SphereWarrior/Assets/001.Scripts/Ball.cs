using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public Vector3 direction;

    private void Update()
    {
        rb.velocity = direction * speed;
    }

    public void SetRandomDirection()
    {
        direction.x = Random.Range(0, 1f);
        direction.y = Random.Range(0, 1f);
        direction.z = Random.Range(0, 1f);

        direction =  direction.normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 collisionNormal = collision.contacts[0].normal;
        direction = CalculateReflectDirection(collisionNormal) * speed;
        ParticleController particle = Managers.Resource.Instantiate("Particle_Ball_Touch", _pooling: true).GetComponent<ParticleController>();
        particle.transform.position = collision.contacts[0].point;
        particle.transform.LookAt(collision.contacts[0].normal + collision.contacts[0].point);
        particle.transform.SetParent(Managers.Object.ParticleTrans);
        particle.Play();
    }

    //충돌 반사 방향 계산
    private Vector3 CalculateReflectDirection(Vector3 _collisionNormalVecter)
    {
        Vector3 nowDir = rb.velocity.normalized;
        Vector3 reflectDir = Vector3.Reflect(nowDir, _collisionNormalVecter).normalized;
        return reflectDir;
    }
}
