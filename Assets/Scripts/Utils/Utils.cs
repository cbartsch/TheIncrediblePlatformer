using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Utils
{
    //find camera displaying at screen pixel position
    public static Camera FindCameraAtScreenPos(Vector2 pixelPos)
    {
        foreach (var camera in Camera.allCameras)
        {
            var relativePos = camera.ScreenToViewportPoint(pixelPos);
            if (new Rect(0, 0, 1, 1).Contains(relativePos))
            {
                return camera;
            }
        }
        return null;
    }

    //find camera displaying at world position
    public static Camera FindCameraAtWorldPos(Vector3 worldPos)
    {
        foreach (var camera in Camera.allCameras)
        {
            var relativePos = camera.WorldToViewportPoint(worldPos);
            if (new Rect(0, 0, 1, 1).Contains(relativePos))
            {
                return camera;
            }
        }
        return null;
    }

    public static Vector3 ScreenPosToWorld(Vector3 screenPos)
    {
        Camera camera = FindCameraAtScreenPos(screenPos);
        return camera ? camera.ScreenToWorldPoint(screenPos) : default(Vector3);
    }
}
