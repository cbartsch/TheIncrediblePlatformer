using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public GameObject spawnPoint;

    public Bounds levelBounds = new Bounds(
        new Vector3(0, -3, 0),
        new Vector3(21, 9, 0)
        );
    
    public List<Player.Data> playerData = new List<Player.Data> { new Player.Data { type = 0 } };

    // Use this for initialization
    void Start () {

        LevelDecorator.Instance.Decorate(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
