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
    [Range(-2f,2f), Tooltip("Change gravity strength. 1 = Default gravity.")]
    [SerializeField]private float gravityScale = 1f;
    [SerializeField]private string sceneName;
    [SerializeField]private float breakForce;
    [SerializeField]private LayerMask breakingLayers;

    private Rigidbody rb;
    private SceneMaster sceneMaster;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void Reset()
    {
        Debug.Log("SceneContainer Reset");
        sceneMaster = GameMaster.Instance.SceneMaster;
        visuals.SetActive(true);
        rb.isKinematic = false;
        destroyPS.Stop();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * rb.mass * gravityScale);
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
