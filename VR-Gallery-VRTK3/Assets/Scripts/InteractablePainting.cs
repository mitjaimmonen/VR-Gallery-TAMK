using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class InteractablePainting : VRTK_InteractableObject {


	[Header("Painting")]
	[SerializeField] GameObject axleObject;
	[SerializeField] bool allowFullRotation = true;
	[SerializeField] float restoreUpwardsRotationSpeed = 1f;
	[SerializeField] float touchImpactMultiplier = 5f;

	[Header("Free Floating")]

	[SerializeField] bool enableFloating = true;
	[SerializeField] Transform transformToFloat;

	[Tooltip("Interval in which new target position is created."), Range(0.1f,10f)]
	[SerializeField] float freeFloatInterval;
	[Tooltip("Target position will be within this radius of initial position."), Range(0f,5f)]
	[SerializeField] float freeFloatMaxRadius = 1f;

	[Header("Free Rotating")]

	[SerializeField] bool enableRotating = true;
	[SerializeField] float maxAngularVelocity = 0.1f;

	//Privates
	Vector3 originalPos;
	Vector3 currentStartPos;
	Vector3 targetPos;
	Vector3 torque;
	Rigidbody rb;
	float lastPositionTime;
	float lastGrabTime;

	protected override void Awake () 
	{
		base.Awake();

		rb = GetComponent<Rigidbody>();
		originalPos = transformToFloat.localPosition;
		targetPos = transformToFloat.localPosition;
		torque = Vector3.up * Random.Range(-maxAngularVelocity, maxAngularVelocity);
		rb.maxAngularVelocity = 200f;
	}
	
	protected override void Update ()
	{
		base.Update();

		if (enableFloating && transformToFloat)
		{
			HandleFloating();
		}

		if (enableRotating)
		{
			HandleRotating();
		}

		if (touchingController)
		{
			HandleTouches();
		}
	}

	GameObject touchingController;
	Vector3 controllerPos = Vector3.zero;
	Vector3 controllerVel = Vector3.zero;

	public override void OnInteractableObjectTouched(InteractableObjectEventArgs e)
	{
		base.OnInteractableObjectTouched(e);

		touchingController = e.interactingObject;
		controllerPos = touchingController.transform.position;
	}
	public override void OnInteractableObjectUntouched(InteractableObjectEventArgs e)
	{
		base.OnInteractableObjectUntouched(e);

		touchingController = null;
	}

	void HandleTouches()
	{
		if (!IsGrabbed())
		{
			controllerVel = touchingController.transform.position - controllerPos;
			controllerPos = touchingController.transform.position;

			if (rb)
			{
				rb.AddForceAtPosition(controllerVel*touchImpactMultiplier, controllerPos, ForceMode.VelocityChange);
			}
		}
	}

	void HandleFloating()
	{

		transform.position = axleObject.transform.position;

		if (!IsGrabbed())
		{
			if (freeFloatInterval + lastPositionTime < Time.time)
			{
				targetPos = originalPos + (Random.insideUnitSphere*freeFloatMaxRadius);
				currentStartPos = transformToFloat.localPosition;
				lastPositionTime = Time.time;
			}

			float lerpT = Easing.Ease((Time.time-lastPositionTime) / freeFloatInterval, Curve.SmoothStep);
			transformToFloat.localPosition = Vector3.Lerp(currentStartPos, targetPos, lerpT);
		}
		else
		{
			//Starts next lerp smoother after grab release
			currentStartPos = transformToFloat.localPosition;
			lastPositionTime = Time.time;
		}
	}

	void HandleRotating()
	{
		if (rb)
		{
			if (!IsGrabbed())
			{
				Vector3 currentTorque = Vector3.Lerp(Vector3.zero, torque, Time.time - lastGrabTime);
				rb.AddTorque(currentTorque);
				Vector3.ClampMagnitude(rb.angularVelocity, Mathf.Lerp(rb.angularVelocity.magnitude, Mathf.Max(currentTorque.y, Mathf.Abs(torque.y)), Time.deltaTime*2f));
				if (restoreUpwardsRotationSpeed > 0)
				{
					Vector3 targetRot = transform.eulerAngles;
					targetRot.x = 0;
					targetRot.z = 0;
					rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.Euler(targetRot), Time.deltaTime * restoreUpwardsRotationSpeed));
				}
			}
			else
			{
				lastGrabTime = Time.time;

				Debug.Log(rb.angularVelocity);
				//Change torque direction to same as grab caused painting to rotate
				if ((rb.angularVelocity.y > 0 && torque.y < 0) ||
					(rb.angularVelocity.y < 0 && torque.y > 0))
				{
					torque.y *= -1;
				}
			}

			if (allowFullRotation)
				rb.constraints = RigidbodyConstraints.None;
			else
				rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		}
	}

	void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			originalPos = transformToFloat.position;
			targetPos = originalPos;
		}
	
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(originalPos, freeFloatMaxRadius);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(targetPos, 0.1f);
		
	}







	
}
