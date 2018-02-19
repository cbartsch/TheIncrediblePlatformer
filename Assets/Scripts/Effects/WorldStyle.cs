using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStyle : MonoBehaviour
{
    public SpriteRenderer gameWorldBackground;

    public List<Sprite> gameWorldBackgroundSprites;
    public List<Color> gameWorldBackgroundColors;
    public List<Color> gameWorldBottomBackgroundColors;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var worldIndex = Mathf.Max(GameManager.Instance.WorldIndex, 0);
	    gameWorldBackground.sprite = gameWorldBackgroundSprites[worldIndex];

	    foreach (var camera in FindObjectsOfType<Camera>())
	    {
	        camera.backgroundColor = gameWorldBackgroundColors[worldIndex];
        }

	    GameCameraController.Instance.bottomBackgroundSprite.color = gameWorldBottomBackgroundColors[worldIndex];
	}
}
