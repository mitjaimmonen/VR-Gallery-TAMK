using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterCanvas : MonoBehaviour {


    [SerializeField]private float fadeOnTime = 1f;
    [SerializeField]private float fadeOffTime = 0.5f;
    [SerializeField]private float fadeOffDelay = 0.5f;
    [SerializeField]private Color fadeOnColor = Color.white;
    [SerializeField]private Color fadeOffColor = Color.clear;
	[SerializeField]private Image fadePanel;
	[SerializeField]private Slider progressSlider;
	[SerializeField]private Text progressText;

	private float progressValue;

	public float FadeOnTime
	{
		get { return fadeOnTime; }
	}

	private Color currentColor = Color.clear;


	void Awake()
	{
		fadePanel.enabled = false;
		progressSlider.gameObject.SetActive(false);
		progressText.enabled = false;
	}

	void Update()
	{
		if (progressValue != progressSlider.value)
		{
			progressSlider.value = Mathf.Lerp(progressSlider.value, progressValue, Time.deltaTime);
			if (Mathf.Approximately(progressValue, progressSlider.value))
				progressSlider.value = progressValue;
		}
	}

	public void ClampFadeout(float maxTime)
	{
		fadeOnTime = Mathf.Clamp(fadeOnTime, 0f, maxTime);
	}

	public void SetSliderValue(bool active, float value, float delay)
	{
		if (delay != 0)
		{
			StartCoroutine(SettingSliderValue(active, value, delay));
		}
		else
		{
			progressSlider.gameObject.SetActive(active);
			progressValue = value;
		}
	}

	IEnumerator SettingSliderValue(bool active, float value, float delay)
	{
		yield return new WaitForSeconds(delay);
		progressSlider.gameObject.SetActive(active);
		progressValue = value;
	}

	public void SetProgressText(bool active, string text, float delay)
	{
		if (delay != 0)
			StartCoroutine(SettingProgressText(active,text,delay));
		else
		{
			progressText.enabled = active;
			progressText.text = text;
		}
	}

	IEnumerator SettingProgressText(bool active, string text, float delay)
	{
		yield return new WaitForSeconds(delay);
		progressText.enabled = active;
		progressText.text = text;
	}
	public void FadeIn()
	{
		StartCoroutine(Fading(true));
	}

	public void FadeOut()
	{
		StartCoroutine(Fading(false));
	}

	private IEnumerator Fading(bool fadeOff)
	{
		Color startCol = fadeOff ? fadeOnColor : fadeOffColor;
		Color endCol = fadeOff ? fadeOffColor : fadeOnColor;
		float fadetime = fadeOff ? fadeOffTime : fadeOnTime;
		fadePanel.enabled = true;
		fadePanel.color = startCol;
		
		if (fadeOff)
		{
			yield return new WaitForSeconds(fadeOffDelay);
		}

		float fadeStartTime = Time.time;
		float t = 0;

		while (fadeStartTime + fadetime >= Time.time)
		{
			fadePanel.enabled = true;
			t = (Time.time - fadeStartTime) / fadetime;
			currentColor = Color.Lerp(startCol, endCol, t);
			fadePanel.color = currentColor;
			yield return null;
		}
		
		fadePanel.color = endCol;

		if (fadeOff)
		{
			fadePanel.enabled = false;
		}
	}
}
