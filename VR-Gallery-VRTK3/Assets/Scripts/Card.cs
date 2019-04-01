using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Card : VRTK_InteractableObject {

	[SerializeField] private bool debug;
	private Transform pst;
	private SpriteRenderer sr;
	private Rigidbody rb;
	private Texture2D texture;
	private Vector2 origin;

	//*
	void Start(){
		texture = new Texture2D(256, 256, TextureFormat.RGB24, true);
		texture.name = "Noise Tex";	
		float step = 1f/256f;
		origin = new Vector2(Random.Range(-100,100), Random.Range(-100,100));
		float scale = 10f;
		for (int y = 0; y < 256; y++)
		{
			for (int x = 0; x < 256; x++)
			{
				float n = Mathf.PerlinNoise(origin.x + (float)x * step * scale, origin.y + (float)y * step * scale);
				texture.SetPixel(x, y, new Color(n,n,n));
			}
		}
		texture.Apply();

		if (debug)
		{
			pst = transform.GetChild(0);
			sr = GetComponent<SpriteRenderer>();
			rb = GetComponent<Rigidbody>();
			pst.localPosition = Vector3.up * sr.sprite.bounds.extents.y;
			Debug.Log(GetComponent<SpriteRenderer>().sprite.bounds.extents);
			StartCoroutine (Kill(5f));
		}
	}
	//*/
	public override void StartUsing(VRTK_InteractUse currentUsingObject)
	{
		base.StartUsing(currentUsingObject);
		pst = transform.GetChild(0);
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody>();
		pst.localPosition = Vector3.up * sr.sprite.bounds.extents.y;
		Debug.Log(GetComponent<SpriteRenderer>().sprite.bounds.extents);
		StartCoroutine (Kill(5f));
	}

	IEnumerator Kill(float duration){
		float destPosition = -pst.transform.localPosition.y;
		float particleSpeed = 2 * pst.transform.localPosition.y / duration;
		float clipSpeed = 1.1f/duration;
		float clip = 1f;
		pst.GetComponent<ParticleSystem>().Play();
		pst.GetComponentInChildren<ParticleSystem>().Play();
		sr.material.SetTexture("_Noise", texture);
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
