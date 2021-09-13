using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    ApologuePlayerInput_Actions playerinputActions;
    private Rigidbody2D rigidBody2D;
    private Animator animator;

    //movement system
    //move
    public float movementSpeed = 2500f;
    private bool facingRight = true;
    public float inputX;

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

    //falling

    //dash
    public float dashSpeed = 5f;
    public float timeUntilNextDash;
    public bool dashDirectionIfStationary = true;

    //crouch
    public bool crouch;

    //slide
    public float slideSpeed;
    private Transform ceilingCheck;

    //combat system
    public LayerMask enemiesLayers;
    float globalAttackCooldown = 5f;
    float nextGlobalAttack = 0f;
    //light attack
    public Transform swordCollider;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public float attackSpeed = 0.75f;
    float nextAttackTime = 0f;
    //light attack combos
    float comboTimeWindow = 0.2f;
    public int numberOfAttacks = 0;
    public bool firstAttack = false;

    //medium attack
    public Transform spearCollider;
    public float attackRangeSpear = 0.5f;
    public int attackDamageSpear = 2;
    public float attackSpeedSpear = 0.2f;
    float nextAttackTimeSpear = 0f;

    //heavy attack
    public Transform axeCollider;
    public float attackRangeAxe = 0.5f;
    public int attackDamageAxe = 2;
    public float attackSpeedAxe = 0.2f;
    float nextAttackTimeAxe = 0f;

    //Testing
    public float animationLength;
    int counter;

    void Awake()
    {
        playerinputActions = new ApologuePlayerInput_Actions();
        playerinputActions.Enable();

        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //playerPosition = transform.Find("PlayerKarasu");
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
        animator.SetBool("animGrounded", grounded);
        //falling animation
        animator.SetFloat("animvSpeedFalling", rigidBody2D.velocity.y);
        //dew();
    }

    void Update()
    {      
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


    //Movement
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        inputX = callbackContext.ReadValue<Vector2>().x;
        
        if (inputX > 0 && !facingRight)
        {
            Flip();
        }
        else if (inputX < 0 && facingRight)
        {
            Flip();
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

    public void OnDoubleJump(InputAction.CallbackContext callbackContext)
    {
        if (!grounded && callbackContext.performed && doubleJump)
        {
            animator.SetBool("animDoubleJump", true);
            rigidBody2D.velocity = (Vector2.up * doubleJumpForce);
            doubleJump = false;
        }
    }

    public void OnDash(InputAction.CallbackContext callbackContext)
    {
        if (Time.time > timeUntilNextDash)
        {
            if (inputX > 0)
            {
                rigidBody2D.AddForce(Vector2.right * dashSpeed * 350);
                //animator.SetTrigger("animDash");
                timeUntilNextDash = Time.time + 2;
            }
            else if (inputX < 0)
            {
                rigidBody2D.AddForce(Vector2.left * dashSpeed * 350);
                //animator.SetTrigger("animDash");
                timeUntilNextDash = Time.time + 2;
            }
            else
            {
                if (dashDirectionIfStationary)
                {
                    rigidBody2D.AddForce(Vector2.right * dashSpeed * 350);
                    //animator.SetTrigger("animDash");
                    timeUntilNextDash = Time.time + 2;
                }
                else if (!dashDirectionIfStationary)
                {
                    rigidBody2D.AddForce(Vector2.left * dashSpeed * 350);
                    //animator.SetTrigger("animDash");
                    timeUntilNextDash = Time.time + 2;
                }
            }
        }
    }

    public void OnCrouch(InputAction.CallbackContext callbackContext)
    {

    }

    public void OnSlide(InputAction.CallbackContext callbackContext)
    {

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

    //Combat system
    public void OnLightAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.control.IsPressed())
        {
            if (Gamepad.all.Count == 0)
            {

            }
            else if (Gamepad.current.leftShoulder.isPressed)
            {
                return;
            }
            if (Time.time >= nextAttackTime && Time.time >= nextGlobalAttack)
            {
                numberOfAttacks = 0;
                animator.SetTrigger("animLightAttack");

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordCollider.position, attackRange, enemiesLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    Debug.Log("We hit " + enemy + " with a sword");
                    enemy.GetComponent<Soldier>().TakeDamage(attackDamage);
                }
                nextAttackTime = Time.time + 1f / attackSpeed;
                nextGlobalAttack = Time.time + 1f / globalAttackCooldown;
                comboTimeWindow = Time.time + attackSpeed / 2;
                
            }
            else if (Time.time <= comboTimeWindow)
            {
                numberOfAttacks++;
            }
        }
    }

    void LightAttackUpwards()
    {
        if (numberOfAttacks == 1)
        {
            animator.SetTrigger("animLightAttackUpwards");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordCollider.position, attackRange, enemiesLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy + " with a sword uppercut");
                enemy.GetComponent<Soldier>().TakeDamage(attackDamage);
            }
            numberOfAttacks++;
            firstAttack = false;
        }
    }

    public void OnMediumAttack(InputAction.CallbackContext callbackContext)
    {
        if (Time.time >= nextAttackTimeSpear && Time.time >= nextGlobalAttack)
        {
            animator.SetTrigger("animMediumAttack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(spearCollider.position, attackRangeSpear, enemiesLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy + " with a spaer");
                enemy.GetComponent<Soldier>().TakeDamage(attackDamageSpear);
            }
            nextAttackTimeSpear = Time.time + 1f / attackSpeedSpear;
            nextGlobalAttack = Time.time + 1f / globalAttackCooldown;
        }

    }

    public void OnHeavyAttack(InputAction.CallbackContext callbackContext)
    {
        if (Time.time >= nextAttackTimeAxe && Time.time >= nextGlobalAttack)
        {
            animator.SetTrigger("animHeavyAttack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(axeCollider.position, attackRangeAxe, enemiesLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy + " with an axe");
                enemy.GetComponent<Soldier>().TakeDamage(attackDamageAxe);
            }
            nextAttackTimeAxe = Time.time + 1f / attackSpeedAxe;
            nextGlobalAttack = Time.time + 1f / globalAttackCooldown;
        }
    }

    //Utilities
    private void OnEnable()
    {
        playerinputActions.Enable();
    }

    private void OnDisable()
    {
        playerinputActions.Disable();
    }

    IEnumerator WaitForAnimationToFinish(float animationDuration)
    {
        yield return new WaitForSeconds(animationDuration);
    }

    private void OnDrawGizmosSelected()
    {
        if (spearCollider == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(spearCollider.position, attackRangeSpear);
    }

}
