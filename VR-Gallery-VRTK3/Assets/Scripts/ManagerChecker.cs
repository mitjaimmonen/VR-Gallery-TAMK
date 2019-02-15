using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerChecker : MonoBehaviour {

	[SerializeField] private string managerScene = "99_Manager";

	// Use this for initialization
	void Awake () {
		//World is going to end if this does not turn false!!!!
		bool apocalypse = true;
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			if (SceneManager.GetSceneAt(i).name == managerScene)
			{
				//Alls goods
				apocalypse = false;
			}
		}

		if (apocalypse)
		{
			if (Application.CanStreamedLevelBeLoaded(managerScene))
				SceneManager.LoadSceneAsync(managerScene, LoadSceneMode.Additive);
			else // Assumes manager to be index 0
				SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);

		}
	}

}
