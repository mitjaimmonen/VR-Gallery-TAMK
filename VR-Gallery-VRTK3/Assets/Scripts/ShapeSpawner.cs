using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour {

	[SerializeField] private Transform prefab;
	[SerializeField] private bool useMeshes;
	[SerializeField] private Mesh[] meshes;
	[SerializeField] private int shapesAmount;
	[SerializeField] private float radius;
	[SerializeField] private bool spawnOnCircle;
	[SerializeField] private float minScale = 0.1f;
	[SerializeField] private float maxScale = 0.5f;
	[SerializeField] private float scaleMultiplier = 1.0f;
	[SerializeField] private string tag;

	void Start () {
		for (int i = 0; i < shapesAmount; i++) {
			Transform s = Instantiate (prefab);

			s.localScale = Vector3.one * Random.Range (minScale, maxScale) * scaleMultiplier;

			Vector3 pos;
			if (spawnOnCircle) {
				pos = Random.insideUnitCircle * radius;
			} else {
				pos = Random.insideUnitSphere * radius;
			}
            float placeholder = pos.y;
			pos.y = Mathf.Abs (pos.z);
            pos.z = placeholder;
			s.position = pos;

			if (useMeshes) {
				Mesh mesh = meshes [Random.Range (0, meshes.Length)];
				s.GetComponent<MeshCollider> ().sharedMesh = mesh;
				s.GetComponent<MeshFilter> ().sharedMesh = mesh;
			}
			if (tag != null && tag != "") {
				s.gameObject.tag = tag;
			}
		}
	}
}
