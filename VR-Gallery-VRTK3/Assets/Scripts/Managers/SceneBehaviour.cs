using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SceneBehaviour : MonoBehaviour {

	[Tooltip("Fog will lerp if changed in inspector")]
	public bool debugFog;
	public bool fog;
	public float defaultFogFadeTime;
	public float fogDensityAtStart;
	public float fogDelayAtStart;
	public bool cameraColorWithFog;
	public Color fogColor;
	[Range(0.0f,0.5f)] 
	public float fogDensity;

	private float debugDensity;


	private Color debugColor;
	private bool updatingFog;

	// Use this for initialization
	void Start () {
		RenderSettings.fogDensity = fogDensityAtStart;
		RenderSettings.fog = fog;
		RenderSettings.fogColor = fogColor;
		if (cameraColorWithFog && GameMaster.Instance.CurrentCamera)
		{
			GameMaster.Instance.CurrentCamera.backgroundColor = fogColor;
		}
		SetFog(fog, fogDensity, fogColor, defaultFogFadeTime, fogDelayAtStart);
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
		updatingFog = true;
		if (delay > 0)
			yield return new WaitForSeconds(delay);
		
		float t = 0;
		float startDensity = RenderSettings.fogDensity;
		float endDensity = fogDensity;
		if (!RenderSettings.fog)
		{
			RenderSettings.fogColor = fogColor;
			RenderSettings.fogDensity = 0;
			RenderSettings.fog = fog;
		}
		Color startFogColor = RenderSettings.fogColor;
		Color startCamColor = Color.white;
		if (cameraColorWithFog && GameMaster.Instance.CurrentCamera)
			startCamColor = GameMaster.Instance.CurrentCamera.backgroundColor;
		Color endColor = fogColor;

		while(lerpTime != 0 && t < lerpTime)
		{
			RenderSettings.fogDensity = Mathf.Lerp(startDensity, endDensity, Easing.Ease(t/lerpTime, curve));
			RenderSettings.fogColor = Color.Lerp(startFogColor, endColor, Easing.Ease(t/lerpTime, curve));
			t += Time.deltaTime;

			if (cameraColorWithFog && GameMaster.Instance.CurrentCamera)
				GameMaster.Instance.CurrentCamera.backgroundColor = Color.Lerp(startCamColor, endColor, Easing.Ease(t/lerpTime, curve));

			if (fogDensity != endDensity || fogColor != endColor)
			{
				//Starts again
				t = 0;
				startFogColor = RenderSettings.fogColor;
				startDensity = RenderSettings.fogDensity;
				endColor = fogColor;
				endDensity = fogDensity;
				if (cameraColorWithFog && GameMaster.Instance.CurrentCamera)
					startCamColor = GameMaster.Instance.CurrentCamera.backgroundColor;
			}

			yield return null;
		}

		RenderSettings.fog = fog;
		RenderSettings.fogDensity = fogDensity;
		RenderSettings.fogColor = fogColor;
		
		if (cameraColorWithFog && GameMaster.Instance.CurrentCamera)
			GameMaster.Instance.CurrentCamera.backgroundColor = fogColor;

		updatingFog = false;

	}
}
