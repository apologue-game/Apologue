using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    ApologuePlayerInput_Actions inputActions;
    public Rigidbody2D rigidBody2D;

    //movement
    private float direction = 0;
    public float movementSpeed = 2500f;
    private bool facingRight = true;

    //jump
    public float jumpForce = 25f;
    public float doubleJumpForce = 15f;
    public bool doubleJump = true;
    public bool grounded;
    private Transform groundCheck;
    private const float groundCheckRadius = .01f;
    private const float ceilingCheckRadius = .01f;
    public LayerMask whatIsGround;
    //public Vector2 doubleJumpForceVector;

    //dash
    public float dashSpeed = 15000f;
    public enum DashState
    {
        Ready,
        NotReady
    }
    private DashState dashState;
    private bool dashReady = true;
    public float dashCooldown = 5f;
    public float timePassedSinceDash;
    private Vector2 dashSpeedVector;

    //slide
    public float slideSpeed;
    private Transform ceilingCheck;


    private void Awake()
    {
        inputActions = new ApologuePlayerInput_Actions();
        inputActions.Enable();
        rigidBody2D = GetComponent<Rigidbody2D>();
        dashSpeedVector = new Vector2(rigidBody2D.velocity.x * dashSpeed, rigidBody2D.velocity.y);
        //doubleJumpForceVector = new Vector2(0, rigidBody2D.velocity.x * jumpForce / 2);


        inputActions.Player.Move.performed += ctx => { direction = ctx.ReadValue<float>(); };

        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
    }

    private void FixedUpdate()
    {
        grounded = false;


        //colliders check to see if the player is currently on the ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                doubleJump = true;
            }             
        }
    }

    private void Update()
    {
        if (!dashReady)
        {
            timePassedSinceDash += Time.deltaTime * 3f;
        }
        Move();
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            DoubleJump();
        }
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            Dash();
        }
    }

    private void DoubleJump()
    {
        if (grounded == false && doubleJump)
        {
            rigidBody2D.velocity = (Vector2.up * jumpForce);
            //rigidBody2D.AddForce(Vector2.up * doubleJumpForce);
            doubleJump = false;
        }
    }

    private void Move()
    {
        if(direction > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction < 0 && facingRight)
        {
            Flip();
        }
        rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.deltaTime, rigidBody2D.velocity.y);
    }

    private void Dash()
    {
        Debug.Log("We're in the Dash function!");
        switch (dashState)
        {
            case DashState.Ready:
                {
                    Debug.Log("We're in the 'Dash is ready' case");
                    rigidBody2D.AddForce(dashSpeedVector);
                    dashState = DashState.NotReady;
                    dashReady = false;
                    break;
                }
            case DashState.NotReady:
                {
                    Debug.Log("We're in the 'Dash is on cooldown' case");
                    if (timePassedSinceDash > dashCooldown)
                    {
                        Debug.Log("We're in setting dash to a ready state");
                        timePassedSinceDash = 0;
                        dashReady = true;
                        dashState = DashState.Ready;
                    }
                    
                    break;
                }
        }
    }

    private void Jump()
    {
        
        if (grounded)
        {
            
            grounded = false;
            // Add a vertical force to the player.
            rigidBody2D.velocity = Vector2.up * jumpForce;

        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
