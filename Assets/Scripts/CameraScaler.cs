using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float targetAspect = 16f / 9f;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        cam.orthographicSize = cam.orthographicSize / scaleHeight;
    }
}