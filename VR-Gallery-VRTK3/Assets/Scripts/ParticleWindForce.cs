using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWindForce : MonoBehaviour {

	private ParticleSystem ps;
	private ParticlesWind wind;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem>();

		if (!GetWind())
			Invoke("GetWind", 1f);

	}

	bool GetWind()
	{
		if (GameMaster.Instance && GameMaster.Instance.SceneBehaviour())
			wind = GameMaster.Instance.SceneBehaviour().GetComponent<ParticlesWind>();

		return wind != null;
	}
	
	// Update is called once per frame
	void Update () {
		SetForce();
	}

	void SetForce()
	{
		if (ps && wind)
		{
			Vector3 force = wind.GetWindForce();
			var forceModule = ps.forceOverLifetime;
			forceModule.enabled = true;
			forceModule.xMultiplier = 1f;
			forceModule.zMultiplier = 1f;
			forceModule.x = force.x;
			forceModule.z = force.x;
			forceModule.space = ParticleSystemSimulationSpace.World;
		}
	}
}
