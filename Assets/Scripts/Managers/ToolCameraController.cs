using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCameraController : MonoBehaviour
{
    private new Camera camera;

    public SpriteRenderer toolAreaRenderer;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        var sz = toolAreaRenderer.size;
        sz.x = sz.y * Screen.width / Screen.height * GameCameraController.SCREEN_HEIGHT / camera.orthographicSize;
        toolAreaRenderer.size = sz;
    }
}
