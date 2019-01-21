﻿using System.Collections;
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
        if (loadSceneAtStart)
        {
            sceneMaster.PreLoadScene(sceneName);
        }
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