using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

Handy little script to apply easing to time.

 */
public enum Curve
{
	linear,
	exponential,
	logarithmic,
	smoothIn,
	smoothOut,
	SmoothStep,
	one,
	zero
}

public class Easing {
	public static float Ease(float currentTime, Curve curve)
	{
		//Useful little function to calculate easing automatically.

		switch (curve)
		{
			case Curve.one :
				return 1f;
			case Curve.zero :
				return 0;
			case Curve.linear :
				return currentTime;
			case Curve.exponential :
				return currentTime*currentTime;
			case Curve.logarithmic :
				return currentTime + (currentTime - (currentTime*currentTime));
			case Curve.smoothIn :
				return 1f - Mathf.Cos(currentTime * Mathf.PI * 0.5f);
			case Curve.smoothOut :
				return currentTime = Mathf.Sin(currentTime * Mathf.PI * 0.5f);
			case Curve.SmoothStep :
				return currentTime*currentTime * (3f - 2f*currentTime);
			default:
				return 1f;
		}
}
}
