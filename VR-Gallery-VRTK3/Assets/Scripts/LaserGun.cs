using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class LaserGun : VRTK_InteractableObject {

	[Header("Laser Options", order = 4)]
	[SerializeField] private Laser prefab;
	[SerializeField] private float speed = 20f;
	[SerializeField] private float scale = 5f;
	[SerializeField] private int life = 60*3;
	[SerializeField] private Transform spawnPoint;
	private Vector3 spawnPointV;
	private VRTK_BasicTeleport teleport;
	private VRTK_ControllerEvents controllerEvents;
	private GameObject grabbingObject;

	private float minTriggerRotation = -10f;
	private float maxTriggerRotation = 45f;

	protected override void Awake()
	{
		base.Awake();
		if (!spawnPoint) {
			spawnPoint = transform;
		}
		teleport = GameMaster.Instance.SceneBehaviour ().GetComponent<VRTK_BasicTeleport> ();
		//pointer = GameMaster.Instance.GetComponent<ControllerSetter> ().GetController ().GetComponent<VRTK_Pointer>();

		//ps = gameObject.GetComponent<ParticleSystem> ();
	}

	public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
	{
		base.Grabbed(currentGrabbingObject);
		controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
		grabbingObject = GetGrabbingObject ();
		grabbingObject.GetComponent<VRTK_Pointer> ().enabled = false;
		teleport.enabled = false;
	}

	public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
	{
		base.Ungrabbed(previousGrabbingObject);
		teleport.enabled = true;
		grabbingObject.GetComponent<VRTK_Pointer> ().enabled = true;
		grabbingObject = null;
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
		var l = Instantiate(prefab, spawnPoint.position, gameObject.transform.rotation);
		l.Fire (scale, speed, life);
	}
}
