using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerProjectileSpawner : MonoBehaviour {


	[Header("Input")]
	public KeyCode spawnKey = KeyCode.Mouse0;


	[Header("Spawner Settings")]
	public GameObject projectilePrefab;
	public Transform spawnPoint;

	public float spawnRate;
	private float timer;


	[Header("Particles")]
	public ParticleSystem spawnParticles;


	[Header("Audio")]
	public AudioSource spawnAudioSource;


	
	void Update()
	{

		//timer += Time.deltaTime;

		//if(Input.GetKey(spawnKey) && timer >= spawnRate)
		//{
		//	SpawnProjectile();
		//}

	}

  

	public void SpawnProjectile()
	{
		
		Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

		if(spawnParticles)
		{
			spawnParticles.Play();
		}

		if(spawnAudioSource)
		{
			spawnAudioSource.Play();
		}

	}

}
