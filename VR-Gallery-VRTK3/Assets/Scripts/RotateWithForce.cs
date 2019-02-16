using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotateWithForce : MonoBehaviour {

	public Vector3 angularVelocity;
	public GameObject overrideObject;
	// public float maxAngularVelocity;
	Rigidbody rb;
	Vector3 vel = Vector3.zero;

	void Awake () {
		if (!overrideObject)
			rb = GetComponent<Rigidbody>();
		else
			rb = overrideObject.GetComponent<Rigidbody>();
		if (rb)
			rb.maxAngularVelocity = 200f;
	}
	
	void FixedUpdate () {
		if (rb && !rb.isKinematic)
			rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, angularVelocity, Time.deltaTime);
		else
		{
			vel = Vector3.Lerp(vel, angularVelocity, Time.deltaTime);
			transform.Rotate(vel);
		}
	}
}
