using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //height of screen in game units
    //cameras are configured to display this height of the world
    public const float SCREEN_HEIGHT = 12;

    private Camera camera;

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

        //find all players
        var players = level.GetComponentsInChildren<Player>();

        if (players == null || players.Length == 0) { return; }

        var playerPos = computePlayerPos(players);

        var range = computeRange(bounds);

        var newPos = computeNewPos(bounds, range, playerPos);

        applyPosition(newPos);
    }

    private static Vector3 computePlayerPos(Player[] players)
    {
        //find average of all players' positions (sum / length)
        var positions = players.Select(p => p.transform.position);
        var playerPos = positions.Aggregate((a, b) => a + b) / players.Length;
        return playerPos;
    }

    private Vector3 computeNewPos(Bounds bounds, Vector3 range, Vector3 playerPos)
    {
        //compute max and min position of camera - center of level plus/minus the range
        var maxPos = bounds.center + range;
        var minPos = bounds.center - range;

        //set camera X/Y position to player position but clamp to max and min position
        return new Vector3(
            Mathf.Clamp(playerPos.x, minPos.x, maxPos.x),
            Mathf.Clamp(playerPos.y, minPos.y, maxPos.y),
            camera.transform.position.z
        );
    }

    private Vector3 computeRange(Bounds bounds)
    {
        //compute range the camera can move - difference of level size and camera size
        var range = bounds.extents - new Vector3(
                        x: camera.orthographicSize * (float)camera.pixelWidth / camera.pixelHeight,
                        y: camera.orthographicSize);

        //if range is < 0, camera can not move at all - clamp to 0 in this case
        return new Vector3(Mathf.Max(range.x, 0), Mathf.Max(range.y, 0));
    }

    private void applyPosition(Vector3 newPos)
    {
        var diff = newPos - camera.transform.position;

        //if position change is small, apply directly, otherwise apply gradually using lerp
        camera.transform.position = diff.sqrMagnitude < 0.1 ? newPos :
            Vector3.Lerp(camera.transform.position, newPos, Time.deltaTime * 10);
    }

    public static bool IsInMainCamera(Vector3 position)
    {
        var camera = Utils.FindCameraAtWorldPos(position);
        return camera && camera.tag == "MainCamera";
    }
}
