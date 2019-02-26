using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour {

	Collider thisCollider;

	void Awake()
	{
		thisCollider = GetComponent<Collider>();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.GetComponent<Fire>())
		{
			
			col.GetComponent<Fire>().GetPoked(thisCollider.ClosestPoint(col.transform.position));
		}
	}
}
