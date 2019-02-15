using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerChecker : MonoBehaviour {

	private const string MANAGER_SCENE = "99_Manager(Persist)";

	// Use this for initialization
	void Awake () {
		//World is going to end if this does not turn false!!!!
		bool apocalypse = true;
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			if (SceneManager.GetSceneAt(i).name == MANAGER_SCENE)
			{
				//Alls goods
				apocalypse = false;
			}
		}

		if (apocalypse)
		{
			SceneManager.LoadSceneAsync(MANAGER_SCENE, LoadSceneMode.Additive);
		}
	}

}
