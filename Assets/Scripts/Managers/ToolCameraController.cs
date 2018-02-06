using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCameraController : MonoBehaviour
{
    public static ToolCameraController Instance { get; private set; }

    private new Camera camera;

    public SpriteRenderer toolAreaRenderer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        var sz = toolAreaRenderer.size;
        sz.x = sz.y * GameCameraController.SCREEN_WIDTH / camera.orthographicSize;
        toolAreaRenderer.size = sz;
    }

    public bool HasMoveItems
    {
        get
        {
            var origin = transform.position;
            var size = new Vector2(GameCameraController.SCREEN_WIDTH, camera.orthographicSize);
            var hit = Physics2D.BoxCast(origin, size, 0, Vector2.zero, 0);
            return hit.collider != null;
        }
    }
}
