﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour {
    
    public SpriteRenderer spriteRenderer;

    public List<Sprite> worldSprites;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        applySprite();
    }

    // Update is called once per frame
    void Update()
    {
        applySprite();
    }

    private void applySprite()
    {
        var worldIndex = Mathf.Max(GameManager.Instance.WorldIndex, 0);
        spriteRenderer.sprite = worldSprites[worldIndex];
    }
}
