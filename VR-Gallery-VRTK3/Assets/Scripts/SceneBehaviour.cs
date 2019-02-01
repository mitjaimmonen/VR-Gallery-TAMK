using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBehaviour : MonoBehaviour {

	public bool fog;
	public float fogDensity;

	// Use this for initialization
	void Start () {
		RenderSettings.fogDensity = fogDensity;
	}
	
	public void SetFog(bool state, float density, float lerpTime = 0f)
	{
		fogDensity = density;
		fog = state;
		StartCoroutine(UpdateFogOverTime(lerpTime));
	}

	private IEnumerator UpdateFogOverTime(float lerpTime)
	{
		float t = 0;
		float startDensity = RenderSettings.fogDensity;

		while(RenderSettings.fogDensity != fogDensity && lerpTime != 0 && t < lerpTime)
		{
			RenderSettings.fogDensity = Mathf.Lerp(startDensity, fogDensity, t/lerpTime);
			t += Time.deltaTime;
			yield return null;
		}

		RenderSettings.fog = fog;
		RenderSettings.fogDensity = fogDensity;

	}
}
