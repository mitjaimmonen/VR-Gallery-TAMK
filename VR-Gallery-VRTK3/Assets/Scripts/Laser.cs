using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	private float scaleTo;
	private float speed;
	private Rigidbody rb;
	private BoxCollider col;
	private Vector3 spawnpoint;
	private bool hasCollided = false;
	private int life;
	private GameObject reflection;

	public void Awake ()
	{
		reflection = GetComponent<ReflectionObj>().reflection;
		rb = GetComponent<Rigidbody> ();
		col = GetComponent<BoxCollider> ();
		transform.localScale = new Vector3 (0.1f, 0.1f, 0f);
	}

	public void Fire(float scaleTo, float speed, int life)
	{
		this.scaleTo = scaleTo;
		this.speed = speed;
		this.life = life;
		//Debug.Log (scaleTo);
		//Debug.Log (speed);
		StartCoroutine (ScaleUp ());
	}

	private void Kill()
	{
		gameObject.SetActive(false);
		Destroy(gameObject);
		reflection.SetActive(false);
		Destroy(reflection);
	}

	//*
	private void OnTriggerEnter(Collider collider){
		if(collider.tag != "lasergun" && collider.tag != "spaceshape")
		{
			hasCollided = true;
			StartCoroutine (ScaleDown (collider));
		}
		else if (collider.GetComponent<SpaceShape>())
		{
			collider.GetComponent<SpaceShape>().Kill();
		}
	}
	//*/

	private IEnumerator ScaleUp(){
		Debug.Log ("starting scaling");
		while (transform.localScale.z < scaleTo) {
			transform.localScale += Vector3.forward * Time.deltaTime * speed;
			if (!hasCollided) {
				yield return null;
			}
		}
		rb.velocity = transform.forward * speed;
		Debug.Log ("speeding");
	}

	private IEnumerator ScaleDown(Collider collider){
		Debug.Log ("scaling down");
		//rb.velocity = Vector3.zero;
		while (transform.localScale.z > 0f){
			transform.localScale -= Vector3.forward * Time.deltaTime * speed;
			//Debug.Log (transform.localScale);
			yield return null;
		}
		rb.velocity = Vector3.zero;
		Kill();
	}

	public void FixedUpdate(){
		if (--life == 0) {
			Kill ();
		}
	}
}
