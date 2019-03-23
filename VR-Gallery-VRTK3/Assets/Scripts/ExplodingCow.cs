using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingCow : MonoBehaviour {

	[SerializeField] private bool autoRestore = true;
	[SerializeField] private float autoRestoreDelay = 5f;

	[SerializeField] private Transform meshAliveTrans;
	[SerializeField] private Transform meshDeadTrans;
	[SerializeField] private Transform explosionOrigin;
	[SerializeField] private int vegetableAmount = 10;
	[SerializeField] private Transform SpawnPointsParent;
	[SerializeField] private Transform ExplosionPSParent;
	[SerializeField] private GameObject meshObject;
	[SerializeField] private List<GameObject> vegetableObjects = new List<GameObject>();

	[Range(0,5f), Tooltip("Max offset from a spawnpoint to spawn a vegetable")]
	[SerializeField] private float spawnRadius = 1f;

	[SerializeField] private float cowCollapseDuration = 1f;
	[SerializeField] private float explosionForce = 5f;
	[SerializeField] private float explosionDuration = 0.35f;
	[SerializeField] private float explosionForcePSMultiplier = 2f;
	[SerializeField] private float restoreDuration = 1f;
	[SerializeField] private int particleCount = 100;


	
	private List<Transform> spawnPoints = new List<Transform>();
	private List<ParticleSystem> explosionParticleSystems = new List<ParticleSystem>();
	private List<Rigidbody> vegetables = new List<Rigidbody>();
	private Vector3[] veggieOriginalPos;
	private int xPosRelativeToCow = 0;

	private bool isDead;
	private bool isRestoring;
	private bool isTouching;
	private float lastTimeTouched;

	// Use this for initialization
	void Start () {
		GetSpawnPoints();
		SpawnVegetables();
		ArrayFromVeggiePositions();
		GetExplodeParticleSystems();
		SetExplodeParticleSystemParams();
	}

	void Update()
	{
		if ( isDead && autoRestore && !isRestoring && lastTimeTouched + autoRestoreDelay < Time.time)
		{
			StartCoroutine(Restore());
		}
	}

	void GetExplodeParticleSystems()
	{
		explosionParticleSystems = new List<ParticleSystem>();
		foreach(var ps in ExplosionPSParent.GetComponentsInChildren<ParticleSystem>())
		{
			explosionParticleSystems.Add(ps);
			ps.Emit(1);
			ps.Clear();
		}
	}
	void SetExplodeParticleSystemParams()
	{
		var minmax = new ParticleSystem.MinMaxCurve();
		minmax.constantMin = explosionForce * 0.1f * explosionForcePSMultiplier;
		minmax.constantMax = explosionForce * explosionForcePSMultiplier;
		minmax.mode = ParticleSystemCurveMode.TwoConstants;

		ParticleSystem.Burst burst = new ParticleSystem.Burst();
		burst.cycleCount = Mathf.Clamp((int)(explosionDuration*10f), 1, 50);
		burst.repeatInterval = explosionDuration / burst.cycleCount;
		burst.count = particleCount / burst.cycleCount;

		foreach(var ps in explosionParticleSystems)
		{
			var main = ps.main;
			main.startSpeed = minmax;
			main.maxParticles = particleCount*2;
			
			var emission = ps.emission;
			emission.burstCount = 1;
			emission.SetBurst(0,burst);
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
				vegetables.Add(temp.GetComponent<Rigidbody>());
				temp.gameObject.SetActive(false);
				
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
		if (other.GetComponentInParent<VRTK.VRTK_TrackedController>())
		{
			isTouching = true;
			if (!isDead)
			{
				//Which side of cow did touch happen
				//THis is not right tho
				xPosRelativeToCow = other.transform.position.x - meshAliveTrans.position.x >= 0 ? 1 : -1;
				Kill();
			}
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.GetComponentInParent<VRTK.VRTK_TrackedController>())
		{
			isTouching = true;
			lastTimeTouched = Time.time;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<VRTK.VRTK_TrackedController>())
		{
			lastTimeTouched = Time.time;
			isTouching = false;
		}
	}


	IEnumerator PlayExplosionParticles()
	{
		foreach(var ps in explosionParticleSystems)
		{
			ps.Play();

			//One system starts per frame
			yield return null;
		}
	}
	

	public void Kill()
	{
		isDead = true;
		StartCoroutine(KillCowWithExplosion());
	}

	IEnumerator KillCowWithExplosion()
	{
		float t = 0;
		float lerpT = 0;
		Vector3 deadPos = meshDeadTrans.localPosition;
		deadPos.x *= xPosRelativeToCow;
		Vector3 deadRot = meshDeadTrans.localEulerAngles;
		deadRot.z *= xPosRelativeToCow;

		while (t <= cowCollapseDuration)
		{	
			lerpT = Easing.Ease((t/cowCollapseDuration), Curve.exponential);
			meshObject.transform.localPosition = Vector3.Lerp(meshAliveTrans.localPosition, deadPos, lerpT);
			meshObject.transform.localRotation = Quaternion.Slerp(meshAliveTrans.localRotation, Quaternion.Euler(deadRot), lerpT);
			t += Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(0.25f);

		StartCoroutine(PlayExplosionParticles());
		StartCoroutine(VeggieRbBlowUp());
		StartCoroutine(MeshBlowUp());
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
			yield return new WaitForSeconds(explosionDuration / vegetables.Count);
		}
	}

	IEnumerator MeshBlowUp()
	{
		float t = 0;
		float lerpTime = explosionDuration;
		while (t < lerpTime)
		{
			meshObject.transform.localScale = Vector3.Lerp(meshAliveTrans.localScale, Vector3.zero, t / lerpTime);
			t += Time.deltaTime;
			yield return null;
		}
		meshObject.SetActive(false);
	}

	void RestoreMesh()
	{
		meshObject.transform.localScale = meshAliveTrans.localScale;
		meshObject.transform.position = meshAliveTrans.position;
		meshObject.transform.rotation = meshAliveTrans.rotation;
		meshObject.SetActive(true);
	}

	IEnumerator Restore()
	{
		isRestoring = true;
		float t = 0;

		//Make veggies disappear
		while (t < restoreDuration/2f)
		{	
			for (int i = 0; i < vegetables.Count; i++)
			{
				vegetables[i].transform.localScale -= Vector3.one * Time.deltaTime;
			}
			t += Time.deltaTime;
			yield return null;
		}

		for (int i = 0; i < vegetables.Count; i++)
		{
			vegetables[i].gameObject.SetActive(false);
			vegetables[i].velocity = Vector3.zero;
			vegetables[i].angularVelocity = Vector3.zero;
			vegetables[i].transform.position = veggieOriginalPos[i];
			vegetables[i].transform.localScale = Vector3.one;
		}

		//Make cow appear
		t = 0;
		float lerpT = 0;

		meshObject.transform.position = meshAliveTrans.position + Vector3.down*2f;
		meshObject.transform.localScale = Vector3.zero;
		meshObject.SetActive(true);

		while (t < restoreDuration/2f)
		{
			lerpT = Easing.Ease(t / (restoreDuration/2f), Curve.SmoothStep);
			meshObject.transform.position = Vector3.Lerp(meshAliveTrans.position + Vector3.down*2f, meshAliveTrans.position, lerpT);
			meshObject.transform.localScale = Vector3.Lerp(Vector3.zero, meshAliveTrans.localScale, lerpT);
			t += Time.deltaTime;
			yield return null;
		}

		RestoreMesh();
		isRestoring = false;
		isDead = false;
	}
}
