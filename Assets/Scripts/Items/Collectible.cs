using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    bool Enabled
    {
        set
        {
            GetComponentInChildren<Renderer>().enabled = value;
            GetComponentInChildren<Collider2D>().enabled = value;
        }
    }

    public void Collect()
    {
        Enabled = false;
    }

    public void Reset()
    {
        Enabled = true;
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
