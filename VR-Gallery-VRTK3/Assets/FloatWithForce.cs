﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatWithForce : MonoBehaviour {

	[Tooltip("Interval in which new target position is created.")]
	public float interval;
	[Tooltip("1=normal force towards target."), Range(0f,50f)]
	public float velocityMultiplier = 1f;
	[Tooltip("Target position will be within this radius of initial position."), Range(0f,50f)]
	public float maxRadius = 1f;
	float maxVelocity = 1f;
	Vector3 originalPos;
	Vector3 targetPos;
	Rigidbody rb;

	float intervalTime;
	Vector3 lerpStartPos;

	bool playing;
	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody>();
		originalPos = transform.position;
		targetPos = transform.position;
		playing = true;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (interval + intervalTime < Time.time)
		{
			targetPos = originalPos + (Random.insideUnitSphere*maxRadius);
			intervalTime = Time.time;
			lerpStartPos = transform.position;
			maxVelocity = maxRadius;
		}

		Vector3 dir =targetPos - transform.position;
		Vector3 force = dir * velocityMultiplier;
		// var vel = Vector3.Lerp(lerpStartPos,targetPos, Easing.Ease((Time.time-intervalTime) / interval, Curve.SmoothStep)) - transform.position;
		// rb.velocity = vel;
	
		rb.AddForce(Time.fixedUnscaledDeltaTime * force);

		if (rb.velocity.magnitude > maxVelocity)
			rb.velocity = rb.velocity.normalized * maxVelocity;
		if ((transform.position - originalPos).magnitude > maxRadius)
			rb.AddRelativeForce(Time.fixedUnscaledDeltaTime * dir*velocityMultiplier);
	}

	void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			originalPos = transform.position;
			targetPos = originalPos;
		}
	
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(originalPos, maxRadius);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(targetPos, 0.1f);
		
	}
}