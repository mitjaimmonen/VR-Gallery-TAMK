using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesWind : MonoBehaviour {

	[SerializeField] private Vector2 windLowMinMax;
	[SerializeField] private Vector2 windHighMinMax;
	[SerializeField, Range(0,360f)] private float windAngle;
	[SerializeField, Range(1f,10f)] private float windInterval = 1f;

	private float oldWind;
	private float targetWind;
	private Vector3 currentWindForce;
	private float windTime;
	private bool currentWindIsLow;

	public Vector3 GetWindForce()
	{
		return currentWindForce;
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateTargetWind();
		UpdateCurrentWind();

	}

	void UpdateTargetWind()
	{
		if (windInterval + windTime < Time.time)
		{
			if (currentWindIsLow)
			{
				oldWind = targetWind;
				targetWind = Random.Range(windHighMinMax.x,windHighMinMax.y);
			}
			else
			{
				oldWind = targetWind;
				targetWind = Random.Range(windLowMinMax.x, windLowMinMax.y);
			}
			windTime = Time.time;
			currentWindIsLow = !currentWindIsLow;
		}
	}

	void UpdateCurrentWind()
	{
		float wind = Mathf.Lerp(oldWind, targetWind, Easing.Ease((Time.time-windTime) / windInterval, Curve.SmoothStep));
 		float a = windAngle * Mathf.PI / 180f;
 		currentWindForce.x = Mathf.Sin(a) * wind;
 		currentWindForce.z = Mathf.Cos(a) * wind;
		currentWindForce.y = 0;
	}


}
