using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowingKnife : MonoBehaviour {

	[SerializeField] float minImpulseToStick;
	[SerializeField] LayerMask layerMask;

	float lastCollisionTime;
	Vector3 origPos;
	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		origPos = transform.position;
	}

	void FixedUpdate()
	{
		if (!rb.isKinematic && rb.velocity.magnitude > 1f && lastCollisionTime + 0.25f < Time.time)
			rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(rb.velocity, Vector3.up), Time.fixedUnscaledDeltaTime*rb.velocity.magnitude));

		if (transform.position.magnitude > 50f)
			transform.position = origPos;
	}

	void OnCollisionStay(Collision col)
	{
		lastCollisionTime = Time.time;
	}

	void OnCollisionEnter(Collision col)
	{
		lastCollisionTime = Time.time;

		if (col.impulse.magnitude >= minImpulseToStick && 
		   (layerMask == (layerMask | (1 << col.gameObject.layer))))
		{
			rb.isKinematic = true;
			transform.position += (-col.contacts[0].normal * Mathf.Clamp(0.01f*col.impulse.magnitude,0.01f,1f));
		}
	}
}
