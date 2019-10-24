using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnTriggerEnter : MonoBehaviour {

    GameObject go;

    void Awake()
    {
        go = transform.GetChild(0).gameObject;
    }

	void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "controller")
        {
            go.SetActive(true);
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "controller")
        {
            go.SetActive(false);
        }
    }
}
