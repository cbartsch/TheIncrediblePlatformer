using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveItem : MonoBehaviour, Resettable.IResettable {

    public bool usePhysics = false;

    private Vector3 startPosition, dropPosition;
    private Quaternion startRotation;

    internal bool Enabled
    {
        set
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = value;
            }

            GetComponentInChildren<DragController>().enabled = value;

            foreach (var collider in GetComponentsInChildren<Collider2D>())
            {
                collider.enabled = value;
            }
        }
    }

    void Awake ()
	{
        startPosition = transform.position;
        startRotation = transform.rotation;
	    dropPosition = startPosition;
	}
	
	void Update ()
	{
	    if (DragController.DragObject == gameObject)
	    {
	        transform.rotation = startRotation;
	        dropPosition = transform.position;
	    }
        else if (GameManager.Instance.Paused)
	    {
	        Reset();
	    }

        //enable physics only if property is set and if item is currently dropped in game area
        var body = GetComponent<Rigidbody2D>();
        if (body)
        {
            var position = GetComponent<Collider2D>().bounds.center;
            var isInLevel = GameCameraController.IsInMainCamera(position);
            var playersDespawning = FindObjectsOfType<Player>().Any(p => p.Despawning);
            body.bodyType = usePhysics && isInLevel && !playersDespawning ? 
                RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        }
    }

    public void Reset(bool resetLevelPosition = false)
    {
        if (resetLevelPosition)
        {
            dropPosition = startPosition;
        }

        Enabled = true;
        transform.rotation = startRotation;
        transform.position = dropPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var collTag = other.tag;

        if (collTag == "KillZone")
        {
            Enabled = false;
        }
    }

}
