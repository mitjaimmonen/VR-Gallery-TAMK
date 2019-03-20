using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour {

	[SerializeField] private Transform[] prefabs;
	[SerializeField] private int shapesAmount;
	[SerializeField] private float radius;
	[SerializeField] private float minScale = 0.1f;
	[SerializeField] private float maxScale = 0.5f;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < shapesAmount; i++) {
			Transform s = Instantiate (prefabs [Random.Range (0, prefabs.Length)]);
			s.localScale = Vector3.one * Random.Range (minScale, maxScale);
			Vector3 pos = Random.onUnitSphere * radius;
			pos.y = Mathf.Abs (pos.y);
			s.localPosition = pos;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
