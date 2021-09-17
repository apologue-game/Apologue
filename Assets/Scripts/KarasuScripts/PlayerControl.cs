using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public static ApologuePlayerInput_Actions playerinputActions;
    static PlayerInput playerInput;
    private Rigidbody2D rigidBody2D;
    private Animator animator;

    //movement system
    //move
    public float movementSpeed = 8f;
    private bool facingRight = true;
    public float inputX;

    //jump
    public float jumpForce = 25f;
    public bool grounded;
    private Transform groundCheck;
    private const float groundCheckRadius = .1f;
    private const float ceilingCheckRadius = .1f;
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
    public float globalAttackCooldown = 5f;
    float nextGlobalAttack = 0f;

    //light attack
    public Transform swordCollider;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public float attackSpeed = 0.75f;
    float nextAttackTime = 0f;
    //light attack combos
    public Transform swordUppercutCollider;
    public float attackRangeUppercut = 0.5f;
    float comboTimeWindow = 0.2f;
    public int numberOfAttacks = 0;
    public bool firstAttack = false;

    //medium attack
    public Transform spearCollider;
    public Vector3 spearRange;
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

    //parry and block
    public Transform parryCollider;
    public GameObject parryColliderGO;
    public GameObject blockColliderGO;
    public float parryBlockRange = 0.5f;
    public bool blocking = false;
    public float parryWindow = 0.4f;
    public float parryWindowActual = 0;
    float nextParry = 0;
    public float parryCooldown = 6;
    //enemy blocking
    public static bool currentlyAttacking = false;
    //Testing

    void Awake()
    {
        playerinputActions = new ApologuePlayerInput_Actions();
        playerinputActions.Enable();
        playerInput = GetComponent<PlayerInput>();

        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //playerPosition = transform.Find("PlayerKarasu");
        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");

        dashDirectionIfStationary = true;
        InputSystem.EnableDevice(Keyboard.current);
        spearRange = new Vector3(2.44f, 0.34f, 0);
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
                currentlyAttacking = true;
                nextAttackTime = Time.time + 0.11f + 1f / attackSpeed;
                nextGlobalAttack = Time.time + 0.11f + 1f / globalAttackCooldown;
                comboTimeWindow = Time.time + 1 + attackSpeed / 2;
                
            }
            else if (Time.time <= comboTimeWindow)
            {
                numberOfAttacks++;
            }
        }
    }

    void LightAttack()
    {
        StartCoroutine("StopMovingWhileAttacking");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordCollider.position, attackRange, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {
                currentlyAttacking = false;
                return;
            }
            Debug.Log("We hit " + enemy + " with a sword");
            enemy.GetComponent<Soldier>().TakeDamage(attackDamage);
        }
        currentlyAttacking = false;
    }

    void LightAttackUpwardsAnimation()
    {
        if (numberOfAttacks == 1)
        {
            numberOfAttacks++;
            animator.SetTrigger("animLightAttackUpwards");
            currentlyAttacking = true;
        }
    }

    void LightAttackUpwards()
    {
        StartCoroutine("StopMovingWhileAttacking");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordUppercutCollider.position, attackRangeUppercut, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {
                currentlyAttacking = false;
                return;
            }
            Debug.Log("We hit " + enemy + " with a sword uppercut");
            enemy.GetComponent<Soldier>().TakeDamage(attackDamage);
        }
        currentlyAttacking = false;
    }

    public void OnMediumAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.control.IsPressed())
        {
            if (Time.time >= nextAttackTimeSpear && Time.time >= nextGlobalAttack)
            {
                nextAttackTimeSpear = Time.time + 1f / attackSpeedSpear;
                nextGlobalAttack = Time.time + 1f / globalAttackCooldown;
                animator.SetTrigger("animMediumAttack");
                currentlyAttacking = true;
            }
        }
    }

    public void MediumAttack()
    {
        StartCoroutine("StopMovingWhileAttacking");
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(spearCollider.position, spearRange, 0, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {
                currentlyAttacking = false;
                return;
            }
            Debug.Log("We hit " + enemy + " with a spaer");
            enemy.GetComponent<Soldier>().TakeDamage(attackDamageSpear);
        }
        currentlyAttacking = false;
    }

    public void OnHeavyAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.control.IsPressed())
        {
            if (Time.time >= nextAttackTimeAxe && Time.time >= nextGlobalAttack)
            {
                nextAttackTimeAxe = Time.time + 1f / attackSpeedAxe;
                nextGlobalAttack = Time.time + 1f / globalAttackCooldown;
                animator.SetTrigger("animHeavyAttack");
                currentlyAttacking = true;
            }
        }
    }

    void HeavyAttack()
    {
        StartCoroutine("StopMovingWhileAttacking");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(axeCollider.position, attackRangeAxe, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {
                currentlyAttacking = false;
                return;
            }
            Debug.Log("We hit " + enemy + " with an axe");
            enemy.GetComponent<Soldier>().TakeDamage(attackDamageAxe);
        }
        currentlyAttacking = false;
    }

    public void OnParry(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (Time.time > nextParry)
            {
                animator.SetTrigger("animParry");
                StartCoroutine("ParryWindow");
                nextParry = Time.time + 1f / parryCooldown;
            }
        }
        
    }

    public void OnBlock(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            animator.SetBool("animBlock", true);
            blockColliderGO.SetActive(true);
            movementSpeed = 0;
        }
        else if (callbackContext.canceled)
        {
            animator.SetBool("animBlock", false);
            blockColliderGO.SetActive(false);
            movementSpeed = 8;
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

    public static void TurnOffControlsOnDeath()
    {
        playerInput.currentActionMap.Disable();
    }

    public static void TurnOnControlsOnRespawn()
    {
        playerInput.currentActionMap.Enable();

    }

    IEnumerator WaitForAnimationToFinish(float animationDuration)
    {
        yield return new WaitForSeconds(animationDuration);
    }

    IEnumerator ParryWindow()
    {
        parryColliderGO.SetActive(true);
        yield return new WaitForSeconds(parryWindow);
        parryColliderGO.SetActive(false);
    }

    IEnumerator StopMovingWhileAttacking()
    {
        if (grounded)
        {
            movementSpeed = 0;
            yield return new WaitForSeconds(0.43f);
            movementSpeed = 8;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (axeCollider == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(axeCollider.position, attackRangeAxe);
    }

}
