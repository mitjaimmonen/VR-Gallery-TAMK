using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DrivablePlatform : VRTK_InteractableObject {

	[SerializeField] private float speed = 1f;

	VRTK_Pointer leftPointer;
	VRTK_Pointer rightPointer;

	void Start() {
		leftPointer = VRTK_DeviceFinder.GetControllerLeftHand (true).GetComponent<VRTK_Pointer> ();
		rightPointer = VRTK_DeviceFinder.GetControllerRightHand (true).GetComponent<VRTK_Pointer> ();
	}

	void Update () {
		Transform usingObject = null;
		if (leftPointer.IsActivationButtonPressed ()) {
			usingObject = leftPointer.transform.parent;
		} else if (rightPointer.IsActivationButtonPressed ()) {
			usingObject = rightPointer.transform.parent;
		}
		if (usingObject != null) {
			Vector3 pos = Quaternion.AngleAxis (usingObject.transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward * speed * Time.deltaTime;
			transform.position += pos;
			GameObject.Find("[CameraRig]").transform.position += pos;
		}
	}
}
