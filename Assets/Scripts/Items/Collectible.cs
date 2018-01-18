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
}
