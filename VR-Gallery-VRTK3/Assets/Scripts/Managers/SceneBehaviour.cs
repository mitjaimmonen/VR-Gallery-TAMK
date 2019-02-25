using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using VRTK;

public class SceneBehaviour : MonoBehaviour {

	public PostProcessProfile pppOverride;

	[Tooltip("Fog will lerp if changed in inspector")]
	public bool debugFog;

	[Header("Fog general parameters")]
	public bool cameraColorWithFog;
	public bool fog;
	public float defaultFogFadeTime;

	[Header("Fog start parameters")]
	public Color fogColorAtStart;
	[Range(0.0f,0.5f)] 
	public float fogDensityAtStart;
	public float fogDelayAtStart;

	[Header("Fog default parameters")]
	public Color fogColor;
	[Range(0.0f,0.5f)] 
	public float fogDensity;

	private float debugDensity;
	private Color debugColor;
	private bool updatingFog;


	void Start () 
	{
		RenderSettings.fogDensity = fogDensityAtStart;
		RenderSettings.fog = fog;
		RenderSettings.fogColor = fogColorAtStart;
		if (cameraColorWithFog)
			SetCameraColor(fogColorAtStart);

		SetFog(fog, fogDensity, fogColor, defaultFogFadeTime, fogDelayAtStart);
		if (pppOverride)
			GameMaster.Instance.PostProcessingManager.SetProfile(pppOverride);
	}

	void SetCameraColor(Color col)
	{
		if (Camera.main)
		{
			Camera.main.backgroundColor = col;
		}
		else
		{
			foreach (var cam in GameMaster.Instance.AllCameras)
			{
				cam.backgroundColor = col;
			}
		}
	}

	void Update()
	{
		if (debugFog)
		{
			if (fogDensity != debugDensity || fogColor != debugColor)
			{
				debugDensity = fogDensity;
				debugColor = fogColor;
				SetFog(fog, fogDensity, fogColor, defaultFogFadeTime);
			}
		}
	}

	public void SetFog(bool state, float density, Color color, float lerpTime, float delay = 0, Curve lerpCurve = Curve.exponential)
	{
		fogDensity = density;
		fog = state;
		fogColor = color;
		if (!updatingFog)		
			StartCoroutine(UpdateFogOverTime(lerpTime, delay, lerpCurve));
	}

	private IEnumerator UpdateFogOverTime(float lerpTime, float delay, Curve curve)
	{
		float t = 0;
		float startDensity = RenderSettings.fogDensity;
		float endDensity = fogDensity;
		Color startFogColor = RenderSettings.fogColor;
		Color startCamColor = RenderSettings.fogColor;
		Color endColor = fogColor;
		updatingFog = true;

		if (delay > 0)
			yield return new WaitForSeconds(delay);
			
		if (!RenderSettings.fog)
		{
			RenderSettings.fogColor = fogColor;
			RenderSettings.fogDensity = 0;
			RenderSettings.fog = fog;
		}
		if (cameraColorWithFog)
			SetCameraColor(startCamColor);

		while(lerpTime != 0 && t < lerpTime)
		{
			RenderSettings.fogDensity = Mathf.Lerp(startDensity, endDensity, Easing.Ease(t/lerpTime, curve));
			RenderSettings.fogColor = Color.Lerp(startFogColor, endColor, Easing.Ease(t/lerpTime, curve));
			t += Time.deltaTime;

			if (cameraColorWithFog && Camera.main)
				SetCameraColor( Color.Lerp(startCamColor, endColor, Easing.Ease(t/lerpTime, curve)) );

			if (fogDensity != endDensity || fogColor != endColor)
			{
				//Starts again
				t = 0;
				startFogColor = RenderSettings.fogColor;
				startDensity = RenderSettings.fogDensity;
				endColor = fogColor;
				endDensity = fogDensity;
				if (cameraColorWithFog && Camera.main)
					startCamColor = Camera.main.backgroundColor;
			}

			yield return null;
		}

		RenderSettings.fog = fog;
		RenderSettings.fogDensity = fogDensity;
		RenderSettings.fogColor = fogColor;
		
		if (cameraColorWithFog && Camera.main)
			Camera.main.backgroundColor = fogColor;

		updatingFog = false;

	}
}
