using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingCow : MonoBehaviour {

	public bool debugBlowUp;
	public bool debugRestore;

	public int vegetableAmount = 10;
	public Transform SpawnPointsParent;
	public Transform ExplosionPSParent;
	public Transform explosionOrigin;
	public GameObject meshObject;
	
	public List<GameObject> vegetableObjects = new List<GameObject>();
	[Range(0,5f), Tooltip("Max offset from a spawnpoint to spawn a vegetable")]
	public float spawnRadius = 1f;

	public float explosionForce = 5f;
	public float explosionForcePSMultiplier = 2f;

	
	private List<Transform> spawnPoints = new List<Transform>();
	private List<ParticleSystem> explosionParticleSystems = new List<ParticleSystem>();
	private List<Rigidbody> vegetables = new List<Rigidbody>();
	private Vector3[] veggieOriginalPos;



	// Use this for initialization
	void Start () {
		GetSpawnPoints();
		SpawnVegetables();
		ArrayFromVeggiePositions();
		GetExplodeParticleSystems();
	}

	void Update()
	{
		if (debugBlowUp || Input.GetKeyDown(KeyCode.J))
		{
			debugBlowUp = false;
			BlowUp();
		}
		if (debugRestore || Input.GetKeyDown(KeyCode.H))
		{
			debugRestore = false;
			Restore();
		}
	}

	void GetExplodeParticleSystems()
	{
		explosionParticleSystems = new List<ParticleSystem>();
		foreach(var ps in ExplosionPSParent.GetComponentsInChildren<ParticleSystem>())
		{
			explosionParticleSystems.Add(ps);
		}
	}

	void PlayExplosionParticles()
	{
		foreach(var ps in explosionParticleSystems)
		{
			var minmax = new ParticleSystem.MinMaxCurve();
			minmax.constantMin = explosionForce * 0.1f * explosionForcePSMultiplier;
			minmax.constantMax = explosionForce * explosionForcePSMultiplier;
			minmax.mode = ParticleSystemCurveMode.TwoConstants;
			var main = ps.main;
			main.startSpeed = minmax;
			ps.Play();
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponentInParent<VRTK.VRTK_TrackedController>())
		{
			BlowUp();
		}
	}

	void GetSpawnPoints()
	{
		spawnPoints = new List<Transform>();
		foreach (var trans in SpawnPointsParent.GetComponentsInChildren<Transform>(true))
		{
			if (trans != SpawnPointsParent)
			{
				spawnPoints.Add(trans);
			}
		}
	}
	
	void SpawnVegetables()
	{
		Vector3 spawnPos = Vector3.zero;
		int veggie = 0;

		//Loop through spawnpoints
		for (int spawnIndex = 0 ; spawnIndex < spawnPoints.Count; spawnIndex++)
		{
			//Divide amount with spawnpoints
			for (int amountIndex = 0; amountIndex < vegetableAmount/spawnPoints.Count; amountIndex++)
			{
				//Instantiate same amount of every veggie
				// veggie = amountIndex < vegetableObjects.Count ? amountIndex : amountIndex % vegetableObjects.Count;
				//Get random spawnpos from current spawnpoint
				Debug.Log(veggie);
				spawnPos = spawnPoints[spawnIndex].position + Random.insideUnitSphere*spawnRadius;

				var temp = Instantiate(vegetableObjects[veggie], spawnPos, Random.rotation);
				temp.transform.parent = spawnPoints[spawnIndex].transform;
				temp.gameObject.SetActive(false);
				vegetables.Add(temp.GetComponent<Rigidbody>());
				
				veggie++;
				veggie %= vegetableObjects.Count;
				
			}
		}
	}

	void ArrayFromVeggiePositions()
	{
		veggieOriginalPos = new Vector3[vegetables.Count];
		for (int i = 0; i < vegetables.Count; i++)
		{
			veggieOriginalPos[i] = vegetables[i].transform.position;
		}
	}

	public void BlowUp()
	{
		meshObject.SetActive(false);
		float force = explosionForce;
		PlayExplosionParticles();
		foreach (var rb in vegetables)
		{
			force = Random.Range(explosionForce*0.25f, explosionForce);
			rb.gameObject.SetActive(true);
			rb.AddExplosionForce(force, explosionOrigin.position, 20f, explosionForce*0.1f, ForceMode.Impulse);
		}

	}

	public void Restore()
	{
		meshObject.SetActive(true);
		for (int i = 0; i < vegetables.Count; i++)
		{
			vegetables[i].gameObject.SetActive(false);
			vegetables[i].velocity = Vector3.zero;
			vegetables[i].angularVelocity = Vector3.zero;
			vegetables[i].transform.position = veggieOriginalPos[i];
		}
	}
}
