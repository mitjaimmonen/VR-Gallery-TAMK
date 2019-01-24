using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VRTK;
public class CanvasCameraSetter : MonoBehaviour
{
    public Vector3 position = Vector3.zero;
    public float planeDistance;
    protected Canvas canvas;
    private Camera cam;

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
        if (cam == null)
        {
            Transform sdkCamera = VRTK_DeviceFinder.HeadsetCamera();
            if (sdkCamera)
                cam = sdkCamera.GetComponent<Camera>();
        }
        if (canvas.worldCamera == null && cam != null)
        {
            canvas.worldCamera = cam;
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
        if (cam == null)
        {
            Transform sdkCamera = VRTK_DeviceFinder.HeadsetCamera();
            if (sdkCamera)
                cam = sdkCamera.GetComponent<Camera>();
        }
        if (canvas.worldCamera == null && cam != null)
        {
            canvas.worldCamera = cam;
        }
    }
}
