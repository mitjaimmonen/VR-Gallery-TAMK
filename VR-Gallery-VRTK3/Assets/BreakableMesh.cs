using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BreakableMesh : MonoBehaviour {

	[Header("Debug")]
	[SerializeField] private bool debugBreak;
	[SerializeField] private bool debugSetBroken;
	[SerializeField] private bool debugRestore;
	[SerializeField] private float debugRestoreTime = 1f;

	[Header("Parameters")]


	[SerializeField] private GameObject meshes;
	[Tooltip("One child should represent the complete piece before being broken.")]
	[SerializeField] private GameObject mainPiece;
	[SerializeField] private Collider mainCollider;
	[SerializeField] private Rigidbody mainRb;
	[SerializeField] private Vector2 BreakForceRange;
	[SerializeField] private float velocityEffect;

	//Stores the initial position & rotation of children to bring object back together.
	private List<KeyValuePair<Rigidbody, KeyValuePair<Vector3, Quaternion>>> childPieces = new List<KeyValuePair<Rigidbody, KeyValuePair<Vector3, Quaternion>>>();


	private float colRadius = 1f;
	private bool broken = false;
	private bool restoring = false;

	private bool wasKinematic = false;

	void Awake()
	{
		Init();

	}

	void Update()
	{
		if (debugBreak)
		{
			debugBreak = false;
			Break(Vector3.zero);
		}
		if (debugRestore)
		{
			debugRestore = false;
			Restore(false, debugRestoreTime);
		}
		if (debugSetBroken)
		{
			debugSetBroken = false;
			SetBroken();
		}
	}

	void Init ()
	{
		mainCollider = GetComponent<Collider>();
		colRadius = (mainCollider.bounds.extents.x + mainCollider.bounds.extents.y + mainCollider.bounds.extents.z) / 3;

		mainRb = GetComponent<Rigidbody>();
		wasKinematic = mainRb.isKinematic;

		childPieces.Clear();

		foreach (var trans in meshes.GetComponentsInChildren<Transform>(true))
		{
			if (trans != mainPiece.transform && trans != meshes.transform)
			{

				Rigidbody rb = trans.gameObject.GetComponent<Rigidbody>();
				if (!rb)
					rb = trans.gameObject.AddComponent<Rigidbody>();

				Collider col = trans.gameObject.GetComponent<Collider>();
				if (!col)
				{
					MeshCollider meshCol = trans.gameObject.AddComponent<MeshCollider>();
					meshCol.convex = true;
				}

				// rb.isKinematic = true;
				KeyValuePair<Vector3, Quaternion> posRot = new KeyValuePair<Vector3,Quaternion>(trans.localPosition, trans.localRotation);
				childPieces.Add(new KeyValuePair<Rigidbody, KeyValuePair<Vector3, Quaternion>>(rb, posRot));
				trans.gameObject.SetActive(false);
			}
		}

		broken = false;
	}
	
	public void Break(Vector3 impulse)
	{
		broken = true;
		mainPiece.SetActive(false);
		wasKinematic = mainRb.isKinematic;
		mainRb.isKinematic = true;

		if (mainCollider)
			mainCollider.enabled = false;
		
		for (int i = 0; i < childPieces.Count; i++)
		{
			childPieces[i].Key.gameObject.SetActive(true);
			childPieces[i].Key.gameObject.GetComponent<Collider>().enabled = true;
			// childPieces[i].Key.isKinematic = false;
			childPieces[i].Key.velocity = Vector3.zero;

			float strength = Random.Range(BreakForceRange.x, BreakForceRange.y) * colRadius;
			childPieces[i].Key.AddExplosionForce(strength, transform.position,colRadius*2f, 1f, ForceMode.Impulse);
			childPieces[i].Key.AddForce(impulse * velocityEffect, ForceMode.Impulse);
		}

	}

	public void SetBroken()
	{
		broken = true;
		mainPiece.SetActive(false);
		wasKinematic = mainRb.isKinematic;
		mainRb.isKinematic = true;

		if (mainCollider)
			mainCollider.enabled = false;
		
		for (int i = 0; i < childPieces.Count; i++)
		{
			childPieces[i].Key.gameObject.SetActive(true);
			childPieces[i].Key.gameObject.GetComponent<Collider>().enabled = true;
			// childPieces[i].Key.isKinematic = false;
			childPieces[i].Key.velocity = Vector3.zero;
			childPieces[i].Key.angularVelocity = Vector3.zero;

			Vector3 direction = transform.TransformPoint(childPieces[i].Value.Key) - transform.position;
			childPieces[i].Key.transform.position = transform.TransformPoint(childPieces[i].Value.Key) + (direction * Random.Range(2,5f));
			childPieces[i].Key.transform.rotation = Random.rotation;

		}
	}

	/// <param name="instant">Does not use forces over time. More lightweight to execute.</param>
	public void Restore(bool instant = true, float restoreTime = 1f)
	{
		if (!broken)
		{
			Debug.Log("Nothing to restore.");
			return;
		}

		if (restoring)
		{
			Debug.Log("Already restoring.");
			return;
		}

		if (instant)
		{
			for (int i = 0; i < childPieces.Count; i++)
			{
				childPieces[i].Key.gameObject.transform.localPosition = childPieces[i].Value.Key;
				childPieces[i].Key.gameObject.transform.localRotation = childPieces[i].Value.Value;
				childPieces[i].Key.isKinematic = true;
				childPieces[i].Key.gameObject.SetActive(false);
			}
			mainPiece.SetActive(true);
			if (mainCollider)
				mainCollider.enabled = true;

			mainRb.isKinematic = wasKinematic;
			broken = false;
		}
		else
		{
			StartCoroutine(RestorePiecesOverTime(restoreTime));
		}
	}


	IEnumerator RestorePiecesOverTime(float restoreTime)
	{
		restoring = true;

		float t = 0;
		float easedTime = 0;
		List<KeyValuePair<Vector3, Quaternion>> pieceTransforms = new List<KeyValuePair<Vector3, Quaternion>>();

		if (restoreTime == 0)
		{
			//Avoids invalid division and restores instantly.
			Restore(true);
			yield break;
		}

		for (int i = 0; i < childPieces.Count; i++)
		{
			childPieces[i].Key.gameObject.SetActive(true);
			childPieces[i].Key.gameObject.GetComponent<Collider>().enabled = false;
			pieceTransforms.Add(new KeyValuePair<Vector3,Quaternion>(childPieces[i].Key.gameObject.transform.localPosition, childPieces[i].Key.transform.localRotation));
		}

		while (t <= restoreTime)
		{
			for (int i = 0; i < childPieces.Count; i++)
			{
				childPieces[i].Key.gameObject.transform.localPosition = Vector3.Lerp(pieceTransforms[i].Key, childPieces[i].Value.Key, easedTime);
				childPieces[i].Key.gameObject.transform.localRotation = Quaternion.Slerp(pieceTransforms[i].Value, childPieces[i].Value.Value, easedTime);
			}

			t += Time.deltaTime;
			easedTime = Easing.Ease(t/restoreTime, Curve.SmoothStep);
			yield return null;
		}

		//Pieces can inactivate as mainPiece activates.
		for (int i = 0; i < childPieces.Count; i++)
		{
			childPieces[i].Key.isKinematic = true;
			childPieces[i].Key.gameObject.SetActive(false);
		}

		//Set values back to initial.
		mainPiece.SetActive(true);
		mainRb.isKinematic = wasKinematic;
		if (mainCollider)
			mainCollider.enabled = true;

		broken = false;
		restoring = false;

	}
}
