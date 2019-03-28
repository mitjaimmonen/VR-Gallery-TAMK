using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SceneContainer : MonoBehaviour
{


    [SerializeField] private float sceneCallDelay = 0.5f;
    [SerializeField]private ParticleSystem destroyPS;

    // [SerializeField]private string sceneName;
    public SceneReference sceneToLoad;

    [SerializeField]private float breakForce;
    [SerializeField]private LayerMask breakingLayers;

    private Rigidbody rb;


    private void OnGUI()
    {
        DisplayLevel(sceneToLoad);
    }

    public void DisplayLevel(SceneReference scene)
    {
        GUILayout.Label(new GUIContent("Scene name Path: " + scene));
        if (GUILayout.Button("Load " + scene))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        Debug.Log("SceneContainer Reset");
        rb.isKinematic = false;
        if (destroyPS)
            destroyPS.Stop();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (breakingLayers == (breakingLayers | (1 << col.gameObject.layer)))
        {
            if (col.impulse.magnitude > breakForce)
            {
                rb.isKinematic = true;
                if (destroyPS)
                    destroyPS.Play();
                if (GetComponent<BreakableMesh>())
                {
                    GetComponent<BreakableMesh>().Break(col.impulse);
                }

                Invoke("SwitchScene", sceneCallDelay);
            }
        }
    }

    private void SwitchScene()
    {
        var sm = GameMaster.Instance.SceneMaster;
        if (sceneToLoad != null)
        {
            Debug.Log("Switch scene called: " + sceneToLoad.ScenePath);
            sm.SwitchScene(sm.GetNameFromPath(sceneToLoad.ScenePath));
        }
    }
}
