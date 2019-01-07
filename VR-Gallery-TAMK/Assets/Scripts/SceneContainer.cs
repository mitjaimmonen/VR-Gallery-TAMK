using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SceneContainer : MonoBehaviour
{
    [Header("Debug")]
    public bool loadSceneAtStart = false;
    public bool disableSceneSwitching = false;

    [Header("References")]
    public GameObject visuals;
    public ParticleSystem destroyPS;

    [Header("Parameters")]
    public string sceneName;
    public float breakForce;
    public LayerMask breakingLayers;

    private Rigidbody rb;
    private SceneMaster sceneMaster;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //All references to gamemaster should be done after awake to avoid nulls
        sceneMaster = GameMaster.Instance.SceneMaster;
        if (loadSceneAtStart)
        {
            sceneMaster.LoadSceneAsync(sceneName);
        }
    }

    public void NewSceneLoaded(string sceneName)
    {
        //Broadcast function from SceneMaster
        Debug.Log("Brodacast received");
        visuals.SetActive(true);
        destroyPS.Stop();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (breakingLayers == (breakingLayers | (1 << col.gameObject.layer)))
        {
            if (col.impulse.magnitude > breakForce)
            {
                visuals.SetActive(false);
                destroyPS.Play();
                if (!disableSceneSwitching)
                    sceneMaster.SwitchScene(sceneName);

                Debug.Log("BREAK. force was: " + col.impulse.magnitude);
            }
        }
    }
}
