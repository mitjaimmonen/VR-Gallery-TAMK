using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerSetter : MonoBehaviour {

	VRTK_SDKManager sdkManager;
	[SerializeField]GameObject leftController;
	[SerializeField]GameObject rightController;
	[SerializeField] GameObject controllerPrefab;


	
	// Update is called once per frame
	void Awake () 
	{
		if (sdkManager == null)
		{
			var temp = GameObject.FindGameObjectWithTag("SDKManager");
			sdkManager = temp.GetComponentInChildren<VRTK_SDKManager>();
		}
		if (sdkManager)
		{
			if (sdkManager.scriptAliasLeftController == null || sdkManager.scriptAliasRightController == null)
			{
				if (!leftController)
				{
					leftController = Instantiate(controllerPrefab);
					leftController.name = "leftController";
				}
				sdkManager.scriptAliasLeftController = leftController;

				if (!rightController)
				{
					rightController = Instantiate(controllerPrefab);
					rightController.name = "rightController";

				}
				sdkManager.scriptAliasRightController = rightController;
			}
		}
	}

	void Update()
	{
		if (sdkManager == null)
		{
			var temp = GameObject.FindGameObjectWithTag("SDKManager");
			sdkManager = temp.GetComponentInChildren<VRTK_SDKManager>();
		}
		if (sdkManager)
		{
			if (sdkManager.scriptAliasLeftController == null || sdkManager.scriptAliasRightController == null)
			{
				if (!leftController)
				{
					leftController = Instantiate(controllerPrefab);
					leftController.name = "leftController";
				}
				sdkManager.scriptAliasLeftController = leftController;

				if (!rightController)
				{
					rightController = Instantiate(controllerPrefab);
					rightController.name = "leftController";

				}
				sdkManager.scriptAliasRightController = rightController;
			}
		}
	}
}
