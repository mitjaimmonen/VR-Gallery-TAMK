using UnityEngine;
using System.Collections;
 
public class FPSCounter : MonoBehaviour
{
	float deltaTime = 0.0f;
	float msec;
	float fps;
	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

		msec = deltaTime * 1000.0f;
		fps = 1.0f / deltaTime;
		if (fps < 60f)
			Debug.LogWarning("FPS is low: " + fps);
	}
}