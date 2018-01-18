using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveItem))]
public class Collectible : MonoBehaviour
{

    public void Collect()
    {
        GetComponent<MoveItem>().Enabled = false;
    }

    public void Reset()
    {
        GetComponent<MoveItem>().Enabled = true;
    }

    public static void ResetAll(Level level)
    {
        if(!level)
        {
            return;
        }
        foreach (var c in level.GetComponentsInChildren<Collectible>())
        {
            c.Reset();
        }
    }
}
