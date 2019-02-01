using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBehaviour : MonoBehaviour {

	[Tooltip("Fog will lerp if changed in inspector")]
	public bool debugFog;
	public bool fog;
	public Color fogColor;
	[Range(0.0f,0.5f)] 
	public float fogDensity;

	private float debugDensity;
	private Color debugColor;

	private bool updatingFog;
	// Use this for initialization
	void Awake () {
		RenderSettings.fogDensity = fogDensity;
		RenderSettings.fog = fog;
		RenderSettings.fogColor = fogColor;
	}

	void Update()
	{
		if (debugFog)
		{
			if (fogDensity != debugDensity || fogColor != debugColor)
			{
				debugDensity = fogDensity;
				debugColor = fogColor;
				SetFog(fog, fogDensity, fogColor);
			}
		}
	}
	
	public void SetFog(bool state, float density, Color color, float lerpTime = 5f, Curve lerpCurve = Curve.SmoothStep)
	{
		fogDensity = density;
		fog = state;
		fogColor = color;
		if (!updatingFog)		
			StartCoroutine(UpdateFogOverTime(lerpTime, lerpCurve));
	}

	private IEnumerator UpdateFogOverTime(float lerpTime, Curve curve)
	{
		updatingFog = true;
		float t = 0;
		float startDensity = RenderSettings.fogDensity;
		float endDensity = fogDensity;
		Color startColor = RenderSettings.fogColor;
		Color endColor = fogColor;
		if (!RenderSettings.fog)
		{
			RenderSettings.fogColor = fogColor;
			RenderSettings.fogDensity = 0;
			RenderSettings.fog = fog;
		}

		while(lerpTime != 0 && t < lerpTime)
		{
			RenderSettings.fogDensity = Mathf.Lerp(startDensity, endDensity, Easing.Ease(t/lerpTime, curve));
			RenderSettings.fogColor = Color.Lerp(startColor, endColor, Easing.Ease(t/lerpTime, curve));
			t += Time.deltaTime;

			if (fogDensity != endDensity || fogColor != endColor)
			{
				//Starts again
				t = 0; 
				endColor = fogColor;
				endDensity = fogDensity;
			}

			yield return null;
		}

		RenderSettings.fog = fog;
		RenderSettings.fogDensity = fogDensity;
		RenderSettings.fogColor = fogColor;
		updatingFog = false;

	}
}
