using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[ExecuteInEditMode]
public class PedestalPlacer : MonoBehaviour {

	GameObject[] pedestalArray;
	GameObject[] sphereArray;
	Vector3[] positionsArray;
	float floorHeight;

	[SerializeField] float radius;
	[SerializeField] float angleOffset;
	[SerializeField] Transform floor;
	[SerializeField] bool usePlayArea = false;

	void Awake () {
		GetGoData();
	}

	void GetGoData() {
		pedestalArray = GameObject.FindGameObjectsWithTag("pedestal");
		sphereArray = GameObject.FindGameObjectsWithTag("SceneContainer");
		positionsArray = new Vector3[pedestalArray.Length];
		if	(floor) {
			floorHeight = floor.position.y;
		} else {
			floorHeight = 0f;
		}
	}

	void CalculatePositions() {
		GetGoData();
		float angle = 2*Mathf.PI/positionsArray.Length;
		Vector3 centre = transform.position;
		centre.y = floorHeight;
		if (usePlayArea)
		{
			//aaaaaaa
		}
		for (int i = 0; i < positionsArray.Length; i++)
		{
			float a = angle * i + angleOffset;
			Vector3 direction = new Vector3(Mathf.Sin(a),0,Mathf.Cos(a));
			positionsArray[i] = centre + direction * radius;
			//Debug.Log(angle*i);
		}
	}

	[ContextMenu("Commit")]
	void Commit() {
		CalculatePositions();
		for (int i = 0; i < pedestalArray.Length; i++)
		{
			pedestalArray[i].transform.position = positionsArray[i];
			
		}
		for (int i = 0; i < sphereArray.Length; i++)
		{
			sphereArray[i].transform.position = new Vector3(positionsArray[i].x, positionsArray[i].y + 1.22f, positionsArray[i].z);
		}
	}

	void OnDrawGizmosSelected()
    {
		CalculatePositions();
		Gizmos.color = Color.black;
		Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.color = Color.yellow;
		for (int i = 0; i < positionsArray.Length; i++)
		{
			Vector3 v = positionsArray[i];
			v.y = floorHeight + 0.5f;
			Gizmos.DrawWireCube(v, new Vector3(0.5f, 1, 0.5f));
		}

    }
}
