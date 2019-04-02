using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerLabels : MonoBehaviour {

	[SerializeField] private GameObject touchpadLabel;
	[SerializeField] private GameObject triggerLabel;
	private GameObject leftController;
	private GameObject rightController;
	private VRTK_ControllerEvents leftControllerEvents;
	private VRTK_ControllerEvents rightControllerEvents;
	private Transform leftModel;
	private Transform rightModel;

	void Start(){
		StartCoroutine(FindControllers());
	}

	IEnumerator FindControllers(){
		while (leftController == null && rightController == null)
		{
			leftController = VRTK_DeviceFinder.GetControllerLeftHand();
			rightController = VRTK_DeviceFinder.GetControllerRightHand();
			yield return null;
		}
		Init();
	}

	private void Init(){
		leftControllerEvents = leftController.GetComponent<VRTK_ControllerEvents>();
		rightControllerEvents = rightController.GetComponent<VRTK_ControllerEvents>();
		leftControllerEvents.ButtonTwoPressed += EnableMenu;
		rightControllerEvents.ButtonTwoPressed += EnableMenu;
		leftModel = leftController.transform.parent.Find("Model");
		rightModel = rightController.transform.parent.Find("Model");
	}
	
	private void EnableMenu(object sender, ControllerInteractionEventArgs e)
	{
		
	}



}
