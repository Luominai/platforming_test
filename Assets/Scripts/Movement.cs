using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    // Public variables are adjustable from the inspector window
    public float acceleration;
    public float decceleration;
    public float topSpeed;
    public float rayLength;
    public float sideBuffer;
    public Rigidbody2D rigidBody;
    public BoxCollider2D boxCollider;

    // Current velocity
    private float xVelocity;
    private float yVelocity;

    // Get when the jump button (space) is pressed and released
    public float jumpBuffer;
    private bool jumpBuffered;
    private float jumpBufferTime;
    private bool jumpButtonDown;
    private bool jumpButtonUp;
    private bool isGrounded;
    private bool jumpCanceled;

    // Gets horizontal and vertical inputs (-1, 0, or 1)
    // W: Positive Y value
    // A: Negative X value
    // S: Negative Y value
    // D: Positive X value
    private float xInput;
    private float yInput;

    private bool facingLeft = true;

    // Specify which layer is the ground
    public LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        groundCheck();
        getInput();
        updateSpeed();
        jump();
        move();
        flip();
    }

    // Collecting input
    void getInput()
    {
        jumpButtonDown = Input.GetButtonDown("Jump");
        jumpButtonUp = Input.GetButtonUp("Jump");
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    void updateSpeed()
    {
        // If no input, accelerate towards 0
        if (xInput == 0)
        {
            xVelocity = Mathf.MoveTowards(xVelocity, 0, decceleration * Time.deltaTime);
        }
        else
        {
            var direction = Mathf.Sign(xInput);
            xVelocity = Mathf.MoveTowards(xVelocity, topSpeed * direction, acceleration * Time.deltaTime);
        }
    }

    void groundCheck()
    {
        Vector2 center = boxCollider.bounds.center;
        float extentsX = boxCollider.bounds.extents.x - sideBuffer;
        float extentsY = boxCollider.bounds.extents.y;
        Vector2 leftCorner = center + new Vector2(-extentsX, -extentsY);
        Vector2 rightCorner = center + new Vector2(extentsX, -extentsY);

        var hit = Physics2D.BoxCast(center, new Vector2(2 * extentsX, 2 * extentsY), 0f, Vector2.down, rayLength, ground);
        Debug.DrawRay(leftCorner, new Vector2(0, -rayLength), Color.green);
        Debug.DrawRay(rightCorner,  new Vector2(0, -rayLength), Color.green);
        Debug.DrawRay(leftCorner - new Vector2(0, rayLength), new Vector2(2 * extentsX, 0), Color.green);

        if (hit.collider)
        {
            Debug.Log("Touching Ground");
            isGrounded = true;
            jumpCanceled = false;

            if (jumpBuffered && (Time.time - jumpBufferTime < jumpBuffer))
            {
                rigidBody.velocity = new Vector2(xVelocity, 10);
            }
            jumpBuffered = false;

        } else
        {
            isGrounded = false;
        }
    }

    void jump()
    {
        if (jumpButtonDown && isGrounded)
        {
            // rigidBody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            rigidBody.velocity = new Vector2(xVelocity, 10);
        }
        if (jumpButtonDown && !isGrounded)
        {
            jumpBuffered = true;
            jumpBufferTime = Time.time;
        }
        if (jumpButtonUp && !isGrounded && rigidBody.velocity.y > 0)
        {
            print("extra grav");
            rigidBody.velocity = new Vector2(xVelocity, 0);
            jumpCanceled = true;
        }
    }

    // Move based on velocity
    void move()
    {
        // var movement = new Vector3(xVelocity, yVelocity) * Time.deltaTime;
        // transform.position = transform.position + movement;

        var additionalGravity = 0f;

        if (jumpCanceled)
        {
            additionalGravity = -.1f;
        }

        rigidBody.velocity = new Vector2(xVelocity, Mathf.Clamp(rigidBody.velocity.y + additionalGravity, -topSpeed, topSpeed));
    }

    void flip()
    {
        if (xVelocity > 0) {
            facingLeft = true;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (xVelocity < 0)
        {
            facingLeft = false;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
