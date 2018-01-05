using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int type = 0;

    public List<RuntimeAnimatorController> typeAnims;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<Animator>().runtimeAnimatorController = typeAnims[type];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player;
        if (other.tag == "Player" && other.isActiveAndEnabled &&
            (player = other.GetComponentInParent<Player>()).PlayerData.type == this.type)
        {
            Destroy(player.gameObject);
            GameManager.Instance.GoalReached();
        }
    }
}
