using UnityEngine;
using System.Collections;
 
public class FPSCounter : MonoBehaviour
{
	float deltaTime = 0.0f;
	float msec;
	float fps;
	float t;
	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

		msec = deltaTime * 1000.0f;
		fps = 1.0f / deltaTime;
		
		t+=Time.deltaTime;
		if (fps < 60f && t > 1f)
		{
			Debug.LogWarning("FPS is low");
			t = 0;
		}
	}
}