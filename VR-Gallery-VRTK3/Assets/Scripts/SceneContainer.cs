using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SceneContainer : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]private bool disableSceneSwitching = false;

    [Header("References")]
    [SerializeField]private GameObject visuals;
    [SerializeField]private ParticleSystem destroyPS;

    [Header("Parameters")]
    [SerializeField]private string sceneName;
    [SerializeField]private float breakForce;
    [SerializeField]private LayerMask breakingLayers;

    private Rigidbody rb;
    private SceneMaster sceneMaster;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        Debug.Log("SceneContainer Reset");
        sceneMaster = GameMaster.Instance.SceneMaster;
        visuals.SetActive(true);
        rb.isKinematic = false;
        destroyPS.Stop();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (breakingLayers == (breakingLayers | (1 << col.gameObject.layer)))
        {
            if (col.impulse.magnitude > breakForce)
            {
                visuals.SetActive(false);
                rb.isKinematic = true;
                destroyPS.Play();
                if (!disableSceneSwitching)
                    sceneMaster.SwitchScene(sceneName);
            }
        }
    }
}
