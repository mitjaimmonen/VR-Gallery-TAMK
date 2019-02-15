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
    [SerializeField] private VRTK.VRTK_SDKManager vrtkPrefab;
    [SerializeField] private SceneBehaviour sceneBehaviourPrefab;
    private VRTK.VRTK_SDKManager _vrtk;
    public VRTK.VRTK_SDKManager vrtk
    {
        get
        {
            if (!_vrtk)
            {
                _vrtk = GetComponentInChildren<VRTK.VRTK_SDKManager>();
                if (!_vrtk)
                {
                    _vrtk = Instantiate(vrtkPrefab, Vector3.zero, Quaternion.identity);
                }
            }
            return _vrtk;
        }
    }

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

    public GameObject GetInputSimulator()
    {
        return GetComponentInChildren<VRTK.SDK_InputSimulator>().gameObject;
    }




    private void Awake()
    {
        if (SceneMaster.IsInManagerScene(this.gameObject))
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        ControllerSetter.ResetControllers();
        SceneMaster.MasterAwake();
    }

    public void SceneStart()
    {
        SceneBehaviour();
        ControllerSetter.ResetControllers();
    }
}
