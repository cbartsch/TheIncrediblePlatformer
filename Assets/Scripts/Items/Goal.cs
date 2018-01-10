using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int type = 0;

    public List<RuntimeAnimatorController> typeAnims;
    
    void Start()
    {

    }
    
    void Update()
    {
        //set animation type/color dependent on goal type
        GetComponentInChildren<Animator>().runtimeAnimatorController = typeAnims[type];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player;
        if (other.tag == "Player" && other.isActiveAndEnabled &&
            (player = other.GetComponentInParent<Player>()).PlayerData.type == this.type)
        {
            player.Remove(true);
        }
    }
}
