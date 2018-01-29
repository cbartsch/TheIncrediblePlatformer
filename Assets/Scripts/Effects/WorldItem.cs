using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour {
    
    public SpriteRenderer spriteRenderer;

    public List<Sprite> worldSprites;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var worldIndex = GameManager.Instance.WorldIndex;
        spriteRenderer.sprite = worldSprites[worldIndex];
    }
}
