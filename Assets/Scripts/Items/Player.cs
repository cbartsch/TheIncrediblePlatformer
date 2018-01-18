using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //when walking slower than this, player turns around
    private const float MIN_WALK_VELOCITY = 0.8f;

    [Serializable]
    public class Data
    {
        public int type;
    }

    [Serializable]
    public class VisualData
    {
        public RuntimeAnimatorController animator;
        public Sprite startSprite;
    }

    public List<VisualData> typeVisuals;

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
    public float spawnTime = 0.3f;

    //called after despawn animation
    public event Action GoalReached;

    enum State { Idle, WalkRight, WalkLeft };

    private State state = State.WalkRight;

    private bool didWalk = false;
    private bool flipped = false;
    private int numGroundContacts = 0;
    private bool crouching = false;
    private bool willJump = false;
    private bool running = false;
    private bool spawning = false;
    private bool hasReachedGoal = false;
    public bool Despawning { get; private set; }

    private bool GroundContact { get { return numGroundContacts > 0; } }

    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
        spawning = true;
        var c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;
        spriteRenderer.sprite = typeVisuals[PlayerData.type].startSprite;
        animator.enabled = false;
    }

    void Update()
    {
        var enabled = !spawning && !Despawning;
        animator.enabled = enabled;
        var groundCrouch = crouching && GroundContact;
        body.simulated = enabled && !groundCrouch;

        if (spawning)
        {
            Color c = spriteRenderer.color;
            c.a += Time.deltaTime / spawnTime;
            if (c.a >= 1)
            {
                c.a = 1;
                spawning = false;
            }
            spriteRenderer.color = c;
            return;
        }
        if (Despawning)
        {
            Color c = spriteRenderer.color;
            c.a -= Time.deltaTime / spawnTime;
            if (c.a <= 0)
            {
                c.a = 0;
                Despawning = false;

                if (hasReachedGoal && GoalReached != null)
                {
                    this.GoalReached();
                }

                Destroy(gameObject);
            }
            spriteRenderer.color = c;
            return;
        }

        animator.runtimeAnimatorController = typeVisuals[PlayerData.type].animator;

        //can't jump while in the air
        if (willJump && GroundContact)
        {
            DoJump();
        }
        else if (GroundContact)
        {
            Walk();
        }

        body.drag = GroundContact ? drag : 0;

        flipped = false;

        animator.SetBool("Crouching", crouching);
        animator.SetBool("Grounded", GroundContact);
        animator.SetBool("Running", running);
        animator.SetFloat("YVel", body.velocity.y);
    }

    void Walk()
    {
        bool moving = (state == State.WalkRight && body.velocity.x > MIN_WALK_VELOCITY) ||
            (state == State.WalkLeft && body.velocity.x < -MIN_WALK_VELOCITY);
        didWalk = didWalk || moving;
        if (!moving && GroundContact && didWalk)
        {
            didWalk = false;
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
        running = false;
        crouching = true;
        Invoke("JumpAfterCrouch", crouchTime);
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
        if (collTag == "Player" && GroundContact)
        {
            FlipWalkDirection();
        }
        if (collTag == "Ground")
        {
            numGroundContacts++;
            Debug.Log("enter ground contact with " + coll.gameObject.name + " #" + numGroundContacts);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        var collTag = coll.gameObject.tag;
        if (collTag == "Ground")
        {
            numGroundContacts--;
            Debug.Log("exit ground contact with " + coll.gameObject.name + " #" + numGroundContacts);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(GameManager.Instance.Paused) return;

        var collTag = other.tag;

        if (collTag == "KillZone")
        {
            Remove();
        }

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

    public void Remove(bool hasReachedGoal = false)
    {
        Despawning = true;
        if (hasReachedGoal)
        {
            this.hasReachedGoal = true;
        }
    }
}
