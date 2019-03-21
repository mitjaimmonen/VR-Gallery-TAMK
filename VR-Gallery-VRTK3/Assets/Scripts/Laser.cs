using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	private float scaleTo;
	private float speed;
	private Rigidbody rb;

	void Awake () {
		rb = gameObject.GetComponent<Rigidbody> ();
		gameObject.transform.localScale = new Vector3 (0.1f, 0.1f, 0f);
	}

	public void Fire(float scaleTo, float speed){
		this.scaleTo = scaleTo;
		this.speed = speed;
		StartCoroutine (ScaleUp ());
	}

	IEnumerator ScaleUp(){
		Debug.Log ("starting scaling");
		while (gameObject.transform.localScale.z < scaleTo) {
			gameObject.transform.localScale += Vector3.forward * Time.deltaTime * speed;
			Debug.Log (gameObject.transform.localScale);
			yield return null;
		}
		rb.velocity = transform.forward * speed;
		Debug.Log ("speeding");
	}
}
