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
        var worldIndex = Mathf.Max(GameManager.Instance.WorldIndex, 0);
        spriteRenderer.sprite = worldSprites[worldIndex];
    }
}
