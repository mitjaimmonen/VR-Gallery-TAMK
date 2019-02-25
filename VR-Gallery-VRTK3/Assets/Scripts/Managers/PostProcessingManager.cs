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
	// In camera.
	PostProcessLayer ppLayer 
	{
		get
		{ 
			if (Camera.main)
				return Camera.main.GetComponent<PostProcessLayer>();
			else
				return null;
		}
	}

	//THe thing that has profile attached
	PostProcessVolume ppVolume
	{
		get
		{ 
			if (Camera.main)
				return Camera.main.GetComponentInChildren<PostProcessVolume>();
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
