using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Key : MonoBehaviour, Resettable.IResettable
{
    private bool isCollected;

    public int type = 0;

    public SpriteRenderer renderer;
    public List<Sprite> typeSprites, typeSpritesCollected;

    public SoundEffects sounds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    renderer.sprite = (isCollected ? typeSpritesCollected : typeSprites)[type];
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance.Paused) return;

        var collTag = other.tag;

        if (collTag == "Player" && !isCollected)
        {
            collect();
        }
    }

    private void collect()
    {
        isCollected = true;
        sounds.PlayKey();
        foreach (var goal in FindObjectsOfType<Goal>())
        {
            if (goal.type == this.type)
            {
                goal.IsClosed = false;
            }
        }
    }

    public void Reset(bool resetLevelPosition)
    {
        isCollected = false;
    }
}
