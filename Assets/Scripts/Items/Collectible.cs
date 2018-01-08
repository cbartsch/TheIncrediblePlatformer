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
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = value;
            }

            GetComponentInChildren<DragController>().enabled = value;

            foreach (var collider in GetComponentsInChildren<Collider2D>())
            {
                collider.enabled = value;
            }
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
