using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{

    public Font font;

	// Use this for initialization
	void Start ()
	{
	    font.material.mainTexture.filterMode = FilterMode.Point;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
