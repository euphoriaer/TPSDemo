using UnityEngine;

public  abstract class Weapon : ScriptableObject
{

    public float hurt;
    [Header("Spawner Settings")]
    public GameObject projectilePrefab;

    [Header("Particles")]
    public ParticleSystem spawnParticles;

    [Header("Audio")]
    public AudioClip shoot;

    public AudioSource spawnAudioSource;

    public  Weapon()
    {
    }

    public virtual void Fire(params object[] message)
    {
    }
    public virtual void Stop(params object[] message)
    {
    }

    public virtual void Active(params object[] message)
    {
        bool isActive = (bool)message[0];
        this.Active(isActive);
    }
}