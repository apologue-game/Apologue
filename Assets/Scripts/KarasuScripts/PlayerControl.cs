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
    float movementSpeedHelper;
    private bool facingRight = true;
    public float inputX;

    //jump
    public float jumpForce = 25f;
    float verticalSpeedAbsolute;
    float verticalSpeed;
    public bool grounded;
    private Transform groundCheck;
    private const float groundCheckRadius = .3f;
    private const float ceilingCheckRadius = .3f;
    public LayerMask whatIsGround;

    //double jump
    public float doubleJumpForce = 20f;
    int jumpCounter = 0;
    public bool doubleJump;

    //wall jump
    public static bool hangingOnTheWall = false;
    public static bool wallJump = false;
    public static float hangingOnTheWallTimer = 2f;

    //falling
    bool falling = false;

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
    enum AttackState
    {
        notAttacking,
        lightAttack,
        lightAttackUpwards,
        mediumAttack,
        heavyAttack
    }
    AttackState attackState;
    bool combo;
    bool comboFinished;
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
    float nextParry = 0;
    public float parryCooldown = 4f;
    //enemy blocking
    public static bool currentlyAttacking = false;
    //Testing

    //Animation manager
    string oldState;

    //animation names
    const string IDLEANIMATION = "karasuIdleAnimation";
    const string WALKANIMATION = "karasuWalkAnimation";
    const string JUMPANIMATION = "karasuJumpAnimation";
    const string DOUBLEJUMPANIMATION = "karasuDoubleJumpAnimation";
    const string WALLJUMPANIMATION = "karasuWallJumpAnimation";
    const string WALLHANGINGANIMATION = "karasuWallHangingAnimation";
    const string FALLINGANIMATION = "karasuFallingAnimation";
    const string CROUCHANIMATION = "karasuCrouchAnimation";
    const string DEATHANIMATION = "karasuDeathAnimation";
    const string DASHANIMATION = "karasuDashAnimation";
    const string PARRYANIMATION = "karasuParryAnimation";
    const string BLOCKANIMATION = "karasuBlockAnimation";
    const string BLOCKEDANDHITANIMATION = "karasuBlockedAndHitAnimation";
    const string LIGHTATTACKANIMATION = "karasuLightAttackAnimation";
    const string LIGHTATTACKUPWARDSANIMATION = "karasuLightAttackUpwardsAnimation";
    const string MEDIUMATTACKANIMATION = "karasuMediumAttackAnimation";
    const string HEAVYATTACKANIMATION = "karasuHeavyAttackAnimation";

    void Awake()
    {
        playerinputActions = new ApologuePlayerInput_Actions();
        playerinputActions.Enable();
        playerInput = GetComponent<PlayerInput>();

        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");

        movementSpeedHelper = movementSpeed;
        spearRange = new Vector3(2.44f, 0.34f, 0);

        attackState = new AttackState();
    }

    void FixedUpdate()
    {
        grounded = false;
        
        //colliders -> check to see if the player is currently on the ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                falling = false;
            }             
        }
        //move character
        rigidBody2D.velocity = new Vector2(inputX * movementSpeed, rigidBody2D.velocity.y);
        verticalSpeedAbsolute = Math.Abs(rigidBody2D.velocity.y);
        verticalSpeed = rigidBody2D.velocity.y;
        if (!blocking && !parryColliderGO.activeSelf && attackState == AttackState.notAttacking)
        {
            if (!grounded && verticalSpeed < -6 && !falling)
            {
                falling = true;
                AnimatorSwitchState(FALLINGANIMATION);
            }
            if (grounded && inputX != 0)
            {
                AnimatorSwitchState(WALKANIMATION);
            }
            else if (grounded && inputX == 0)
            {
                AnimatorSwitchState(IDLEANIMATION);
            }
            //jumping
            else if (!grounded && verticalSpeedAbsolute > 0)
            {
                if (jumpCounter == 2)
                {
                    AnimatorSwitchState(DOUBLEJUMPANIMATION);
                    goto DONE;
                }
                AnimatorSwitchState(JUMPANIMATION);
            }
        DONE:;
        }
        else if (parryColliderGO.activeSelf)
        {
            AnimatorSwitchState(PARRYANIMATION);
        }
        else if (blockColliderGO.activeSelf)
        {
            AnimatorSwitchState(BLOCKANIMATION);
        }
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
            doubleJump = true;
        }
    }


    //Movement
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
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
        if (grounded && callbackContext.performed && !blocking)
        {
            // Add a vertical force to the player.
            rigidBody2D.velocity = Vector2.up * jumpForce;
            jumpCounter = 1;
        }
        if (hangingOnTheWall && wallJump)
        {
            hangingOnTheWall = false;
            wallJump = false;
            rigidBody2D.velocity = Vector2.up * jumpForce;
        }
    }

    public void OnDoubleJump(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
        if (!grounded && callbackContext.performed && jumpCounter == 1 && doubleJump)
        {
            rigidBody2D.velocity = (Vector2.up * doubleJumpForce);
            jumpCounter++;
            doubleJump = false;
        }
    }

    public void OnDash(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
        if (Time.time > timeUntilNextDash)
        {
            if (inputX > 0)
            {
                rigidBody2D.AddForce(Vector2.right * dashSpeed * 350);
                timeUntilNextDash = Time.time + 2;
            }
            else if (inputX < 0)
            {
                rigidBody2D.AddForce(Vector2.left * dashSpeed * 350);
                timeUntilNextDash = Time.time + 2;
            }
            else
            {
                if (dashDirectionIfStationary)
                {
                    rigidBody2D.AddForce(Vector2.right * dashSpeed * 350);
                    timeUntilNextDash = Time.time + 2;
                }
                else if (!dashDirectionIfStationary)
                {
                    rigidBody2D.AddForce(Vector2.left * dashSpeed * 350);
                    timeUntilNextDash = Time.time + 2;
                }
            }
        }
    }

    public void OnCrouch(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
    }

    public void OnSlide(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
    }

    //Combat system
    public void OnLightAttack(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
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
                combo = false;
                AnimatorSwitchState(LIGHTATTACKANIMATION);
                numberOfAttacks = 0;
                attackState = AttackState.lightAttack;
                currentlyAttacking = true;
                nextAttackTime = Time.time + 0.11f + 1f;
                nextGlobalAttack = Time.time + 0.11f + 1.5f / globalAttackCooldown;
                comboTimeWindow = Time.time + 1 + attackSpeed / 2;
                
            }
            else if (Time.time <= comboTimeWindow)
            {
                numberOfAttacks++;
                combo = true;
            }
            if (combo && numberOfAttacks > 1)
            {
                comboFinished = true;
            }
        }
    }

    void LightAttack()
    {
        StartCoroutine(StopMovingWhileAttackingCombos(animator.GetCurrentAnimatorStateInfo(0).length));
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordCollider.position, attackRange, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {   
                currentlyAttacking = false;
                return;
            }
            if (enemy.name == "EnemiesInRange")
            {
                continue;
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
            attackState = AttackState.lightAttackUpwards;
            currentlyAttacking = true;
            AnimatorSwitchState(LIGHTATTACKUPWARDSANIMATION);
        }
    }

    void LightAttackUpwards()
    {
        StartCoroutine(StopMovingWhileAttackingCombos(animator.GetCurrentAnimatorStateInfo(0).length));
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordUppercutCollider.position, attackRangeUppercut, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {
                currentlyAttacking = false;
                return;
            }
            if (enemy.name == "EnemiesInRange")
            {
                continue;
            }
            Debug.Log("We hit " + enemy + " with a sword uppercut");
            enemy.GetComponent<Soldier>().TakeDamage(attackDamage);
        }
        currentlyAttacking = false;
    }

    public void OnMediumAttack(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
        if (callbackContext.control.IsPressed())
        {
            if (Time.time >= nextAttackTimeSpear && Time.time >= nextGlobalAttack)
            {
                AnimatorSwitchState(MEDIUMATTACKANIMATION);
                nextAttackTimeSpear = Time.time + 1f / attackSpeedSpear;
                nextGlobalAttack = Time.time + 1.5f / globalAttackCooldown;
                attackState = AttackState.mediumAttack;
                currentlyAttacking = true;
            }
        }
    }

    public void MediumAttack()
    {
        StartCoroutine(StopMovingWhileAttacking(animator.GetCurrentAnimatorStateInfo(0).length));
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(spearCollider.position, spearRange, 0, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {
                currentlyAttacking = false;
                return;
            }
            if (enemy.name == "EnemiesInRange")
            {
                continue;
            }
            Debug.Log("We hit " + enemy + " with a spaer");
            enemy.GetComponent<Soldier>().TakeDamage(attackDamageSpear);
        }
        currentlyAttacking = false;
    }

    public void OnHeavyAttack(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
        if (callbackContext.control.IsPressed())
        {
            if (Time.time >= nextAttackTimeAxe && Time.time >= nextGlobalAttack)
            {
                AnimatorSwitchState(HEAVYATTACKANIMATION);
                nextAttackTimeAxe = Time.time + 1f / attackSpeedAxe;
                nextGlobalAttack = Time.time + 1.5f / globalAttackCooldown;
                attackState = AttackState.heavyAttack;
                currentlyAttacking = true;
            }
        }
    }

    void HeavyAttack()
    {
        StartCoroutine(StopMovingWhileAttacking(animator.GetCurrentAnimatorStateInfo(0).length));
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(axeCollider.position, attackRangeAxe, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {
                currentlyAttacking = false;
                return;
            }
            if (enemy.name == "EnemiesInRange")
            {
                continue;
            }
            Debug.Log("We hit " + enemy + " with an axe");
            enemy.GetComponent<Soldier>().TakeDamage(attackDamageAxe);
        }
        currentlyAttacking = false;
    }

    public void OnParry(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
        if (callbackContext.performed)
        {
            if (Time.time > nextParry)
            {
                StartCoroutine(ParryWindow());
                nextParry = Time.time + 1f / parryCooldown;
            }
        }  
    }

    public void OnBlock(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
        if (callbackContext.performed && !blocking)
        {
            blocking = true;
            blockColliderGO.SetActive(true);
            movementSpeed = 0;
        }
        else if (callbackContext.canceled)
        {
            blocking = false;
            blockColliderGO.SetActive(false);
            movementSpeed = movementSpeedHelper;
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

    private void Flip()
    {
        // Switch the way the player is labeled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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

    IEnumerator StopMovingWhileAttackingCombos(float waitingDuration)
    {
        if (grounded)
        {
            movementSpeed = 0;
            yield return new WaitForSeconds(waitingDuration - 0.05f);
            movementSpeed = movementSpeedHelper;
            if (!combo)
            {
                attackState = AttackState.notAttacking;
            }
            if (combo && attackState == AttackState.lightAttack && comboFinished)
            {
                attackState = AttackState.notAttacking;
            }
            else if (combo)
            {
                StartCoroutine(ComboWindow());
            }
        }
    }

    IEnumerator StopMovingWhileAttacking(float waitingDuration)
    {
        if (grounded)
        {
            movementSpeed = 0;
            yield return new WaitForSeconds(waitingDuration - 0.05f);
            movementSpeed = movementSpeedHelper;
            attackState = AttackState.notAttacking;
        }
    }

    IEnumerator ComboWindow()
    {
        yield return new WaitForSeconds(0.4f);
        combo = false;
        comboFinished = false;
        attackState = AttackState.notAttacking;
    }

    private void OnDrawGizmosSelected()
    {
        if (axeCollider == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(axeCollider.position, attackRangeAxe);
    }

    //Animation manager
    public void AnimatorSwitchState(string newState)
    {
        if (oldState == newState)
        {
            return;
        }

        animator.Play(newState);

        oldState = newState;
    }
}
