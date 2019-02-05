using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class CustomGrab : MonoBehaviour {

	public VRTK_ControllerEvents.ButtonAlias firstGrabButton = VRTK_ControllerEvents.ButtonAlias.GripPress;
	public bool pressAndHoldFirst;
	public bool toggleFirst;
	public VRTK_ControllerEvents.ButtonAlias secondGrabButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
	public bool pressAndHoldSecond;
	public bool toggleSecond;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
