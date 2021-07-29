using UnityEngine;

public class GunWeapon : Weapon
{
    public float speed;

    public override void Fire(params object[] message)
    {
     
        Transform spawnPoint = message[0] as Transform;

        Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        if (spawnParticles)
        {
            spawnParticles.Play();
        }
        spawnAudioSource = projectilePrefab.GetComponent<AudioSource>();
        if (spawnAudioSource)
        {
            spawnAudioSource.clip = shoot;
            spawnAudioSource.Play();
        }
    }
}