using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasContentArea : MonoBehaviour {


	public float zOffset = 5000f;
	public float scaleMultiplier = 2.5f;
	// Use this for initialization
	void Start () {
		RectTransform trans = GetComponent<RectTransform>();
		trans.anchoredPosition3D = new Vector3(0,0, 4000);
		trans.localScale = Vector3.one * scaleMultiplier;
	}
}
