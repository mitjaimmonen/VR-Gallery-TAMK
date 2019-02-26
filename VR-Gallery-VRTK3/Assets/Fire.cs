using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public ParticleSystem pokedParticles;

	public void GetPoked(Vector3 position)
	{
		//TODO Play sound
		if (pokedParticles)
		{
			pokedParticles.transform.position = position;
			pokedParticles.Play();
		}
	}
}
