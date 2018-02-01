using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public static PhysicsManager Instance
    {
        get;
        private set;
    }

    public interface PhysicsBehavior
    {
        void PhysicsUpdate();
    }

    public const int UPDATES_PER_SECOND = 50;

    private float deltaTime;
    private float remainingTime;

    private readonly List<PhysicsBehavior> physicsBehaviors = new List<PhysicsBehavior>();

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        deltaTime = 1f / UPDATES_PER_SECOND;
    }

    public void RegisterPhysicsBehavior(PhysicsBehavior behavior)
    {
        physicsBehaviors.Add(behavior);
    }
    
    void Update()
    {
        remainingTime -= Time.deltaTime;
        while (remainingTime < 0)
        {
            Physics2D.Simulate(deltaTime);
            remainingTime += deltaTime;

            foreach (var pb in physicsBehaviors)
            {
                pb.PhysicsUpdate();
            }
        }
    }
}