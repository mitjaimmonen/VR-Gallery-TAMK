using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DrivablePlatform : VRTK_InteractableObject {

	[SerializeField] private float speed = 1f;
	
	Transform usingObject;
	VRTK_ControllerEvents leftController;
	VRTK_ControllerEvents rightController;
	Color color;

	void Start(){
		color = gameObject.GetComponent<MeshRenderer> ().material.color;
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
		usingObject = VRTK_DeviceFinder.GetControllerByIndex(e.controllerIndex, false).transform;
	}

	void StopMoving(object o, ControllerInteractionEventArgs e){
		usingObject = null;
	}
	
	void Update () {
		if (usingObject != null) {
			VRTK_StraightPointerRenderer pointer = usingObject.GetComponent<VRTK_StraightPointerRenderer> ();
			Vector3 pos = Quaternion.AngleAxis (usingObject.transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward * speed * Time.deltaTime;
			transform.position += pos;
			pointer.validCollisionColor = color;
			pointer.invalidCollisionColor = color;
			GameObject.Find ("[CameraRig]").transform.position += pos;
		}
	}
}
