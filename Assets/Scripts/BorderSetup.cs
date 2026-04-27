using UnityEngine;

public class BorderSetup : MonoBehaviour
{
    public Transform top, bottom, left, right;
    public float thickness = 0.3f;

    void Start()
    {
        Camera cam = Camera.main;
        float h = cam.orthographicSize;
        float w = h * cam.aspect;

        top.position = new Vector3(0, h - thickness / 2, 0);
        top.localScale = new Vector3(w * 2, thickness, 1);

        bottom.position = new Vector3(0, -h + thickness / 2, 0);
        bottom.localScale = new Vector3(w * 2, thickness, 1);

        left.position = new Vector3(-w + thickness / 2, 0, 0);
        left.localScale = new Vector3(thickness, h * 2, 1);

        right.position = new Vector3(w - thickness / 2, 0, 0);
        right.localScale = new Vector3(thickness, h * 2, 1);
    }
}