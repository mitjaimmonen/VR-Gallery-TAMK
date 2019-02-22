using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    [SerializeField] private float sceneSwitchDelay = 2f;

    [Tooltip("FadeoutTime will be overriden by sceneSwitchDelay if it takes longer.")]
    [SerializeField] private bool overrideFadeOutTime = true;
    

    private float sceneSwitchInitTime;
    private List<SceneContainer> sceneContainers = new List<SceneContainer>();
    private MasterCanvas masterCanvas;

    private bool loading;

    public bool IsInCurrentScene(GameObject obj)
    {
        return obj.scene == SceneManager.GetActiveScene();
    }

    public string GetNameFromPath(string path)
    {
        string[] asd = path.Split("/Scenes/"[0]);
        asd = asd[2].Split(".Unity"[0]);
        return asd[0];
    }


    public void MoveToScene(GameObject objToMove, string sceneName = null)
    {
        if (Loadable(sceneName))
        {
            SceneManager.MoveGameObjectToScene(objToMove, SceneManager.GetSceneByName(sceneName));
        }
        else if (objToMove != null)
        {
            SceneManager.MoveGameObjectToScene(objToMove, SceneManager.GetActiveScene());
        }
    }


    public void MasterAwake() 
    {
        SceneManager.activeSceneChanged += OnSceneActivated;
        SceneManager.sceneLoaded += OnSceneLoaded;
        masterCanvas = GetComponent<MasterCanvas>();
    }


    private void OnSceneActivated(Scene oldScene, Scene newScene)
    {
        Debug.Log("Scene activation: " + newScene.name);

        GameMaster.Instance.SceneStart();
        
        if (Loadable(oldScene.name) && oldScene.isLoaded)
        {
            Debug.Log("Unloading scene:" + oldScene.name);
            SceneManager.UnloadSceneAsync(oldScene);
        }


        if (masterCanvas)
            masterCanvas.FadeIn();
        
        GetSceneContainers();
        ResetSceneContainers();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if ( SceneManager.GetActiveScene() != scene )
        {
            GameMaster.Instance.SceneLoaded();
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
            Debug.LogWarning("Can not load scene: " + sceneName + ", loading: " + loading);
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

    //NOT WORKING NO IDEA WHY
    public void NextScene()
    {
        //Increment build index by one and start loading the scene.
        int currentIndex = SceneManager.GetActiveScene().buildIndex;    // works
        currentIndex++;                                                 // works
        Scene scene = SceneManager.GetSceneByBuildIndex(currentIndex);  // doesnt work
        StartCoroutine(LoadAndSwitchScene(scene.name));
    }
}
