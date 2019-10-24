using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VRTK;
public class CanvasCameraSetter : MonoBehaviour
{
    public Vector3 position = Vector3.zero;
    public float planeDistance;
    protected Canvas canvas;

    void OnEnable()
    {
        InitCanvas();
    }

    void Update()
    {
        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
        }
        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;

            if (canvas.worldCamera == null)
                canvas.enabled = false;
            else
                canvas.enabled = true;
        }
        else
        {
            canvas.planeDistance = planeDistance;
        }

    }


    void InitCanvas()
    {
        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
        }
        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }
}
