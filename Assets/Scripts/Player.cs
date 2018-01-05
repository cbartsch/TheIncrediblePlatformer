using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Serializable]
    public class Data
    {
        public int type;

        public Color Color { get { return TypeColor(type); } }
    }

    public List<RuntimeAnimatorController> typeAnims;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D body;
    public CircleCollider2D feetCircleCollider;

    public Level Level { get; set; }
    public Data PlayerData { get; set; }

    public float jumpForce = 10;
    public float walkForce = 50;
    public float runForce = 100;
    public float drag = 1;
    public float crouchTime = 0.1f;
    public float runDuration = 2;

    enum State { Idle, WalkRight, WalkLeft };

    private State state = State.WalkRight;
    private bool didWalk = false;
    private bool flipped = false;
    private bool groundContact = false;
    private bool crouching = false;
    private bool willJump = false;
    private bool running = false;

    private Vector3 startScale;
    
    void Start()
    {
        startScale = transform.localScale;
    }
    void Update()
    {
        animator.runtimeAnimatorController = typeAnims[PlayerData.type];

        if (willJump && groundContact)
        {
            DoJump();
        }
        else if (groundContact)
        {
            Walk();
        }

        body.drag = groundContact ? drag : 0;

        flipped = false;

        animator.SetBool("Crouching", crouching);
        animator.SetBool("Grounded", groundContact);
        animator.SetBool("Running", running);
        animator.SetFloat("YVel", body.velocity.y);
    }

    void Walk()
    {
        bool moving = Mathf.Abs(body.velocity.x) > 0.01;
        didWalk = didWalk || moving;
        if (!moving && groundContact && didWalk)
        {
            FlipWalkDirection();
        }

        float forceFactor = 0;
        switch (state)
        {
            case State.WalkRight:
                forceFactor = 1;
                transform.localScale = startScale;
                break;
            case State.WalkLeft:
                forceFactor = -1;
                transform.localScale = Vector3.Scale(startScale, new Vector3(-1, 1, 1));
                break;
            case State.Idle:
                break;
        }
        float force = running ? runForce : walkForce;
        body.AddForce(Vector2.right * force * forceFactor * Time.deltaTime * 60);
    }

    public void Jump()
    {
        //can't jump while running
        if (!running)
        {
            crouching = true;
            Invoke("JumpAfterCrouch", crouchTime);
        }
    }

    private void JumpAfterCrouch()
    {
        willJump = true;
    }

    private void DoJump()
    {
        body.velocity = new Vector2(body.velocity.x, 0);
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        willJump = false;
        crouching = false;
    }

    public void Run()
    {
        running = true;
        Invoke("StopRun", runDuration);
    }

    public void StopRun()
    {
        running = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        var collTag = coll.gameObject.tag;

        if (collTag == "KillZone")
        {
            Die();
        }
        if(collTag == "Player" && groundContact)
        {
            FlipWalkDirection();
        }
        if (collTag == "Ground")
        {
            groundContact = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        var collTag = coll.gameObject.tag;
        if (collTag == "Ground")
        {
            groundContact = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var collTag = other.tag;
        Collectible c;
        //collider might be sub-object of collectible item
        if ((c = other.GetComponentInParent<Collectible>()) != null)
        {
            c.Collect();
            Collect(c);
        }
    }

    void Collect(Collectible c)
    {
        if (c.tag == "JumpItem")
        {
            Jump();
        }
        else if (c.tag == "RunItem")
        {
            Run();
        }
    }

    private void FlipWalkDirection()
    {
        if (flipped) return;

        switch (state)
        {
            case State.WalkRight:
                state = State.WalkLeft;
                break;
            case State.WalkLeft:
                state = State.WalkRight;
                break;
            case State.Idle:
                break;
        }

        flipped = true;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public static Color TypeColor(int type)
    {
        switch (type)
        {
            case 1: return Color.red;
            default: return Color.white;
        }
    }
}
