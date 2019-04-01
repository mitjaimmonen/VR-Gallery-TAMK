﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShape : MonoBehaviour {

	[SerializeField] private ParticleSystem ps;
	GameObject reflection;

	public void Kill(){
		if ( GetComponent<ReflectionObj>() != null && transform.parent != GameObject.Find("Reflection Objects").transform)
		{
			GetComponent<ReflectionObj>().reflection.GetComponent<SpaceShape>().Kill();
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
	
}
