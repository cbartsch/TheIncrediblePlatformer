using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZoneEffect : MonoBehaviour
{
    public new Renderer renderer;
    public float scrollTime = 1;
    public Vector2 offsetScale = new Vector2(1, 0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float offset = Mathf.Repeat(Time.time / scrollTime, 1);
        Vector2 textureOffset = Vector2.Scale(new Vector2(offset, offset), offsetScale);

	    renderer.material.SetFloat("_XOffset", textureOffset.x);
	    renderer.material.SetFloat("_YOffset", textureOffset.y);
	}
}
