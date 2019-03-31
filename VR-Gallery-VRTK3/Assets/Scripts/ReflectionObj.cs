using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionObj : MonoBehaviour {

	public GameObject reflection {get; private set;}
	private Transform parent;

	void Awake() {
		parent = GameObject.Find("Reflection Objects").transform;
	}

	// Use this for initialization
	void Start () {
		if (transform.parent != parent) {
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			pos.y = -pos.y;
			rot.y = -rot.y;
			rot.w = -rot.w;
			reflection = Instantiate(gameObject, pos, rot, parent);
			reflection.GetComponent<Rigidbody>().isKinematic = true;
		}
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
