using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    [SerializeField] private string managerScene;
    [SerializeField] private string startScene;
    [SerializeField] private float sceneSwitchDelay = 2f;

    [Tooltip("FadeoutTime will be overriden by sceneSwitchDelay if it takes longer.")]
    [SerializeField] private bool overrideFadeOutTime = true;
    

    private string currentScene;
    private float sceneSwitchInitTime;
    private List<SceneContainer> sceneContainers = new List<SceneContainer>();
    private MasterCanvas masterCanvas;

    private bool loading;

    public bool IsManagerSceneActive()
    {
        return SceneManager.GetActiveScene().name == managerScene;
    }

    public bool IsInManagerScene(GameObject obj)
    {
        return obj.scene.name == managerScene;
    }

    public void MasterAwake() 
    {
        SceneManager.activeSceneChanged += OnSceneActivated;
        SceneManager.sceneLoaded += OnSceneLoaded;
        masterCanvas = GetComponent<MasterCanvas>();


        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name != managerScene)
            {
                if (SceneManager.GetSceneAt(i).name != SceneManager.GetActiveScene().name)
                {
                    Debug.Log("Setting scene: " + SceneManager.GetSceneAt(i).name + " active.");
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(i));
                }

                currentScene = SceneManager.GetSceneAt(i).name;
                Debug.Log("Active scene at awake is: " + currentScene);
            }
        }

        //If manager scene is only scene. Load default scene.
        if ( !Loadable(currentScene) )
        {
            Debug.Log("Cannot load: " + currentScene + ". Load start-scene.");
            SwitchScene(startScene);
        }
        else if (!SceneManager.GetSceneByName(currentScene).isLoaded)
        {
            Debug.Log("Load currentscene");
            SwitchScene(currentScene);
        }


    }
    private void OnSceneActivated(Scene oldScene, Scene newScene)
    {
        
        Debug.Log("Scene activation: " + newScene.name);
        if (oldScene.name != managerScene && Loadable(oldScene.name) && Loadable(currentScene))
        {
            Debug.Log("Unloading scene:" + oldScene.name);
            SceneManager.UnloadSceneAsync(oldScene);
        }

        currentScene = newScene.name;

        if (currentScene != managerScene)
        {
            GameMaster.Instance.SceneStart();
        }

        if (masterCanvas)
            masterCanvas.FadeIn();
        
        GetSceneContainers();
        ResetSceneContainers();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if ( scene.name != managerScene  && SceneManager.GetActiveScene() != scene)
        {
            SceneManager.SetActiveScene( scene );
        }
    }

    private bool Loadable(string sceneName)
    {
        return Application.CanStreamedLevelBeLoaded(SceneManager.GetSceneByName(sceneName).buildIndex);
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
        if (!Application.CanStreamedLevelBeLoaded(sceneName) || loading)
        {
            Debug.LogWarning("Can not load scene");
        }
        else
        {
            loading = true;
            Debug.Log("NextScene: " + sceneName);
            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            sceneSwitchInitTime = Time.time;
            if (masterCanvas)
            {
                if (overrideFadeOutTime)
                    masterCanvas.ClampFadeout(sceneSwitchDelay);
                masterCanvas.FadeOut();
            }

            //When the load is still in progress, output progress
            while (!asyncOperation.isDone)
            {
                if (masterCanvas)
                {
                    masterCanvas.SetSliderValue(true, asyncOperation.progress * 100, 0);
                    masterCanvas.SetProgressText(true,"Travelling to another world...", 0);
                }
    
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    //Wait until delay is also done.
                    if (sceneSwitchInitTime + sceneSwitchDelay < Time.time)
                    {
                        //Activate the Scene
                        masterCanvas.SetProgressText(true,"Drilling through time and space...", 0);
                        asyncOperation.allowSceneActivation = true;
                    }
                }

                yield return null;
            }
        }

        masterCanvas.SetSliderValue(false,100f, 0);
        masterCanvas.SetSliderValue(false,0, 0.85f);
        masterCanvas.SetProgressText(true,"Arrived to destination.", 0);
        masterCanvas.SetProgressText(false,"", 0.85f);
        loading = false;
    }
    public void SwitchScene(string sceneName)
    {
        StartCoroutine(LoadAndSwitchScene(sceneName));
    }

    public void NextScene()
    {
        //Increment build index by one and start loading the scene.
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        currentIndex++;
        Scene scene = SceneManager.GetSceneByBuildIndex(currentIndex);
        StartCoroutine(LoadAndSwitchScene(scene.name));
    }
}
