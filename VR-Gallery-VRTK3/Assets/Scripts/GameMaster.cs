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

    private SceneBehaviour _sceneBehaviour;
    public SceneBehaviour SceneBehaviour
    {
        get{
            if (!_sceneBehaviour)
            {
                var temp = GameObject.FindGameObjectWithTag("SceneBehaviour");
                if (temp)
                {
                    _sceneBehaviour = temp.GetComponent<SceneBehaviour>();
                }
            }
            return _sceneBehaviour;
        }
    }



    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length == 1)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (!Instance)
            _instance = this;
    }

    private void Reset() 
    {

    }

    public void ActiveSceneChanged(string sceneName)
    {
    }
}
