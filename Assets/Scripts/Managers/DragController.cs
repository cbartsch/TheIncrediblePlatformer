using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DragController : MonoBehaviour
{
    //static - only allow 1 drag at a time globally
    public static bool DragActive { get; private set; }
    public static GameObject DragObject { get; private set; }

    public GameObject crosshairPrefab;

    private GameObject crosshair;

    public Collider2D moveAreaCollider;

    private bool Dragging { get; set; }

    private Bounds DragBounds
    {
        get { return moveAreaCollider.bounds; }
    }

    private Vector3 touchPivot;

    private Vector3 Center { get { return moveAreaCollider.bounds.center; } }

    private Vector3 Bottom
    {
        get
        {
            return Center - moveAreaCollider.bounds.extents.y * Vector3.up;
        }
    }

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

    private bool IsDragTarget
    {
        get
        {
            var result = Physics2D.OverlapPoint(InputManager.TouchPosWorld, -1, -10, 10);
            return result && result.gameObject.transform.IsChildOf(gameObject.transform);
        }
    }
    
    void Update()
    {
        bool dragEnabled = Utils.FindCameraAtWorldPos(transform.position) != Camera.main ||
                           GameManager.Instance.Paused || Dragging;
        crosshair.SetActive(dragEnabled);
        if (!dragEnabled)
        {
            return;
        }

        var touchPosWorld = Dragging ? InputManager.TouchPosWorldForDrag : InputManager.TouchPosWorld;

        crosshair.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Dragging ? 1 : 0.2f);

        touchPosWorld.z = DragBounds.center.z;
        if (!DragActive && InputManager.HasTouchDown && IsDragTarget)
        {
            //not dragging but touch is active -> start dragging now
            Dragging = true;
            DragActive = true;
            DragObject = gameObject;

            //on touch screen, move bottom; with mouse, move exact click point
            touchPivot = transform.position - (InputManager.HasTouchScreenTouch ? Bottom : touchPosWorld);
        }
        if (Dragging && !InputManager.HasTouch)
        {
            //dragging but no touch active anymore -> stop dragging now
            Dragging = false;
            DragActive = false;
            DragObject = null;
        }
        if (Dragging)
        {
            //currently dragging -> move position with drag
            transform.position = touchPosWorld + touchPivot;
        }
    }
}
