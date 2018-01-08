using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class InputManager
{
    //amount of inches to drag object above finger on touch screens
    private const float TOUCH_DRAG_OFFSET = 40f / 160f;

    public static bool HasTouchScreenTouch { get { return Input.touchCount > 0; } }
    public static bool HasMouseClick { get { return !HasTouchScreenTouch && Input.GetMouseButton(0); } }
    public static bool HasTouch { get { return HasTouchScreenTouch || HasMouseClick; } }

    public static bool HasTouchScreenTouchDown { get { return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began; } }
    public static bool HasMouseClickDown { get { return !HasTouchScreenTouchDown && Input.GetMouseButtonDown(0); } }
    public static bool HasTouchDown { get { return HasTouchScreenTouchDown || HasMouseClickDown; } }


    public static Vector3 TouchPosScreen
    {
        get
        {
            return HasTouchScreenTouch ? (Vector3)Input.GetTouch(0).position :
                    HasMouseClick ? Input.mousePosition :
                    default(Vector3);
        }
    }

    public static Vector3 TouchPosWorld { get { return Utils.ScreenPosToWorld(TouchPosScreen); } }

    public static Vector3 TouchPosWorldForDrag
    {
        get
        {
            var touchPosScreen = TouchPosScreen;
            if(HasTouchScreenTouch)
            {
                //move up so the dragging object is visible above the finger on touch screens
                touchPosScreen += Vector3.up * Screen.dpi * TOUCH_DRAG_OFFSET;
            }
            return Utils.ScreenPosToWorld(touchPosScreen);
        }
    }
}
