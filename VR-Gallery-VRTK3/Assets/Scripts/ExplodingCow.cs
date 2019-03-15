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
	private Vector3 meshOrigScale;


	private bool isBlownUp;

	// Use this for initialization
	void Start () {
		meshOrigScale = meshObject.transform.localScale;

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
	private void OnTriggerEnter(Collider other) 
	{
		if (!isBlownUp && other.GetComponentInParent<VRTK.VRTK_TrackedController>())
		{
			BlowUp();
		}
	}
	IEnumerator PlayExplosionParticles()
	{
		var minmax = new ParticleSystem.MinMaxCurve();
		minmax.constantMin = explosionForce * 0.1f * explosionForcePSMultiplier;
		minmax.constantMax = explosionForce * explosionForcePSMultiplier;
		minmax.mode = ParticleSystemCurveMode.TwoConstants;

		foreach(var ps in explosionParticleSystems)
		{
			var main = ps.main;
			main.startSpeed = minmax;
			ps.Play();

			//Allows more time to execute all
			yield return new WaitForSeconds(0.2f / explosionParticleSystems.Count);
		}
	}
	

	public void BlowUp()
	{
		StartCoroutine(PlayExplosionParticles());
		StartCoroutine(VeggieRbBlowUp());
		StartCoroutine(MeshBlowUp());
		isBlownUp = true;
	}

	IEnumerator VeggieRbBlowUp()
	{
		float force = explosionForce;
		foreach (var rb in vegetables)
		{
			force = Random.Range(explosionForce*0.25f, explosionForce);
			rb.gameObject.SetActive(true);
			rb.AddExplosionForce(force, explosionOrigin.position, 20f, explosionForce*0.1f, ForceMode.Impulse);

			//Blow up takes 0,2 seconds.
			yield return new WaitForSeconds(0.2f / vegetables.Count);
		}
	}

	IEnumerator MeshBlowUp()
	{
		float t = 0;
		float lerpTime = 0.1f;
		while (t < lerpTime)
		{
			meshObject.transform.localScale = Vector3.Lerp(meshOrigScale, Vector3.zero, t / lerpTime);
			t += Time.deltaTime;
			yield return null;
		}
		meshObject.SetActive(false);
	}

	void RestoreMesh()
	{
		meshObject.transform.localScale = meshOrigScale;
		meshObject.SetActive(true);
	}

	public void Restore()
	{
		isBlownUp = false;
		RestoreMesh();
		for (int i = 0; i < vegetables.Count; i++)
		{
			vegetables[i].gameObject.SetActive(false);
			vegetables[i].velocity = Vector3.zero;
			vegetables[i].angularVelocity = Vector3.zero;
			vegetables[i].transform.position = veggieOriginalPos[i];
		}
	}
}
