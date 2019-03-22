using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class LaserGun : VRTK_InteractableObject {

	[Header("Laser Options", order = 4)]
	[SerializeField] private Laser prefab;
	[SerializeField] private float speed = 20f;
	[SerializeField] private float scale = 5f;
	private VRTK_ControllerEvents controllerEvents;

	private float minTriggerRotation = -10f;
	private float maxTriggerRotation = 45f;

	protected override void Awake()
	{
		base.Awake();
		//ps = gameObject.GetComponent<ParticleSystem> ();
	}

	public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
	{
		base.Grabbed(currentGrabbingObject);

		Debug.Log ("grabbed");
		controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
	}

	public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
	{
		base.Ungrabbed(previousGrabbingObject);
		Debug.Log ("ungrabbed");
	}

	/*
	protected override void Update()
	{
		base.Update();
		if (controllerEvents)
		{
			var pressure = (maxTriggerRotation * controllerEvents.GetTriggerAxis()) - minTriggerRotation;
			trigger.transform.localEulerAngles = new Vector3(0f, pressure, 0f);
		}
		else
		{
			trigger.transform.localEulerAngles = new Vector3(0f, minTriggerRotation, 0f);
		}
	}
	//*/

	public override void StartUsing(VRTK_InteractUse currentUsingObject)
	{
		base.StartUsing(currentUsingObject);
		Debug.Log ("using");
		Fire ();
	}

	private void Fire()
	{
		var l = Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);
		l.Fire (scale, speed);
	}
}
