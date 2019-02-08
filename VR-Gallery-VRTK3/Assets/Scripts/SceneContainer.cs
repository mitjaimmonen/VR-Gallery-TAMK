using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SceneContainer : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]private bool disableSceneSwitching = false;

    [Header("References")]
    [SerializeField]private ParticleSystem destroyPS;

    [SerializeField]private string sceneName;
    [SerializeField]private float breakForce;
    [SerializeField]private LayerMask breakingLayers;

    private Rigidbody rb;
    private SceneMaster sceneMaster;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        Debug.Log("SceneContainer Reset");
        sceneMaster = GameMaster.Instance.SceneMaster;
        rb.isKinematic = false;
        destroyPS.Stop();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (breakingLayers == (breakingLayers | (1 << col.gameObject.layer)))
        {
            if (col.impulse.magnitude > breakForce)
            {
                rb.isKinematic = true;
                destroyPS.Play();
                if (GetComponent<BreakableMesh>())
                {
                    GetComponent<BreakableMesh>().Break(col.impulse);
                }

                if (!disableSceneSwitching)
                    sceneMaster.SwitchScene(sceneName);
            }
        }
    }
}
