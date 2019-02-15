using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotateWithForce : MonoBehaviour {

	public Vector3 angularForce;
	public float maxAngularVelocity;
	Rigidbody rb;

	void Awake () {
		rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
		rb.AddTorque(angularForce);
		if (rb.angularVelocity.magnitude > maxAngularVelocity)
			rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, Mathf.Abs(maxAngularVelocity));
	}
}
