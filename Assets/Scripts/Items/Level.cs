using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour {

    public GameObject spawnPoint;

    public AudioClip musicClip;

    public int minDecors = 10, maxDecors = 20;

    public Bounds levelBounds = new Bounds(
        new Vector3(0, -3, 0),
        new Vector3(21, 9, 0)
        );

    public Bounds ScaledBounds
    {
        get
        {
            var scale = transform.lossyScale;
            return new Bounds(
                Vector3.Scale(levelBounds.center, scale), 
                Vector3.Scale(levelBounds.size, scale)
                );
        }
    }
    
    public List<Player.Data> playerData = new List<Player.Data> { new Player.Data { type = 0 } };
    public int GoalPlayerCount
    {
        get { return playerData.Count(d => d.IsGoalPlayer); }
    }

    void Start () {
        LevelDecorator.Instance.Decorate(this);
	}
	
	void Update () {
		
	}
}
