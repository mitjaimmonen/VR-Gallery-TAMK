using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    public bool keepMainSceneLoaded;
    public string mainScene;
    public float sceneSwitchDelay = 2f;
    
    private string currentScene;
    private string nextScene; 

    private float sceneSwitchInitTime;


    private void Awake() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.activeSceneChanged += OnSceneChanged;
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (loadMode == LoadSceneMode.Single)
        {
        }
        else if (loadMode == LoadSceneMode.Additive)
        {
        }
    } 
    private void OnSceneUnloaded(Scene scene)
    {

    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        currentScene = newScene.name;
        if (oldScene.isLoaded && !(keepMainSceneLoaded && oldScene.name == mainScene))
            SceneManager.UnloadSceneAsync(oldScene);

        BroadcastMessage("NewSceneLoaded", newScene.name, SendMessageOptions.DontRequireReceiver);
    }

    private IEnumerator LoadingLoop()
    {
        //Waits until all loaded up and blasts into da new scene
        sceneSwitchInitTime = Time.time;
        Debug.Log(sceneSwitchInitTime + sceneSwitchDelay > Time.time);
        while(!SceneManager.GetSceneByName(nextScene).isLoaded || sceneSwitchInitTime + sceneSwitchDelay > Time.time)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log("Next scene not loaded");
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
        currentScene = nextScene;

        Debug.Log("Coroutine finished loading scene");

        yield return null;
    }

    public void LoadSceneAsync(string sceneName)
    {
        //Loads scene in the background while player is holding a scene in their hand or whatever
        nextScene = sceneName;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
    public void LoadSceneAsync(Scene scene)
    {
        //Loads scene in the background while player is holding a scene in their hand or whatever
        nextScene = scene.name;
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
    }

    public void SwitchScene(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogWarning("Not a valid scene name!");
            return;
        }
        
        //Gets called when player breaks a scene they held in hand.
        //If scene has yet not loaded, it will start loading and switch immediately
        nextScene = sceneName;
        if (!SceneManager.GetSceneByName(nextScene).isLoaded)
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        StartCoroutine(LoadingLoop());

    }

    public void SwitchToLoadedScene()
    {
        if (Application.CanStreamedLevelBeLoaded(nextScene) && SceneManager.GetSceneByName(nextScene).isLoaded)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
        }
        //Go to "nextSceneToLoad" -scene
    }

    public bool IsSceneLoaded(Scene scene)
    {
        //Return true if loadsceneasync has finished.
        return scene.isLoaded;
    }

    public void UnloadAllScenes()
    {
        //Reset nextSceneToLoad
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (!(SceneManager.GetSceneAt(i).name == mainScene && keepMainSceneLoaded) && SceneManager.GetSceneAt(i).name != currentScene)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
            }
        }
    }
}
