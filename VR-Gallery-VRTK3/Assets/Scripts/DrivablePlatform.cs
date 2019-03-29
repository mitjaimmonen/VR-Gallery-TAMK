using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DrivablePlatform : VRTK_InteractableObject {

	[SerializeField] private float speed = 1f;

	VRTK_ControllerEvents leftController;
	VRTK_ControllerEvents rightController;
	VRTK_StraightPointerRenderer leftPointer;
	VRTK_StraightPointerRenderer rightPointer;
	Color color;

	void Start(){
		color = gameObject.GetComponent<MeshRenderer> ().material.color;
	}

	bool getControllers() {
		GameObject lgo = VRTK_DeviceFinder.GetControllerLeftHand ();
		GameObject rgo = VRTK_DeviceFinder.GetControllerRightHand ();
		if (lgo == null || rgo == null) {
			return false;
		}
		leftController = lgo.GetComponent<VRTK_ControllerEvents> ();
		rightController = rgo.GetComponent<VRTK_ControllerEvents> ();
		leftPointer = lgo.GetComponent<VRTK_StraightPointerRenderer> ();
		rightPointer = rgo.GetComponent<VRTK_StraightPointerRenderer> ();
		return true;
	}

	void Update () {
		if (leftController == null || rightController == null) {
			if (!getControllers()) {
				return;
			}
		}

		Transform usingObject = null;

		if (leftController.touchpadPressed) {
			//Debug.Log ("left pressed");
			usingObject = leftController.transform.parent;
		} else if (rightController.touchpadPressed) {
			//Debug.Log ("right pressed");
			usingObject = rightController.transform.parent;
		}
		if (usingObject != null) {
			Vector3 pos = Quaternion.AngleAxis (usingObject.transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward * speed * Time.deltaTime;
			transform.position += pos;
			leftPointer.validCollisionColor = color;
			rightPointer.validCollisionColor = color;
			leftPointer.invalidCollisionColor = color;
			rightPointer.invalidCollisionColor = color;
			GameObject.Find ("[CameraRig]").transform.position += pos;
		}
	}
}

/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DrivablePlatform : VRTK_InteractableObject {

	[SerializeField] private float speed = 1f;

	VRTK_ControllerEvents leftController;
	VRTK_ControllerEvents rightController;

	void Start() {
		leftController = VRTK_DeviceFinder.GetControllerLeftHand (true).GetComponent<VRTK_ControllerEvents> ();
		rightController = VRTK_DeviceFinder.GetControllerRightHand (true).GetComponent<VRTK_ControllerEvents> ();
	}

	void Update () {
		Transform usingObject = null;
		if (leftController.touchpadPressed) {
			Debug.Log ("left pressed");
			usingObject = leftController.transform.parent;
		} else if (rightController.touchpadPressed) {
			Debug.Log ("right pressed");
			usingObject = rightController.transform.parent;
		}
		if (usingObject != null) {
			Vector3 pos = Quaternion.AngleAxis (usingObject.transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward * speed * Time.deltaTime;
			transform.position += pos;
			GameObject.Find("[CameraRig]").transform.position += pos;
		}
	}
}
*/
