using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DragController : MonoBehaviour
{
    //static - only allow 1 drag at a time globally
    private static bool dragActive = false;

    public GameObject crosshairPrefab;

    public float dragSizeOffset = 4f / 160f;

    private GameObject crosshair;
    
    private Collider2D Collider
    {
        get { return GetComponentInChildren<Collider2D>(); }
    }

    private bool _dragging;
    private bool Dragging
    {
        get { return _dragging; }
        set
        {
            if (value != _dragging)
            {
                Time.timeScale = value ? 0 : 1;
                if (!value)
                {
                    GameManager.Instance.ResetPlayers();
                }
            }
            _dragging = value;
        }
    }

    private Bounds DragBounds
    {
        get
        {
            var bounds = Collider.bounds;
            //scale offset from inches to game units
            var offset = dragSizeOffset * Screen.dpi / Screen.width * CameraController.SCREEN_HEIGHT / transform.lossyScale.x;
            bounds.size += new Vector3(offset, offset);
            return bounds;
        }
    }

    private Vector3 touchPivot;

    private Vector3 Center { get { return transform.position + Collider.bounds.size / 2; } }

    // Use this for initialization
    void Start()
    {
        //attach crosshair to make drag area visible
        crosshair = Instantiate(crosshairPrefab,
            position: Center,
            rotation: Quaternion.identity,
            parent: transform);

        //scale crosshair sprite to drag area
        var chSprite = crosshair.GetComponent<SpriteRenderer>();
        var size = DragBounds.size;
        size.x /= transform.localScale.x;
        size.y /= transform.localScale.y;
        chSprite.size = size;
    }

    // Update is called once per frame
    void Update()
    {
        var touchPosWorld = Dragging ? InputManager.TouchPosWorldForDrag : InputManager.TouchPosWorld;

        crosshair.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Dragging ? 1 : 0.2f);

        touchPosWorld.z = DragBounds.center.z;
        if (!dragActive && InputManager.HasTouchDown && DragBounds.Contains(touchPosWorld))
        {
            Dragging = true;
            dragActive = true;

            //on touch screen, move center; with mouse, move exact click piont
            touchPivot = transform.position - (InputManager.HasTouchScreenTouch ? Center : touchPosWorld);
        }
        if (Dragging && !InputManager.HasTouch)
        {
            Dragging = false;
            dragActive = false;
        }
        if (Dragging)
        {
            transform.position = touchPosWorld + touchPivot;
        }
    }
}
