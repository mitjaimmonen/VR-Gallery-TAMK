using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    [SerializeField]private float sceneSwitchDelay = 2f;
    [Tooltip("FadeoutTime will be overriden by sceneSwitchDelay if it takes longer.")]
    [SerializeField]private bool overrideFadeOutTime = true;
    private float sceneSwitchInitTime;
    private List<SceneContainer> sceneContainers = new List<SceneContainer>();
    private MasterCanvas masterCanvas;

    private void Awake() 
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        masterCanvas = GetComponent<MasterCanvas>();
    }
    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        if (oldScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(oldScene);
        }

        if (masterCanvas)
            masterCanvas.FadeIn();

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
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }
    }




}
