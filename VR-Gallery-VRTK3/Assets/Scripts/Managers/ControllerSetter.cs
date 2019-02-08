using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerSetter : MonoBehaviour {

	VRTK_SDKManager sdkManager;

	[SerializeField]GameObject controllerPrefab;
	[SerializeField]GameObject leftController;
	[SerializeField]GameObject rightController;


	
	// Update is called once per frame
	void Awake () 
	{
		if (sdkManager == null)
		{
			sdkManager = GetComponentInChildren<VRTK_SDKManager>();
		}
		if (sdkManager)
		{
			if (sdkManager.scriptAliasLeftController == null || sdkManager.scriptAliasRightController == null)
			{
				if (leftController == null)
				{
					leftController = Instantiate(controllerPrefab, transform.position, transform.rotation);
					leftController.transform.parent = transform;
				}
				if (rightController == null)
				{
					rightController = Instantiate(controllerPrefab, transform.position, transform.rotation);
					rightController.transform.parent = transform;
				}
				sdkManager.scriptAliasLeftController = leftController;
				sdkManager.scriptAliasRightController = rightController;
			}
		}
	}

	void Update()
	{
		if (sdkManager == null)
		{
			sdkManager = GetComponentInChildren<VRTK_SDKManager>();
		}
		if (sdkManager)
		{
			if (sdkManager.scriptAliasLeftController == null || sdkManager.scriptAliasRightController == null)
			{
				if (leftController == null)
				{
					leftController = Instantiate(controllerPrefab, transform.position, transform.rotation);
					leftController.transform.parent = transform;
				}
				if (rightController == null)
				{
					rightController = Instantiate(controllerPrefab, transform.position, transform.rotation);
					rightController.transform.parent = transform;
				}

				sdkManager.scriptAliasLeftController = leftController;
				sdkManager.scriptAliasRightController = rightController;
			}
		}
	}
}
