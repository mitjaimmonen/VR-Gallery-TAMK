﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour {

	[SerializeField] private Transform prefab;
	[SerializeField] private Mesh[] meshes;
	[SerializeField] private int shapesAmount;
	[SerializeField] private float radius;
	[SerializeField] private float minScale = 0.1f;
	[SerializeField] private float maxScale = 0.5f;
	[SerializeField] private float scaleMultiplier = 1.0f;
	[SerializeField] private float gravity = 1.0f;

	void Awake () {
		Physics.gravity = new Vector3 (0, -gravity, 0);
		for (int i = 0; i < shapesAmount; i++) {
			Transform s = Instantiate (prefab);
			Mesh mesh = meshes [Random.Range (0, meshes.Length)];
			s.localScale = Vector3.one * Random.Range (minScale, maxScale) * scaleMultiplier;
			Vector3 pos = Random.onUnitSphere * radius;
			pos.y = Mathf.Abs (pos.y);
			s.localPosition = pos;
			s.GetComponent<MeshCollider> ().sharedMesh = mesh;
			s.GetComponent<MeshFilter> ().sharedMesh = mesh;
			s.gameObject.tag = "spaceshape";
		}
	}
}