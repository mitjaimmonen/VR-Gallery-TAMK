using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Card : VRTK_InteractableObject {

	private Transform pst;
	private SpriteRenderer sr;
	private Rigidbody rb;

	//*
	void Start(){
		pst = transform.GetChild(0);
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody>();
		pst.localPosition = Vector3.up * sr.sprite.bounds.extents.y;
		Debug.Log(GetComponent<SpriteRenderer>().sprite.bounds.extents);
		StartCoroutine (Kill(5f));
	}
	//*/
	public override void StartUsing(VRTK_InteractUse currentUsingObject)
	{
		base.StartUsing(currentUsingObject);
		pst = transform.GetChild(0);
		sr = GetComponent<SpriteRenderer>();
		pst.localPosition = Vector3.up * sr.sprite.bounds.extents.y;
		Debug.Log(GetComponent<SpriteRenderer>().sprite.bounds.extents);
		StartCoroutine (Kill(5f));
	}

	IEnumerator Kill(float duration){
		float destPosition = -pst.transform.localPosition.y;
		float particleSpeed = 2 * pst.transform.localPosition.y / duration;
		float clipSpeed = 1/duration;
		float clip = 1f;
		pst.GetComponent<ParticleSystem>().Play();
		pst.GetComponentInChildren<ParticleSystem>().Play();
		while (pst.transform.localPosition.y > destPosition){
			clip -= Time.deltaTime * clipSpeed;
			sr.material.SetFloat("_ClipRange", clip);
			pst.transform.localPosition += Vector3.down * Time.deltaTime * particleSpeed;
			//rb.AddTorque(transform.up * 200f * Time.deltaTime);
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}
}
