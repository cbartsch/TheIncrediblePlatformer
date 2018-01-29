using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStyle : MonoBehaviour
{
    public Camera gameCamera;
    public SpriteRenderer gameWorldBackground;

    public List<Sprite> gameWorldBackgroundSprites;
    public List<Color> gameWorldBackgroundColors;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var worldIndex = GameManager.Instance.WorldIndex;
	    gameWorldBackground.sprite = gameWorldBackgroundSprites[worldIndex];
	    gameCamera.backgroundColor = gameWorldBackgroundColors[worldIndex];
	}
}
