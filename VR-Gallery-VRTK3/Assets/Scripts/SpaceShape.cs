using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShape : MonoBehaviour {

	[SerializeField] private ParticleSystem ps;
	[SerializeField] private AudioClip[] throwSounds;
	[SerializeField] private AudioClip[] holdSounds;
	[SerializeField] private AudioClip[] explosionSounds;
	private Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource> ();
	private GameObject reflection;

	void Start(){
		AudioSource[] ass = gameObject.GetComponentsInChildren<AudioSource> ();
		for (int i = 0; i < ass.Length; i++) {
			switch (ass[i].name) {
			case "Explosion":
				sources.Add ("explosion", ass [i]);
				sources ["explosion"].clip = explosionSounds [Random.Range (0, explosionSounds.Length)];
				break;
			case "Release":
				sources.Add ("throw", ass [i]);
				sources ["throw"].clip = throwSounds [Random.Range (0, throwSounds.Length)];
				break;
			case "Hold":
				sources.Add ("hold", ass [i]);
				sources ["hold"].clip = holdSounds [Random.Range (0, holdSounds.Length)];
				break;
			case "Grab":
				sources.Add ("grab", ass [i]);
				break;
			default:
				break;
			}
		}
	}

	public void Kill(){
		if ( GetComponent<ReflectionObj>() != null && transform.parent != GameObject.Find("Reflection Objects").transform)
		{
			GetComponent<ReflectionObj>().reflection.GetComponent<SpaceShape>().Kill();
		}
		sources ["explosion"].Play ();
		ps = Instantiate (ps, transform.position, Random.rotation);
		GetComponent<MeshRenderer>().enabled = false;
		StartCoroutine (Destroy());
	}

	public void Grabbed(){
		sources ["grab"].Play ();
		sources ["hold"].Play ();
	}
	public void Released(){
		sources ["hold"].Stop ();
		sources ["throw"].Play ();
		sources ["throw"].clip = throwSounds [Random.Range (0, throwSounds.Length)];
	}

	IEnumerator Destroy(){
		yield return new WaitForSeconds (3);
		Destroy (ps.gameObject);
		Destroy (gameObject);
	}
	
}
