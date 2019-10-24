using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DrivablePlatform : VRTK_InteractableObject {

	[SerializeField] private float speed = 1f;
	
	Transform controller;
	VRTK_ControllerEvents leftController;
	VRTK_ControllerEvents rightController;
	AudioSource audioSource;
	Color color;

	void Start(){
		color = GetComponent<MeshRenderer> ().material.color;
		audioSource = GetComponent<AudioSource>();
        StartCoroutine(getControllers());
	}
	IEnumerator getControllers() {
		GameObject lgo = null;
		GameObject rgo = null;
		while (lgo == null && rgo == null)
		{
			lgo = VRTK_DeviceFinder.GetControllerLeftHand ();
			rgo = VRTK_DeviceFinder.GetControllerRightHand ();
			yield return null;
		}
		leftController = lgo.GetComponent<VRTK_ControllerEvents> ();
		rightController = rgo.GetComponent<VRTK_ControllerEvents> ();
		leftController.TouchpadPressed += StartMoving;
		rightController.TouchpadPressed += StartMoving;
		leftController.TouchpadReleased += StopMoving;
		rightController.TouchpadReleased += StopMoving;
		
	}

	void StartMoving(object o, ControllerInteractionEventArgs e){
        Debug.Log("start moving");
		controller = VRTK_DeviceFinder.GetControllerByIndex(e.controllerReference.index, false).transform;
		audioSource.Play();
	}

	void StopMoving(object o, ControllerInteractionEventArgs e){
		controller = null;
		audioSource.Stop();
	}
	
	protected override void Update () {
		base.Update();

		if (controller != null) {
			VRTK_StraightPointerRenderer pointer = controller.GetComponent<VRTK_StraightPointerRenderer> ();
			Vector3 pos = Quaternion.AngleAxis (controller.transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward * speed * Time.deltaTime;
			transform.position += pos;
			pointer.validCollisionColor = color;
			pointer.invalidCollisionColor = color;
			//VRTK_DeviceFinder.HeadsetTransform.pointerActivatesUseAction += pos;
			GameObject.Find ("[CameraRig]").transform.position += pos;
		}
	}
}
