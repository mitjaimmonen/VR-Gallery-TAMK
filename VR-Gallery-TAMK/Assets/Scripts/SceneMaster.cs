using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    public float sceneSwitchDelay = 2f;
    
    private string currentScene;
    private string nextScene;

    private float sceneSwitchInitTime;

    private List<SceneContainer> sceneContainers = new List<SceneContainer>();


    private void Awake() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.activeSceneChanged += OnSceneChanged;
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {


    } 
    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Scene unloaded: " + scene.name);
    }
    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        Debug.Log("Scene changed");
        GameMaster.Instance.NewScene();
        currentScene = newScene.name;

        GetSceneContainers();
        ResetSceneContainers();

        ResetLoadedScenes();

        if (oldScene.isLoaded)
            SceneManager.UnloadSceneAsync(oldScene);
    }

    private void GetSceneContainers()
    {
        sceneContainers.Clear();
        GameObject[] go = GameObject.FindGameObjectsWithTag("SceneContainer");
        for (int i = 0; i < go.Length; i++)
        {
            sceneContainers.Add(go[i].GetComponent<SceneContainer>());
        }
    }
    private void ResetSceneContainers()
    {
        foreach (SceneContainer item in sceneContainers)
        {
            item.Reset();
        }
    }

    private IEnumerator LoadingLoop()
    {
        //Waits until all loaded up and blasts into da new scene
        sceneSwitchInitTime = Time.time;
            Debug.LogWarning("NextScene: " + nextScene);
        while(!SceneManager.GetSceneByName(nextScene).isLoaded || sceneSwitchInitTime + sceneSwitchDelay > Time.time)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log("Next scene " + nextScene + " not loaded");
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
        currentScene = nextScene;

        Debug.Log("Coroutine finished loading scene");

        yield return null;
    }

    public void PreLoadScene(string sceneName)
    {
        //Loads scene in the background while player is holding a scene in their hand or whatever
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            nextScene = sceneName;
            Debug.LogWarning("NextScene: " + nextScene);
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
    public void PreLoadScene(Scene scene)
    {
        //Loads scene in the background while player is holding a scene in their hand or whatever
        if (!scene.isLoaded)
        {
            nextScene = scene.name;
            Debug.LogWarning("NextScene: " + nextScene);
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        }
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
            Debug.LogWarning("NextScene: " + nextScene);
        if (!SceneManager.GetSceneByName(nextScene).isLoaded)
            PreLoadScene(nextScene);

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

    public void ResetLoadedScenes()
    {

        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name != currentScene &&
                SceneManager.GetSceneAt(i).name != nextScene &&
                SceneManager.GetActiveScene() != SceneManager.GetSceneAt(i))
            {
                Debug.Log("Unloading scene: " + SceneManager.GetSceneAt(i).name +". NextScene: " + nextScene + ", currentScene: " + currentScene);
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }
    }




}
