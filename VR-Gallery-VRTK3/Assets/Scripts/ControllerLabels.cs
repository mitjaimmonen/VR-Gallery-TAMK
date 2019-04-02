using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerLabels : MonoBehaviour {

	public GameObject controller;

	// Use this for initialization
	void Start () {
		//transform.position = VRTK_DeviceFinder.GetControllerRightHand (true).transform.Find ("touchpad").transform.GetChild (0).position;
		StartCoroutine(lookForControllers());

	}

	IEnumerator lookForControllers (){
		while (controller == null) {
			controller = VRTK_DeviceFinder.GetControllerRightHand ();
			yield return null;
		}
		SetPivot();
	}

	void SetPivot(){
		transform.position = controller.transform.parent.Find ("Model").position;
		//.Find ("trackpad").GetChild (0)
	}
}
