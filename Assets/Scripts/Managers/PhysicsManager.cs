using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{

    public int updatesPerSecond = 30;

    private float deltaTime;
    private float remainingTime;

	// Use this for initialization
	void Start ()
	{
	    deltaTime = 1f / updatesPerSecond;
	    remainingTime = deltaTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    remainingTime -= Time.deltaTime;
	    while (remainingTime < 0)
	    {
	        Physics2D.Simulate(deltaTime);
	        remainingTime += deltaTime;
	    }
	}
}
