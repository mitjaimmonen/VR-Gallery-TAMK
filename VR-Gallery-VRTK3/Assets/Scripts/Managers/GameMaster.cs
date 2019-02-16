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
        get;
        set;
    }


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

		Debug.Log("SDK Manager: " + _SDKManager);
		return _SDKManager;
	}



    private void Awake()
    {
        if (!_instance && GameObject.FindGameObjectsWithTag("GameController").Length  == 1)
        {
            DontDestroyOnLoad(this.gameObject);
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        SceneBehaviour();
        SDKManager();

        ControllerSetter.ResetControllers();
        SceneMaster.MasterAwake();
    }

    //Scene manager calls on scene activation
    public void SceneStart()
    {
        SceneBehaviour();
        SDKManager();
        ControllerSetter.ResetControllers();
    }
}
