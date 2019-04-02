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

	public void Start ()
	{
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
		Debug.Log ("killing");
		//rb.velocity = Vector3.zero;
		//transform.GetChild(0).gameObject.SetActive(false);
		//GetComponent<ParticleSystem> ().Play ();
		//StartCoroutine (Despawn ());
		if (GetComponent<ReflectionObj>().reflection != null) {
			reflection = GetComponent<ReflectionObj> ().reflection;
			reflection.SetActive(false);
			Destroy(reflection);
		}
		gameObject.SetActive(false);
		Destroy(gameObject);
	}

	//*
	private void OnTriggerEnter(Collider collider){
		if(collider.tag != "lasergun" && !hasCollided && collider.gameObject.layer != 8)
		{
			hasCollided = true;
			Kill();
			if (collider.GetComponent<SpaceShape> () != null) {
				collider.GetComponent<SpaceShape>().Kill();
			}
		}
	}
	//*/

	private IEnumerator ScaleUp(){
		//Debug.Log ("starting scaling");
		while (transform.localScale.z < scaleTo) {
			transform.localScale += Vector3.forward * Time.deltaTime * speed;
			if (hasCollided) {
				yield break;
			}
			yield return null;
		}
		rb.velocity = transform.forward * speed;
		//Debug.Log ("speeding");
	}

	private IEnumerator Despawn(){
		yield return new WaitForSeconds (3);
		gameObject.SetActive(false);
		Destroy(gameObject);
	}

	public void FixedUpdate(){
		if (--life == 0) {
			Kill ();
		}
	}
}
