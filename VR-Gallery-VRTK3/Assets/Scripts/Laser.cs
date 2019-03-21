using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	private float scaleTo;
	private float speed;
	private Rigidbody rb;
	private BoxCollider col;

	void Awake () {
		rb = gameObject.GetComponent<Rigidbody> ();
		col = gameObject.GetComponent<BoxCollider> ();
		gameObject.transform.localScale = new Vector3 (0.1f, 0.1f, 0f);
	}

	public void Fire(float scaleTo, float speed){
		this.scaleTo = scaleTo;
		this.speed = speed;
		StartCoroutine (ScaleUp ());
	}

	private void Kill(Collider collider){
		if (collider.tag == "shape")
		{
			//collider.gameObject.Kill();
		}
		gameObject.SetActive(false);
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider collider){
		StartCoroutine(ScaleDown(collider));
	}

	private IEnumerator ScaleUp(){
		//Debug.Log ("starting scaling");
		while (gameObject.transform.localScale.z < scaleTo) {
			gameObject.transform.localScale += Vector3.forward * Time.deltaTime * speed;
			Debug.Log (gameObject.transform.localScale);
			yield return null;
		}
		rb.velocity = transform.forward * speed;
		//Debug.Log ("speeding");
	}

	private IEnumerator ScaleDown(Collider collider){
		while (gameObject.transform.localScale.z > 0){
			float factor = Time.deltaTime * speed;
			if (gameObject.transform.localScale.z - factor >= 0)
			{
				gameObject.transform.localScale -= Vector3.forward * factor;
			} else {
				gameObject.transform.localScale = new Vector3 (0.1f, 0.1f, 0f);
				rb.velocity = Vector3.zero;
			}
			yield return null;
		}
		Kill(collider);
	}
}
