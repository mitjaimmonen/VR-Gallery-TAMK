using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour {

	public PostProcessProfile defaultProfile;

	private PostProcessProfile currentProfile;
	private PostProcessProfile newProfile;


	VRTK.VRTK_SDKManager _sdkManager;
	VRTK.VRTK_SDKManager sdkManager
	{
		get
		{
			if (!_sdkManager)
				_sdkManager = GameMaster.Instance.SDKManager();
			
			return _sdkManager;
		}
	}

	Transform _sdkCamera;
	Transform SdkCamera
	{
		get
		{
			if (!_sdkCamera)
			{
				_sdkCamera = VRTK.VRTK_DeviceFinder.HeadsetCamera();
				Debug.Log(_sdkCamera);
			}

			return _sdkCamera;
		}
	}

	// In camera.
	PostProcessLayer ppLayer 
	{
		get
		{ 
			if (SdkCamera)
				return SdkCamera.GetComponent<PostProcessLayer>();
			else
				return null;
				
		}
	}

	//THe thing that has profile attached
	PostProcessVolume ppVolume
	{
		get
		{ 
			if (SdkCamera)
				return SdkCamera.GetComponentInChildren<PostProcessVolume>();
			else
				return null;
				
		}
	}

	public void SetProfile(PostProcessProfile profile)
	{
		newProfile = profile;
	}

	public void ResetProfile()
	{
		newProfile = defaultProfile;
	}

	private void Update()
	{
		if (currentProfile == null)
		{
			if (ppVolume)
			{
				currentProfile = ppVolume.profile;
			}
		}
		if (currentProfile != newProfile)
		{
			if (ppVolume)
			{
				ppVolume.profile = newProfile;
				currentProfile = newProfile;
				
			}
		}
	}
}
