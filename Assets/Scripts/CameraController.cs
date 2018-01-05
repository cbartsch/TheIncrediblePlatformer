using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //height of screen in game units
    //cameras are configured to display this height of the world
    public const float SCREEN_HEIGHT = 12;

       
    void Update()
    {
        //move camera to follow player(s)

        var camera = GetComponent<Camera>();
        var level = GameManager.Instance.level;

        if (!level) { return; }

        var bounds = level.levelBounds;

        //find all players
        var players = level.GetComponentsInChildren<Player>();

        if (players == null || players.Length == 0) { return; }

        //find average of all players' positions (sum / length)
        var positions = players.Select(p => p.transform.position);
        var playerPos = positions.Aggregate((a, b) => a + b) / players.Length;

        //compute range the camera can move - difference of level size and camera size
        var range = bounds.extents - new Vector3(
            x: camera.orthographicSize * (float)camera.pixelWidth / camera.pixelHeight, 
            y: camera.orthographicSize);

        //if range is < 0, camera can not move at all - clamp to 0 in this case
        range = new Vector3(Mathf.Max(range.x, 0), Mathf.Max(range.y, 0));

        //compute max and min position of camera - center of level plus/minus the range
        var maxPos = bounds.center + range;
        var minPos = bounds.center - range;

        //set camera X/Y position to player position but clamp to max and min position
        var newPos = new Vector3(
             Mathf.Clamp(playerPos.x, minPos.x, maxPos.x),
             Mathf.Clamp(playerPos.y, minPos.y, maxPos.y),
             camera.transform.position.z
            );

        //apply position change gradually using lerp
        camera.transform.position = Vector3.Lerp(camera.transform.position, newPos, Time.deltaTime * 10);
    }
}
