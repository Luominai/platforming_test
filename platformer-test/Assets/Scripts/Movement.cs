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

    // Current velocity
    private float xVelocity;
    private float yVelocity;

    // Get when the jump button (space) is pressed and released
    private bool jumpButtonDown;
    private bool jumpButtonUp;

    // Gets horizontal and vertical inputs (-1, 0, or 1)
    // W: Positive Y value
    // A: Negative X value
    // S: Negative Y value
    // D: Positive X value
    private float xInput;
    private float yInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
        updateSpeed();
        move();
    }

    // Collecting input
    void getInput()
    {
        jumpButtonDown = Input.GetButtonDown("Jump");
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

    void collisionChecks()
    {

    }

    // Move based on velocity
    void move()
    {
        var movement = new Vector3(xVelocity, yVelocity) * Time.deltaTime;
        transform.position = transform.position + movement;
    }
}
