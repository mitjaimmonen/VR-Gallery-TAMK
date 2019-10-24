using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour {

	[SerializeField] AudioClip[] clips;
	Collider thisCollider;
	AudioSource audioSource;

	void Awake()
	{
		thisCollider = GetComponent<Collider>();
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = clips[Random.Range(0,clips.Length)];
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.GetComponent<Fire>())
		{
			
			col.GetComponent<Fire>().GetPoked(thisCollider.ClosestPoint(col.transform.position));
			audioSource.Play();
		}
	}

	IEnumerator GetNewSound(){
		while(audioSource.isPlaying){
			yield return null;
		}
		audioSource.clip = clips[Random.Range(0,clips.Length)];
	}
}
