using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem particle;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        particle.Play();
    }

    public void OnParticleSystemStopped()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
