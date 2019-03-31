using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShape : MonoBehaviour {

	[SerializeField] private ParticleSystem ps;

	GameObject reflection;

	public void Kill(){
		if (reflection != null)
		{
			reflection.GetComponent<SpaceShape>().Kill();
		}
		ps = Instantiate (ps, transform.position, Random.rotation);
		StartCoroutine (Destroy());
		gameObject.SetActive (false);
	}

	IEnumerator Destroy(){
		yield return new WaitForSeconds (3);
		Destroy (ps.gameObject);
		Destroy (gameObject);
	}

	public void SpawnReflection () {
		Vector3 pos = transform.position;
		Quaternion rot = transform.rotation;
		pos.y = -pos.y;
		rot.y = -rot.y;
		rot.w = -rot.w;
		reflection = Instantiate(gameObject, pos, rot, transform.parent);
		reflection.GetComponent<Rigidbody>().isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (reflection != null)
		{
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			pos.y = -pos.y;
			reflection.transform.position = pos;
			rot.y = -rot.y;
			rot.w = -rot.w;
			reflection.transform.rotation = rot;
		}
	}
}
