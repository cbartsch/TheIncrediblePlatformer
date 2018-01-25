using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoftPlatform : MonoBehaviour {

    private const float DELTA_Y = 0.01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var ownColliders = GetComponentsInChildren<Collider2D>();
	    var ownBounds = Utils.TotalBounds(ownColliders);

	    foreach (Collider2D ownCollider in ownColliders)
	    {
	        foreach (var player in FindObjectsOfType<Player>())
	        {
	            var playerColliders = player.GetComponentsInChildren<Collider2D>();
	            var playerBounds = Utils.TotalBounds(playerColliders);

	            var isAbove = playerBounds.min.y + DELTA_Y > ownBounds.max.y;

	            foreach (Collider2D playerCollider in playerColliders)
	            {
                    Physics2D.IgnoreCollision(ownCollider, playerCollider, !isAbove);
	            }
	        }
	    }
	}
}
