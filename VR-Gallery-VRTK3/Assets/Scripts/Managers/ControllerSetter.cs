using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerSetter : MonoBehaviour {

	[SerializeField]GameObject controllerPrefab;

	VRTK_SDKManager _sdkManager;
	VRTK_SDKManager sdkManager
	{
		get
		{
			if (!_sdkManager)
				_sdkManager = GameMaster.Instance.SDKManager();
			
			return _sdkManager;
		}
	}
	GameObject leftController;
	GameObject rightController;


	
	// Update is called once per frame
	void Awake () 
	{
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
		if (sdkManager)
		{
			if (sdkManager.scriptAliasLeftController == null || sdkManager.scriptAliasRightController == null)
			{
				ResetControllers();
			}
		}
	}

	public void ResetControllers()
	{
		if (sdkManager)
		{
			if (leftController)
			{
				sdkManager.scriptAliasLeftController = null;
				Destroy(leftController.gameObject);
				leftController = Instantiate(controllerPrefab, transform.position, transform.rotation);
				leftController.transform.parent = transform;
			}
			if (rightController)
			{
				sdkManager.scriptAliasRightController = null;
				Destroy(rightController.gameObject);
				rightController = Instantiate(controllerPrefab, transform.position, transform.rotation);
				rightController.transform.parent = transform;
			}

			sdkManager.scriptAliasLeftController = leftController;
			sdkManager.scriptAliasRightController = rightController;
			
			if (sdkManager.loadedSetup)
			{
				if (sdkManager.loadedSetup.actualLeftController && sdkManager.loadedSetup.actualRightController)
				{
					leftController.transform.parent = sdkManager.loadedSetup.actualLeftController.transform;
					rightController.transform.parent = sdkManager.loadedSetup.actualRightController.transform;
					leftController.transform.localPosition = Vector3.zero;
					leftController.transform.localEulerAngles = Vector3.zero;
					rightController.transform.localPosition = Vector3.zero;
					rightController.transform.localEulerAngles = Vector3.zero;
				}
			}
		}
	}
}
