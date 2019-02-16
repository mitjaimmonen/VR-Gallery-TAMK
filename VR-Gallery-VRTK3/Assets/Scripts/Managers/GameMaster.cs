using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    game
}

public class GameMaster : MonoBehaviour
{
    [SerializeField] private SceneBehaviour sceneBehaviourPrefab;
    [SerializeField] private VRTK.VRTK_SDKManager sdkManagerPrefab;

    private static GameMaster _instance;
    public static GameMaster Instance
    {
        get { return _instance;}
    }

    private SceneMaster _sceneMaster;
    public SceneMaster SceneMaster
    {
        get {
            if (!_sceneMaster)
            {
                _sceneMaster = GetComponent<SceneMaster>();
            }
            return _sceneMaster;
        }
    }
    private ControllerSetter _controllerSetter;
    public ControllerSetter ControllerSetter
    {
        get {
            if (!_controllerSetter)
            {
                _controllerSetter = GetComponent<ControllerSetter>();
            }
            return _controllerSetter;
        }
    }
    private PostProcessingManager _ppManager;
    public PostProcessingManager PostProcessingManager
    {
        get {
            if (!_ppManager)
            {
                _ppManager = GetComponent<PostProcessingManager>();
            }
            return _ppManager;
        }
    }
    private SceneBehaviour _sceneBehaviour;
    public SceneBehaviour SceneBehaviour()
    {
        if (!_sceneBehaviour)
        {
            var temp = GameObject.FindGameObjectWithTag("SceneBehaviour");
            if (temp)
            {
                _sceneBehaviour = temp.GetComponent<SceneBehaviour>();
            }
            else
            {
                _sceneBehaviour = Instantiate(sceneBehaviourPrefab, Vector3.zero, Quaternion.identity);
            }
        }
        return _sceneBehaviour;
    }

    public Camera CurrentCamera
    {
        get{ 
            if (SDKManager() && VRTK.VRTK_DeviceFinder.HeadsetCamera())
                return VRTK.VRTK_DeviceFinder.HeadsetCamera().GetComponent<Camera>();
            else
            {
                Debug.Log("No CurrentCamera found.");
                return null;
            }
        }
    }

    //If sdk manager is missing, new instance is created.
    //If it is in old scene (as new has loaded), move it to new scene
    //Otherwise return current reference.
	private VRTK.VRTK_SDKManager _SDKManager;
	public VRTK.VRTK_SDKManager SDKManager()
	{
		if (!_SDKManager)
		{
			var temp = GameObject.FindGameObjectWithTag("SDKManager");
			if (temp)
				_SDKManager = temp.GetComponent<VRTK.VRTK_SDKManager>();
			if (!_SDKManager)
				_SDKManager = Instantiate(sdkManagerPrefab, Vector3.zero, Quaternion.identity);
		}
        else if (!GameMaster.Instance.SceneMaster.IsInCurrentScene(_SDKManager.gameObject))
        {
            SceneMaster.MoveToScene(_SDKManager.gameObject);
        }

		return _SDKManager;
	}



    private void Awake()
    {
        if (!_instance && GameObject.FindGameObjectsWithTag("GameController").Length  == 1)
        {
            //This gameObject becomes persistent if there are no other gameMasters.
            DontDestroyOnLoad(this.gameObject);
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        //Creates an instance of these if not found.
        SceneBehaviour();
        SDKManager();

        //Controller references in sdk are usually fucked, this ensures they are right.
        ControllerSetter.ResetControllers();

        //Call awake in scenemaster.
        SceneMaster.MasterAwake();
    }

    //CAlled before activating new scene.
    public void SceneLoaded()
    {
        //Reset has to be called before new scene calls awake methods,
        //as sceneBehaviour might override profile.
        PostProcessingManager.ResetProfile();
    }

    //Scene manager calls on scene activation
    //Called before unloading previous scene.
    public void SceneStart()
    {
        SceneBehaviour();
        SDKManager();
        ControllerSetter.ResetControllers();
    }
}
