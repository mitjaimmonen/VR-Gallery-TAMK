using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterChecker : MonoBehaviour {

	[SerializeField] private GameObject gameMasterPrefab;

	void Awake()
	{
		if (!GameMaster.Instance && GameObject.FindGameObjectsWithTag("GameController").Length == 0)
		{
			Instantiate(gameMasterPrefab, Vector3.zero, Quaternion.identity);
		}
	}
}
