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
            var delta = 0.1f;
            if (new Rect(-delta, -delta, 1 + 2*delta, 1 + 2*delta).Contains(relativePos))
            {
                return camera;
            }
        }
        return null;
    }

    //transform a position from contained camera's pixel space to world space
    public static Vector3 ScreenPosToWorld(Vector3 screenPos)
    {
        Camera camera = FindCameraAtScreenPos(screenPos);
        return camera ? camera.ScreenToWorldPoint(screenPos) : default(Vector3);
    }

    //scale a vector from main camera pixel space to world space
    public static Vector3 ScaleScreenToWorld(Vector3 screenVector)
    {
        Camera camera = Camera.main;
        return screenVector * camera.orthographicSize * 2 / camera.rect.height / Screen.height;
    }

    public static Bounds TotalBounds(Collider2D[] allColliders)
    {
        return allColliders.Select(p => p.bounds).Aggregate((a, b) =>
        {
            a.Encapsulate(b);
            return a;
        });
    }
}
