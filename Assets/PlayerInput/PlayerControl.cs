using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    ApologuePlayerInput_Actions inputActions;
    public Rigidbody2D rigidBody2D;
    public Animator animator;

    //movement
    public float movementSpeed = 2500f;
    private bool facingRight = true;
    float inputX;

    //jump
    public float jumpForce = 25f;
    public bool grounded;
    private Transform groundCheck;
    private const float groundCheckRadius = .01f;
    private const float ceilingCheckRadius = .01f;
    public LayerMask whatIsGround;

    //double jump
    public float doubleJumpForce = 20f;
    public bool doubleJump;

    //dash
    public float dashSpeed = 20000f;
    private bool dashReady = true;
    public float dashCooldown = 5f;
    public float timePassedSinceDash;
    public bool dashDirectionIfStationary = true;

    //slide
    public float slideSpeed;
    private Transform ceilingCheck;

    void Awake()
    {
        inputActions = new ApologuePlayerInput_Actions();
        inputActions.Enable();

        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");

        dashDirectionIfStationary = true;
    }

    void FixedUpdate()
    {
        grounded = false;
        
        //colliders check to see if the player is currently on the ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }             
        }
        //move character
        rigidBody2D.velocity = new Vector2(inputX * movementSpeed, rigidBody2D.velocity.y);

        //move animation
        animator.SetFloat("animSpeed", Mathf.Abs(rigidBody2D.velocity.x));
        //jump animation
        animator.SetFloat("animvSpeed", Mathf.Abs(rigidBody2D.velocity.y));
    }

    void Update()
    {      
        if (!dashReady)
        {
            timePassedSinceDash += Time.deltaTime * 3f;
            if (timePassedSinceDash >= dashCooldown)
            {
                dashReady = true;
                timePassedSinceDash = 0;
            }
        }
        if (inputX > 0)
        {
            dashDirectionIfStationary = true;
        }
        else if (inputX < 0)
        {
            dashDirectionIfStationary = false;
        }
        if (grounded)
        {
            animator.SetBool("animDoubleJump", false);
            doubleJump = true;
        }
    }

    public void OnDoubleJump(InputAction.CallbackContext callbackContext)
    {
        if (!grounded && callbackContext.performed && doubleJump)
        {
            animator.SetBool("animDoubleJump", true);
            rigidBody2D.velocity = (Vector2.up * doubleJumpForce);
            doubleJump = false;
        }    
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (grounded && callbackContext.performed)
        {
            // Add a vertical force to the player.
            rigidBody2D.velocity = Vector2.up * jumpForce;
        }
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        inputX = callbackContext.ReadValue<Vector2>().x;
        if(inputX > 0 && !facingRight)
        {
            Flip();
        }
        else if (inputX < 0 && facingRight)
        {
            Flip();
        }
    }

    public void OnDash(InputAction.CallbackContext callbackContext)
    {
        if (dashReady)
        {
            if (inputX > 0)
            {
                rigidBody2D.AddForce(Vector2.right * dashSpeed);
                dashReady = false;
            }
            else if (inputX < 0)
            {
                rigidBody2D.AddForce(Vector2.left * dashSpeed);
                dashReady = false;
            }
            else
            {
                if (dashDirectionIfStationary)
                {
                    rigidBody2D.AddForce(Vector2.right * dashSpeed);
                    dashReady = false;
                }
                else if (!dashDirectionIfStationary)
                {
                    rigidBody2D.AddForce(Vector2.left * dashSpeed);
                    dashReady = false;
                }
            }
        }
    }

    private void Flip()
    {
        // Switch the way the player is labeled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
