using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    //height of screen in game units
    //cameras are configured to display this height of the world
    public const float SCREEN_HEIGHT = 10;

    private new Camera camera;

    private Vector3 lastDragPos;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        //move camera to follow player(s)

        var level = GameManager.Instance.level;

        if (!level) { return; }

        var bounds = level.levelBounds;

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
