using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyGlobe : BreakableMesh {

	public Material skyGlobeMaterial;
	public bool fadeOffOnRestored = true;
	public float defaultFadeTime = 1f;

	public float restoreTime;
	public bool startBroken = false;
	public bool restoreAtSceneStart = true;


	private Color newColor;
	private Color oldColor;


	private bool changingColor;

	void Awake()
	{
		if (startBroken)
			SetBroken();
		if (restoreAtSceneStart)
			Restore(false, restoreTime, true);
	}
	protected override void Restored()
	{
		base.Restored();
		if (fadeOffOnRestored)
		{
			Color tempCol = skyGlobeMaterial.color;
			tempCol.a = 0;
			SetMaterialColor(tempCol, defaultFadeTime);
		}
	}

	public void SetMaterialColor(Color col, float time = 0)
	{
		newColor = col;
		oldColor = skyGlobeMaterial.color;

		if (!changingColor)
		{
			if (time != 0)
				StartCoroutine(SetMaterialColorOverTime(time));
			else
				skyGlobeMaterial.color = col;
		}

	}

	private IEnumerator SetMaterialColorOverTime(float time)
	{
		changingColor = true;
		float t = 0;
		Color tempNewColor = newColor;

		if (time == 0)
		{
			//Double-check to avoid 0-division error.
			skyGlobeMaterial.color = newColor;
			yield break;
		}

		while (t / time <= 1f)
		{
			skyGlobeMaterial.color = Color.Lerp(oldColor, tempNewColor, t / time);
			if (tempNewColor != newColor)
			{
				t = 0;
				tempNewColor = newColor;
				oldColor = skyGlobeMaterial.color;
			}
			t += Time.deltaTime;
			yield return null;
		}

		skyGlobeMaterial.color = newColor;
		changingColor = false;
		Faded();
	}

	private void Faded()
	{
		
	}
}
