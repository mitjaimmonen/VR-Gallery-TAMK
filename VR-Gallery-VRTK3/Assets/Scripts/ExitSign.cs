using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ExitSign : VRTK_InteractableObject {

	public float activationDelay = 1f;
	public bool breakGlobeOnActivation = true;
    public SceneReference sceneToLoad;

	public GameObject exitSignVisuals;
	public ParticleSystem exitSignExplosionPS;

	private float activationStartTime = 0;
	private bool activationInProgress = false;
	private bool isActivated = false;
	private SkyGlobe globe;

	private AudioSource a;


    private void OnGUI()
    {
        DisplayLevel(sceneToLoad);
    }

    public void DisplayLevel(SceneReference scene)
    {
        GUILayout.Label(new GUIContent("Scene name Path: " + scene));
        if (GUILayout.Button("Load " + scene))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }

	private void Start()
	{
		a = GetComponent<AudioSource> ();
		if (!globe)
			globe = GameObject.FindObjectOfType<SkyGlobe>();
	}

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		base.StartUsing(usingObject);

		activationStartTime = Time.time;
		activationInProgress = true;
		Debug.Log("StartUsing called");
	}

	public override void StopUsing(VRTK_InteractUse usingObject,bool resetUsingObjectState = true)
	{
		base.StopUsing(usingObject, resetUsingObjectState);
		Debug.Log("StopUsing called");

		activationInProgress = false;
	}

	protected override void Update()
	{
		base.Update();
		
		if (activationInProgress && !isActivated && activationStartTime < Time.time - activationDelay)
		{
			isActivated = true;
			a.Play();
			StartCoroutine(ExitSceneWithStyle());
		}
	}

	IEnumerator ExitSceneWithStyle()
	{
		//Blow up the place
		//Then load main scene

		if (globe)
			globe.Break(Vector3.zero);
		
		if (exitSignExplosionPS && exitSignVisuals)
		{
			exitSignVisuals.SetActive(false);
			exitSignExplosionPS.Play();
		}

		yield return new WaitForSeconds(2f);
		
		var sm = GameMaster.Instance.SceneMaster;
		if (sceneToLoad != null && sm != null)
			sm.SwitchScene(sm.GetNameFromPath(sceneToLoad.ScenePath));

		yield return null;
	}
}
