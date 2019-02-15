using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyGlobe : BreakableMesh {

	public Material skyGlobeMaterial;
	public Color skyGlobeColor;
	public bool fadeOffOnRestored = true;
	public float defaultFadeTime = 1f;

	public bool startBroken = false;
	public bool restoreAtSceneStart = true;


	private Color newColor;
	private Color oldColor;


	private bool changingColor;

	void Start()
	{
		if (startBroken)
			SetBroken();
		if (restoreAtSceneStart)
			Restore(restoreTime, keepMainPieceInactive);

		skyGlobeMaterial.color = skyGlobeColor;
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

	public void ResetMaterialColor(float time = 0)
	{
		newColor = skyGlobeColor;
		oldColor = skyGlobeMaterial.color;

		if (!changingColor)
		{
			if (time != 0)
				StartCoroutine(SetMaterialColorOverTime(time));
			else
				skyGlobeMaterial.color = skyGlobeColor;
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
