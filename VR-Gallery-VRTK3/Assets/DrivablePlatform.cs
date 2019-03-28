using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DrivablePlatform : VRTK_InteractableObject {

	[SerializeField] private float speed = 1f;

	void Update () {
		if (IsUsing()) {
			Debug.Log ("using");
			Vector3 pos = Quaternion.AngleAxis (GetUsingObject ().transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward * speed * Time.deltaTime;
			transform.position += pos;
			GameObject.Find("[CameraRig]").transform.position += pos;
		}
	}
}
