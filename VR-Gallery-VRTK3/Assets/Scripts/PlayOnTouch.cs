using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnTouch : MonoBehaviour {

	[SerializeField] AudioClip[] clips;
	AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		source.clip = clips[Random.Range (0, clips.Length)];
	}

	void OnCollisionEnter() {
		source.Play ();
		StartCoroutine (RandomizeSound());
	}

	IEnumerator RandomizeSound() {
		while (!source.isPlaying) {
			yield return null;
		}
		source.clip = clips[Random.Range (0, clips.Length)];
	}
}
