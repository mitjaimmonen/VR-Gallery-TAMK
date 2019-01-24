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
        SceneManager.activeSceneChanged += OnSceneChanged;
        currentScene = SceneManager.GetActiveScene().name;
    }
    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        if (oldScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(oldScene);
        }
        
        GameMaster.Instance.ActiveSceneChanged(newScene.name);

        GetSceneContainers();
        ResetSceneContainers();

        ResetLoadedScenes();

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

    private IEnumerator LoadAndSwitchScene(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogWarning("Not a valid scene name!");
        }
        else
        {
            Debug.Log("NextScene: " + sceneName);
            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            sceneSwitchInitTime = Time.time;

            //When the load is still in progress, output progress
            while (!asyncOperation.isDone)
            {
                Debug.Log("Scene " + sceneName + " Loading progress: " + (asyncOperation.progress * 100) + "%");

                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    //Wait until delay is also done.
                    if (sceneSwitchInitTime + sceneSwitchDelay < Time.time)
                        //Activate the Scene
                        asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        Debug.Log("Coroutine finished loading scene");
    }
    public void SwitchScene(string sceneName)
    {
        StartCoroutine(LoadAndSwitchScene(sceneName));
    }

    public void ResetLoadedScenes()
    {
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneAt(i))
            {
                Debug.Log("Unloading scene: " + SceneManager.GetSceneAt(i).name +". NextScene: " + nextScene + ", currentScene: " + currentScene);
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }
    }




}
