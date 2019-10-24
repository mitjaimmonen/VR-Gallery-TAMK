using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class LaserGun : VRTK_InteractableObject {

	[Header("Laser Options", order = 4)]
	[SerializeField] private Laser prefab;
	[SerializeField] private float speed = 20f;
	[SerializeField] private float scale = 5f;
	[SerializeField] private int life = 60*3;
	[SerializeField] private Transform spawnPoint;
	[SerializeField] private AudioClip[] throwSounds;
	private Vector3 spawnPointV;
	private VRTK_BasicTeleport teleport;
	//private VRTK_ControllerEvents controllerEvents;
	private GameObject grabbingObject;
	private GameObject reflection;

	private float minTriggerRotation = -10f;
	private float maxTriggerRotation = 45f;
	private Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource> ();

	protected void Start()
	{
		if (!spawnPoint) {
			spawnPoint = transform;
		}
		teleport = GameMaster.Instance.SceneBehaviour ().GetComponent<VRTK_BasicTeleport> ();
		AudioSource[] ass = gameObject.GetComponentsInChildren<AudioSource> ();
		for (int i = 0; i < ass.Length; i++) {
			switch (ass[i].name) {
			case "Throw":
				sources.Add ("throw", ass [i]);
				sources ["throw"].clip = throwSounds [Random.Range (0, throwSounds.Length)];
				break;
			case "Shoot":
				sources.Add ("shoot", ass [i]);
				break;
			case "Grab":
				sources.Add ("grab", ass [i]);
				break;
			default:
				break;
			}
		}
	}

	public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
	{
		sources["grab"].Play();
		base.Grabbed(currentGrabbingObject);
		//controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
		grabbingObject = GetGrabbingObject ();
		grabbingObject.GetComponent<VRTK_Pointer> ().enabled = false;
		teleport.enabled = false;
	}

	public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
	{
		sources["throw"].Play();
		base.Ungrabbed(previousGrabbingObject);
		sources ["throw"].clip = throwSounds [Random.Range (0, throwSounds.Length)];
		teleport.enabled = true;
		grabbingObject.GetComponent<VRTK_Pointer> ().enabled = true;
		grabbingObject = null;
		Debug.Log ("ungrabbed");
	}

	public override void StartUsing(VRTK_InteractUse currentUsingObject)
	{
		sources["shoot"].Play();
		base.StartUsing(currentUsingObject);
		Fire ();
	}

	private void Fire()
	{
		var l = Instantiate(prefab, spawnPoint.position, gameObject.transform.rotation);
		l.Fire (scale, speed, life);
	}
}
