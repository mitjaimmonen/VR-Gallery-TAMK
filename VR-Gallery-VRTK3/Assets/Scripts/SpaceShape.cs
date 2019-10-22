using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SpaceShape : MonoBehaviour {

	[SerializeField] private ParticleSystem ps;
	//[SerializeField] private AudioClip[] throwSounds;
	[SerializeField] private AudioClip[] holdSounds;
	[SerializeField] private AudioClip[] explosionSounds;
	[SerializeField] private AudioClip[] collisionSounds;
	private Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource> ();
	private GameObject reflection;

	void Awake(){
		VRTK_InteractableObject io = GetComponent<VRTK_InteractableObject>();
		io.InteractableObjectGrabbed += Grabbed;
		io.InteractableObjectUngrabbed += Released;


		AudioSource[] ass = GetComponentsInChildren<AudioSource> ();
		for (int i = 0; i < ass.Length; i++) {
			switch (ass[i].name) {
			case "Explosion":
				sources.Add ("explosion", ass [i]);
				sources ["explosion"].clip = explosionSounds [Random.Range (0, explosionSounds.Length)];
				break;
			case "Release":
				sources.Add ("throw", ass [i]);
				break;
			case "Hold":
				sources.Add ("hold", ass [i]);
				sources ["hold"].clip = holdSounds [Random.Range (0, holdSounds.Length)];
				break;
			case "Grab":
				sources.Add ("grab", ass [i]);
				break;
			case "Collision":
				sources.Add ("collision", ass [i]);
				sources ["collision"].clip = collisionSounds [Random.Range (0, collisionSounds.Length)];
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

	public void Grabbed(object o, InteractableObjectEventArgs e){
		sources ["grab"].Play ();
		sources ["hold"].Play ();
	}
	public void Released(object o, InteractableObjectEventArgs e){
		sources ["hold"].Stop ();
		sources ["throw"].volume = Mathf.Clamp(VRTK_DeviceFinder.GetControllerVelocity(VRTK_ControllerReference.GetControllerReference(VRTK_DeviceFinder.GetControllerIndex(e.interactingObject))).magnitude/2, 0, 1);
		sources ["throw"].Play ();
	}

	IEnumerator Destroy(){
		yield return new WaitForSeconds (3);
		Destroy (ps.gameObject);
		Destroy (gameObject);
	}
	
}
