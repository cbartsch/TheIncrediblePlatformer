using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItem : MonoBehaviour {

    public bool usePhysics = false;

    private Vector3 startPosition;
    private Quaternion startRotation;

	void Start () {
        startPosition = transform.position;
        startRotation = transform.rotation;
	}
	
	void Update () {
	    if (DragController.DragObject == gameObject)
	    {
	        transform.rotation = startRotation;
	    }

        //enable physics only if property is set and if item is currently dropped in game area
        var body = GetComponent<Rigidbody2D>();
        if (body)
        {
            var position = GetComponent<Collider2D>().bounds.center;
            body.bodyType = usePhysics && CameraController.IsInMainCamera(position) ?
                RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var collTag = other.tag;

        if (collTag == "KillZone")
        {
            //reset position if item falls out of the level
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
    }
 }
