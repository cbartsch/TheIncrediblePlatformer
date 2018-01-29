using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour, Resettable.IResettable
{
    public bool isContinueGoal = false;
    [SerializeField]
    private bool isClosed = false;
    public int type = 0;

    public List<RuntimeAnimatorController> typeAnims;

    public List<SpriteRenderer> renderers;
    public List<Sprite> openSprites, closedSprites;

    public bool IsClosed { get; set; }
    
    void Start()
    {
        Reset();
    }
    
    void Update()
    {
        var i = 0;
        foreach (var renderer in renderers)
        {
            renderer.sprite = IsClosed ? closedSprites[i] : openSprites[i];
            i++;
        }

        if (!isContinueGoal) //continue goal has special animation
        {
            //set animation type/color dependent on goal type
            GetComponentInChildren<Animator>().runtimeAnimatorController = typeAnims[type];
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player;
        if (other.tag == "Player" && other.isActiveAndEnabled && !IsClosed &&
            (player = other.GetComponentInParent<Player>()).PlayerData.type == this.type)
        {
            player.Remove(hasReachedGoal:true, isContinueGoal:this.isContinueGoal);
        }
    }

    public void Reset(bool resetLevelPosition = false)
    {
        IsClosed = isClosed;
    }
}
