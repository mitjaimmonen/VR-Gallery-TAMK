using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobwebPS : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("PauseCobweb", 2f);
	}
	
	// Update is called once per frame
	void PauseCobweb()
	{
		GetComponent<ParticleSystem>().Pause();
	}
}
