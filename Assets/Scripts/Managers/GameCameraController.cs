using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    public static GameCameraController Instance { get; private set; }

    //height of screen in game units
    //cameras are configured to display this height of the world
    public const float SCREEN_HEIGHT = 10;

    public static float SCREEN_WIDTH
    {
        get { return (float) Screen.width / Screen.height * GameCameraController.SCREEN_HEIGHT; }
    }

    public Camera topBackgroundCamera;
    public SpriteRenderer bottomBackgroundSprite;

    private new Camera camera;

    private Vector3 lastDragPos;

    private float startHeight, startSize;

    void Start()
    {
        Instance = this;
        camera = GetComponent<Camera>();
        startHeight = camera.rect.height;
        startSize = camera.orthographicSize;
    }

    void Update()
    {
        adjustViewports();

        //move camera to follow player(s)

        var level = GameManager.Instance.level;

        if (!level) { return; }

        var bounds = level.ScaledBounds;

        Vector3 cameraPos = camera.transform.position;
        bool interpolate;
        if (!DragController.DragActive && InputManager.HasTouch)
        {
            var pos = InputManager.TouchPosScreen;
            if (InputManager.HasTouchDown)
            {
                lastDragPos = pos;
            }
            if (InputManager.HasTouch)
            {
                var diff = lastDragPos - pos;
                cameraPos += Utils.ScaleScreenToWorld(diff);
                lastDragPos = pos;
            }

            //don't interpolate dragging
            interpolate = false;
        }
        else
        { 
            //find all players
            var players = level.GetComponentsInChildren<Player>();

            if (players == null || players.Length == 0) { return; }

            interpolate = true;
            cameraPos = computePlayerPos(players);
        }

        var range = computeRange(bounds);

        var newPos = computeNewPos(bounds, range, cameraPos);

        applyPosition(newPos, interpolate);
    }

    private void adjustViewports()
    {
        //levels are designed at 16:9 ratio
        //- at wider ratios, there is more space on the screen to the left and right, which is 
        //- at narrower ratios, there would be less space on left and right and thus scrolling needed
        //  -> scale camera height down (providing more width) so the 16:9 design fits on screen completely
        var aspectRatio = (float) Screen.width / Screen.height;
        var REFERENCE_RATIO = 16.0f / 9.0f;
        var scale = Math.Min(1, aspectRatio / REFERENCE_RATIO);
        var cameraHeight = startHeight * scale;
        var bgHeight = startHeight - cameraHeight;
        
        camera.rect = new Rect(0, 1 - startHeight, 1, cameraHeight);
        
        topBackgroundCamera.rect = new Rect(0, 1 - bgHeight, 1, bgHeight);
        topBackgroundCamera.orthographicSize = startSize * bgHeight / cameraHeight;
        topBackgroundCamera.transform.localPosition = new Vector3(0, camera.orthographicSize + topBackgroundCamera.orthographicSize);
        topBackgroundCamera.gameObject.SetActive(topBackgroundCamera.orthographicSize > 0.1);
    }

    private static Vector3 computePlayerPos(Player[] players)
    {
        //find average of all players' positions (sum / length)
        var positions = players.Select(p => p.transform.position);
        return positions.Aggregate((a, b) => a + b) / players.Length;
    }

    private Vector3 computeNewPos(Bounds bounds, Vector3 range, Vector3 cameraPos)
    {
        //compute max and min position of camera - center of level plus/minus the range
        var maxPos = bounds.center + range;
        var minPos = bounds.center - range;

        //set camera X/Y position to player position but clamp to max and min position
        return new Vector3(
            Mathf.Clamp(cameraPos.x, minPos.x, maxPos.x),
            Mathf.Clamp(cameraPos.y, minPos.y, maxPos.y),
            camera.transform.position.z
        );
    }

    private Vector3 computeRange(Bounds bounds)
    {
        float aspectRatio = (float) camera.pixelWidth / camera.pixelHeight;

        //compute range the camera can move - difference of level size and camera size
        var range = bounds.extents - new Vector3(
                        x: camera.orthographicSize * aspectRatio,
                        y: camera.orthographicSize);

        //if range is < 0, camera can not move at all - clamp to 0 in this case
        return new Vector3(Mathf.Max(range.x, 0), Mathf.Max(range.y, 0));
    }

    private void applyPosition(Vector3 newPos, bool interpolate)
    {
        var diff = newPos - camera.transform.position;

        //if position change is small, apply directly, otherwise apply gradually using lerp
        camera.transform.position = diff.sqrMagnitude < 1 || !interpolate ? newPos :
            Vector3.Lerp(camera.transform.position, newPos, Time.deltaTime * 5);
    }

    public static bool IsInMainCamera(Vector3 position)
    {
        var camera = Utils.FindCameraAtWorldPos(position);
        return camera && camera.tag == "MainCamera";
    }
}
