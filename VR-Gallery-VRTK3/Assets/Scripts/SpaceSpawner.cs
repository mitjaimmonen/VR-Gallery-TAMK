using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSpawner : Spawner {

	[SerializeField] Mesh[] meshes;
	[SerializeField] Material[] materials;
	[SerializeField] float force;
	[SerializeField] string tag;

	public override void ExtendPosition(Transform t, int i){
		Vector3 pos = t.position;
		pos.y = Mathf.Abs(pos.y);
		if (pos.y < 1)
		{
			pos.y += Random.Range(minRadius, maxRadius);
		}
		t.position = pos;
	}

	public override void ExtendComponents(GameObject o, int i){
		Mesh mesh = meshes [Random.Range (0, meshes.Length)];
		o.GetComponent<MeshCollider> ().sharedMesh = mesh;
		o.GetComponent<MeshFilter> ().sharedMesh = mesh;
		o.GetComponent<MeshRenderer> ().material = materials[Random.Range(0,materials.Length)];
		o.tag = tag;
	}

	public override void ExtendActive(GameObject o, int i){
		o.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * force);
	}
}
