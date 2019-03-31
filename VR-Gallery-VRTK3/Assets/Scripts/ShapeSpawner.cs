﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour {

	[SerializeField] private Transform prefab;
	[SerializeField] private bool useMeshes;
	[SerializeField] private Mesh[] meshes;
	[SerializeField] private Sprite[] sprites;
	[SerializeField] private bool customAmount = false;
	[SerializeField] private int shapesAmount = 0;
	[SerializeField] private float radius = 5f;
	[SerializeField] private bool spawnOnCircle = false;
	[SerializeField] private float minScale = 0.3f;
	[SerializeField] private float maxScale = 2f;
	[SerializeField] private float scaleMultiplier = 1.0f;
	[SerializeField] private string tag;

	void Start () {
		int spawnAmount = shapesAmount;
		if (!customAmount) {
			shapesAmount = Mathf.Max (meshes.Length, sprites.Length);
		}
		for (int i = 0; i < shapesAmount; i++) {
			Transform s = Instantiate (prefab);

			s.localScale = Vector3.one * Random.Range (minScale, maxScale) * scaleMultiplier;

			Vector3 pos = transform.position;
			if (spawnOnCircle) {
				Vector3 circle = Random.insideUnitCircle * radius;
				float y2z = circle.y;
				circle.y = Mathf.Abs (circle.z);
				circle.z = y2z;
				pos += circle;
			} else {
				pos += Random.insideUnitSphere * radius;
				pos.y = Mathf.Abs(pos.y);
			}
			s.position = pos;

			if (useMeshes) {
				Mesh mesh = meshes [Random.Range (0, meshes.Length)];
				s.GetComponent<MeshCollider> ().sharedMesh = mesh;
				s.GetComponent<MeshFilter> ().sharedMesh = mesh;
				s.rotation = Random.rotation;
				s.GetComponent<SpaceShape>().SpawnReflection();

			} else {
				Sprite sprite = sprites [i];
				s.GetComponent<SpriteRenderer> ().sprite = sprite;
			}
			if (tag != null && tag != "") {
				s.gameObject.tag = tag;
			}
		}
	}
}
