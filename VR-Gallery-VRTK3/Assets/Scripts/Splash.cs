using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour {

	public bool debugSceneSwitch;
	public string scene;
	// Use this for initialization
	void Start () {
		if (debugSceneSwitch)
			Invoke("NextScene", 2f);
	}
	
	void NextScene()
	{
		Debug.Log("Splash calling next scene!");
		if (GameMaster.Instance)
			GameMaster.Instance.SceneMaster.SwitchScene(scene);
	}
}
