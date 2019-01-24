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
    private Vector3 spawnPos;
    private Quaternion spawnRot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spawnPos = transform.position;
        spawnRot = transform.rotation;
    }

    public void Reset()
    {
        Debug.Log("SceneContainer Reset");
        sceneMaster = GameMaster.Instance.SceneMaster;
        visuals.SetActive(true);
        rb.isKinematic = false;
        transform.position = spawnPos;
        transform.rotation = spawnRot;
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

                Debug.Log("BREAK. force was: " + col.impulse.magnitude);
            }
        }
    }
}
