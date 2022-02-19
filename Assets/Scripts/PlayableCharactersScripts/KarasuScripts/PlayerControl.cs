using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //Self references
    public static ApologuePlayerInput_Actions playerinputActions; 
    public static PlayerInput playerInput;
    Rigidbody2D rigidBody2D;
    Animator animator;

    //Particle and audio system
    public ParticleSystem movementAndJumpDust;
    public ParticleSystem dashParticleEffect;

    //Interactable objects -> icon above the head //Should be exported into a separate class
    GameObject interactionTooltip;
    SpriteRenderer interactionTooltipSR;
    Vector2 interactionSize = new Vector2(0.1f, 1); //Interactable object's domain in which the interact button will be shown

    //Pause menu
    public GameObject interactionIconPrefab;
    public Sprite keyboardInteractionIcon;
    public Sprite gamepadInteractionIcon;

    //Movement system
    //Move
    public float movementSpeed = 5.8f;
    float movementSpeedHelper; //No need for it if the movement doesn't stop while attacking
    public bool facingRight = true;
    float inputX;

    //Jump
    public float jumpForce = 11f;
    float verticalSpeedAbsolute;
    float verticalSpeed;
    public bool grounded;
    private const float ceilingCheckRadius = .3f;
    public LayerMask whatIsGround;
    int jumpHoldCounter = 0;
    bool holdingJump = false;

    //Double jump
    public float doubleJumpForce = 10.5f;
    int jumpCounter = 0;
    bool doubleJump;

    //Wall jump
    public Transform wallHangingCollider;
    Coroutine coroutineWallHanging = null;
    float wallHangingColliderRange = 0f;
    public static bool hangingOnTheWall = false;
    public static bool wallJump = false;
    public static float hangingOnTheWallTimer = 1f;
    float wallJumpAdditionalForce = 50f;
    int wallJumpPushBackCounter = 0;
    bool initiatePushBackCounter = false;

    //Falling
    bool falling = false;

    //Dash
    public float dashSpeed = 3f;
    float timeUntilNextDash;
    bool dashDirectionIfStationary = true;
    bool isDashing = false;
    bool dashInAirAvailable = true;

    //Crouch
    public LayerMask whatIsCeiling;
    Transform ceilingCheck;
    public BoxCollider2D regularCollider;
    public BoxCollider2D slideCollider;
    bool isCrouching = false;
    bool canStandUp;
    public float crouchSpeedMultiplier = 0.5f;

    //Slide
    bool isSliding = false;
    float slideDirection;
    public float slideSpeed = 15f;

    //Slopes
    public bool onASlope = false;
    bool jumpAvailable = true;
    public float slopeXPosition = 0f;

    //Combat system
    public LayerMask enemiesLayers;
    public enum AttackState
    {
        notAttacking,
        cannotAttack,
        lightAttackSword1,
        lightAttackSword2,
        lightAttackSword3,
        mediumAttackSword1,
        mediumAttackSword2,
        heavyAttackSword1,
        heavyAttackSword2,
        lightAttackAxe1,
        lightAttackAxe2,
        lightAttackAxe3,
        mediumAttackAxe1,
        mediumAttackAxe2,
        heavyAttackAxe1,
        heavyAttackAxe2
    }
    public AttackState attackState;

    //Light attack
    public Transform swordCollider;
    public float attackRange = 0.56f;
    public int attackDamage = 1;
    public float attackSpeed = 0.75f;
    float nextAttackTime = 0f;
    //Light attack combos

    //Medium attack
    public Transform spearCollider;
    public Vector3 spearRange;
    int attackDamageSpear = 2;
    public float attackSpeedSpear = 0.2f;
    float nextAttackTimeSpear = 0f;

    //Heavy attack
    public static bool axePickedUp = false; //Heavy attack only usable after finding the axe

    //Parry and block
    public Transform parryCollider;
    public GameObject parryColliderGO;
    float parryWindow = 0.4f;
    float nextParry = 0;
    float parryCooldown = 4f;
    //Enemy blocking
    public bool currentlyAttacking = false; //Needed for the soldier enemy script blocking interaction
    bool blockedOrParried = false;

    //Utilities
    //Pause menu
    public GameObject pauseMenuPanel;
    public GameObject tooltipPanel;
    public static bool isGamePaused = false;

    //Animation manager
    string oldAnimationState;

    //Animation states
    public enum AnimationState
    {
        idle,
        walk,
        jump,
        doubleJump,
        wallJump,
        wallHanging,
        falling,
        crouchMove,
        crouchIdle,
        slide,
        crouchRoll,
        death,
        spikeDeath,
        dash,
        parry,
        block,
        hitWhileBlocking,
        lightAttack,
        lightAttackUpwards,
        mediumAttack,
        heavyAttack
    }
    public AnimationState animationState;

    void Awake()
    {
        playerinputActions = new ApologuePlayerInput_Actions();
        playerinputActions.Enable();
        playerInput = GetComponent<PlayerInput>();

        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ceilingCheck = transform.Find("CeilingCheck");

        movementSpeedHelper = movementSpeed;
        spearRange = new Vector3(2.44f, 0.34f, 0);

        attackState = new AttackState();
        animationState = AnimationState.idle;

        interactionTooltip = transform.Find("InteractionTooltip").gameObject;
        interactionTooltipSR = interactionTooltip.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        AnimatorSwitchState(animationState);
        if (KarasuEntity.dead)
        {
            if (!KarasuEntity.spikesDeath)
            {
                animationState = AnimationState.death;
            }
            rigidBody2D.velocity = Vector2.zero;
            return;
        }
        if (isDashing)
        {
            return;
        }
        if (onASlope)
        {
            rigidBody2D.velocity = Vector2.ClampMagnitude(rigidBody2D.velocity, 20);
            animationState = AnimationState.slide;
            return;
        }
        if (isSliding)
        {
            if (slideDirection == inputX)
            {
                animationState = AnimationState.slide;
                return;
            }
            else
            {
                isSliding = false;
            }
        }

        //grounded = false;
        ////Colliders->check to see if the player is currently on the ground
        //Collider2D[] collidersGround = Physics2D.OverlapBoxAll(groundCheck.position, groundCheckRange, whatIsGround);
        //for (int i = 0; i < collidersGround.Length; i++)
        //{
        //    if (collidersGround[i].name == "PlatformsTilemap" || collidersGround[i].name == "GroundTilemap" || collidersGround[i].CompareTag("Box") || collidersGround[i].CompareTag("FallingPlatforms"))
        //    {
        //        grounded = true;
        //        falling = false;
        //        dashInAirAvailable = true;
        //    }
        //}

        //Might be a better way to check whether the player is on the ground
        if (rigidBody2D.IsTouchingLayers(whatIsGround))
        {
            grounded = true;
            dashInAirAvailable = true;
            falling = false;
        }
        else
        {
            grounded = false;
        }
        

        //Check to see if there is a ceiling above the player and whether they can get up from the crouching state
        Collider2D[] collidersCeiling = Physics2D.OverlapCircleAll(ceilingCheck.position, ceilingCheckRadius, whatIsCeiling);
        if (collidersCeiling.Length < 1)
        {
            canStandUp = true;
        }
        else
        {
            canStandUp = false;
        }

        if (!isCrouching)
        {
            regularCollider.enabled = true;
            slideCollider.enabled = false;
        }
        else
        {
            regularCollider.enabled = false;
            slideCollider.enabled = true;
        }

        //Move character
        if (!isCrouching)
        {
            rigidBody2D.velocity = new Vector2(inputX * movementSpeed, rigidBody2D.velocity.y);
        }
        else
        {
            rigidBody2D.velocity = new Vector2(inputX * movementSpeed * crouchSpeedMultiplier, rigidBody2D.velocity.y);
        }
        
        verticalSpeedAbsolute = Math.Abs(rigidBody2D.velocity.y);
        verticalSpeed = rigidBody2D.velocity.y;

        //Animations
        if (!parryColliderGO.activeSelf && !hangingOnTheWall)
        {
            if (attackState == AttackState.notAttacking || attackState == AttackState.cannotAttack)
            {
                //Falling
                if (!grounded && verticalSpeed < -12 && !falling)
                {
                    falling = true;
                    animationState = AnimationState.falling;
                }
                //Walking and idle
                if (grounded && inputX != 0 && !isCrouching)
                {
                    animationState = AnimationState.walk;
                }
                else if (grounded && inputX == 0 && !isCrouching)
                {
                    animationState = AnimationState.idle;
                }
                else if (grounded && inputX != 0 && isCrouching)
                {
                    animationState = AnimationState.crouchMove;
                }
                else if (grounded && inputX == 0 && isCrouching)
                {
                    animationState = AnimationState.crouchIdle;
                }
                //Jumping
                else if (!grounded && verticalSpeedAbsolute > 0)
                {
                    if (jumpCounter == 2)
                    {
                        animationState = AnimationState.doubleJump;
                    }
                    else
                    {
                        animationState = AnimationState.jump;
                    }
                }
            }
        }
        else if (hangingOnTheWall)
        {
            animationState = AnimationState.wallHanging;
        }
        else if (parryColliderGO.activeSelf)
        {
            animationState = AnimationState.parry;
        }
        if (holdingJump)
        {
            rigidBody2D.AddForce(Vector2.up * jumpForce * 2);
        }
        if (verticalSpeed < -1)
        {
            holdingJump = false;
            rigidBody2D.AddForce(Vector2.down * 25);
        }
        if (initiatePushBackCounter)
        {
            wallJumpPushBackCounter++;
            if (facingRight)
            {
                rigidBody2D.AddForce(Vector2.left * wallJumpAdditionalForce);
            }
            else
            {
                rigidBody2D.AddForce(Vector2.right * wallJumpAdditionalForce);
            }
            if (wallJumpPushBackCounter > 10)
            {
                initiatePushBackCounter = false;
                wallJumpPushBackCounter = 0;
            }
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
        if (transform.position.x > slopeXPosition + 1)
        {
            jumpAvailable = true;
        }
        if (grounded)
        {
            if (attackState == AttackState.cannotAttack)
            {
                attackState = AttackState.notAttacking;
            }
        }
        if (inputX > 0 && !facingRight && !onASlope && !hangingOnTheWall)
        {
            Flip();
            interactionTooltipSR.flipX = false;
        }
        else if (inputX < 0 && facingRight && !onASlope && !hangingOnTheWall)
        {
            Flip();
            interactionTooltipSR.flipX = true;
        }
    }

    //Movement
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        inputX = callbackContext.ReadValue<Vector2>().x;
        if (hangingOnTheWall || onASlope)
        {
            return;
        }
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        jumpHoldCounter++;
        if (jumpHoldCounter == 2)
        {
            holdingJump = true;
        }
        if (jumpHoldCounter == 3)
        {
            holdingJump = false;
            jumpHoldCounter = 0;
        }
        if (onASlope && callbackContext.performed && jumpAvailable)
        {
            jumpAvailable = false;
            rigidBody2D.velocity = new Vector2(1 * jumpForce/2, 1 * jumpForce);
            return;
        }
        if (grounded && callbackContext.performed && attackState == AttackState.notAttacking && !onASlope)
        {
            rigidBody2D.velocity = Vector2.up * jumpForce;
            isCrouching = false;
            isSliding = false;
            jumpCounter = 1;
            CreateDust();
        }
        if (hangingOnTheWall && wallJump && callbackContext.performed)
        {
            hangingOnTheWall = false;
            wallJump = false;
            rigidBody2D.velocity = Vector2.up * jumpForce;
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            attackState = AttackState.cannotAttack;
            initiatePushBackCounter = true;
        }
    }

    public void OnDoubleJump(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || onASlope)
        {
            return;
        }
        if (!grounded && callbackContext.performed && doubleJump)
        {
            rigidBody2D.velocity = (Vector2.up * doubleJumpForce);
            jumpCounter++;
            doubleJump = false;
        }
    }

    public void OnDash(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || onASlope)
        {
            return;
        }
        if (callbackContext.performed && Time.time > timeUntilNextDash && !isCrouching)
        {
            if (grounded || !grounded && dashInAirAvailable)
            {
                if (!grounded)
                {
                    dashInAirAvailable = false;
                }
                if (inputX > 0)
                {
                    rigidBody2D.velocity = new Vector2(inputX * movementSpeed * dashSpeed, rigidBody2D.velocity.y);
                    timeUntilNextDash = Time.time + 1;
                }
                else if (inputX < 0)
                {
                    rigidBody2D.velocity = new Vector2(inputX * movementSpeed * dashSpeed, rigidBody2D.velocity.y);
                    timeUntilNextDash = Time.time + 1;
                }
                else
                {
                    if (dashDirectionIfStationary)
                    {
                        rigidBody2D.velocity = new Vector2(movementSpeed * dashSpeed, rigidBody2D.velocity.y);
                        timeUntilNextDash = Time.time + 1;
                    }
                    else if (!dashDirectionIfStationary)
                    {
                        rigidBody2D.velocity = new Vector2(-1 * movementSpeed * dashSpeed, rigidBody2D.velocity.y);
                        timeUntilNextDash = Time.time + 1;
                    }
                }
                CreateDashParticleEffect();
                StartCoroutine(IsDashing());
            }
        }
    }

    public void OnCrouch(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || onASlope)
        {
            return;
        }
        if (playerInput.currentControlScheme == "Gamepad")
        {
            if (callbackContext.ReadValue<float>() != 1)
            {
                return;
            }
        }
        if (callbackContext.performed && inputX == 0 && grounded && !isCrouching)
        {
            isCrouching = true;
        }
        else if (callbackContext.performed && isCrouching && !isSliding)
        {
            if (canStandUp)
            {
                isCrouching = false;
            }
        }
    }

    public void OnSlide(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || onASlope)
        {
            return;
        }
        if (playerInput.currentControlScheme == "Gamepad")
        {
            if (callbackContext.ReadValue<float>() != 1)
            {
                return;
            }
        }
        if (callbackContext.performed && inputX != 0 && grounded && !isCrouching)
        {
            isCrouching = true;
            slideDirection = inputX;
            rigidBody2D.velocity = new Vector2(inputX * movementSpeed * 2f, rigidBody2D.velocity.y);
            regularCollider.enabled = false;
            slideCollider.enabled = true;
            //audioManager.PlaySound(SLIDESOUND);
            StartCoroutine(IsSliding());
        }
    }

    IEnumerator IsSliding()
    {
        isSliding = true;
        yield return new WaitForSeconds(0.35f);
        isSliding = false;
    }

    IEnumerator IsDashing()
    {
        isDashing = true;
        yield return new WaitForSeconds(0.10f);
        isDashing = false;
    }

    //Combat system
    public void OnLightAttack(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || attackState == AttackState.cannotAttack || onASlope)
        {
            return;
        }
        if (callbackContext.performed)
        {
            if (Gamepad.all.Count == 0)
            {

            }
            else if (Gamepad.current.leftShoulder.isPressed)
            {
                return;
            }
            if (true) //help me
            {
                attackState = AttackState.lightAttackSword1;
                animationState = AnimationState.lightAttack;
                currentlyAttacking = true;
            }
        }
    }

    void LightAttack()
    {
        //StartCoroutine(StopMovingWhileAttackingCombos(animator.GetCurrentAnimatorStateInfo(0).length));
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordCollider.position, attackRange, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {
                blockedOrParried = true;
                continue;
            }
            if (enemy.name == "SoldierSight")
            {
                continue;
            }
            if (enemy.CompareTag("Box"))
            {
                enemy.GetComponent<Box>().MoveOrDestroy(true);
            }
            Debug.Log("We hit " + enemy + " with a sword");
            if (enemy.GetComponent<IEnemy>() != null && !blockedOrParried)
            {
                //audioManager.PlaySound(STABSOUND);
                enemy.GetComponent<IEnemy>().TakeDamage(attackDamage, null);
            }
        }
        blockedOrParried = false;
        currentlyAttacking = false;
    }

    public void OnHeavyAttack(InputAction.CallbackContext callbackContext)
    { 
        if (hangingOnTheWall || attackState == AttackState.cannotAttack || onASlope || !axePickedUp)
        {
            return;
        }
        if (callbackContext.control.IsPressed())
        {
            if (true) //help, please
            {
                animationState = AnimationState.heavyAttack;
                currentlyAttacking = true;
            }
        }
    }

    void HeavyAttack()
    {

    }

    public void OnParry(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || onASlope || attackState != AttackState.notAttacking || currentlyAttacking)
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

    //Utilities
    public void PauseGame(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (isGamePaused == true)
            {
                if (!tooltipPanel.activeInHierarchy)
                {
                    pauseMenuPanel.SetActive(false);
                }
                else
                {
                    tooltipPanel.SetActive(false);
                }
                
                playerInput.SwitchCurrentActionMap("Player");
                Time.timeScale = 1f;
                isGamePaused = false;
            }
            else
            {
                pauseMenuPanel.SetActive(true);
                playerInput.SwitchCurrentActionMap("UI");
                Time.timeScale = 0f;
                isGamePaused = true;
            }
        }
    }

    public void ShowInteractionIcon()
    {
        interactionIconPrefab.SetActive(true);
        if (playerInput.currentControlScheme == "Gamepad")
        {
            interactionIconPrefab.GetComponent<SpriteRenderer>().sprite = gamepadInteractionIcon;
        }
        else
        {
            interactionIconPrefab.GetComponent<SpriteRenderer>().sprite = keyboardInteractionIcon;
        }
    }

    public void HideInteractionIcon()
    {
        interactionIconPrefab.SetActive(false);
    }

    public void CheckInteraction(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, interactionSize, 0f, Vector2.zero);
            if (hits.Length > 0)
            {
                foreach (RaycastHit2D raycastHits in hits)
                {
                    if (raycastHits.transform.GetComponent<Interactable>())
                    {
                        raycastHits.transform.GetComponent<Interactable>().Interact();
                    }
                }
            }
        }
    }

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

    public void Flip()
    {
        if (grounded)
        {
            CreateDust();
        }
        //Switch the way the player is labeled as facing.
        facingRight = !facingRight;

        //Multiply the player's x local scale by -1.
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

    //private void OnDrawGizmosSelected()
    //{
    //    if (groundCheck == null)
    //    {
    //        return;
    //    }
    //    Gizmos.DrawWireCube(groundCheck.position, groundCheckRange);
    //}
    
    void CreateDust()
    {
        movementAndJumpDust.Play();
    }

    void CreateDashParticleEffect()
    {
        dashParticleEffect.Play();
    }   

    //Animation manager
    public void AnimatorSwitchState(AnimationState newState)
    {
        if (oldAnimationState == newState.ToString())
        {
            return;
        }

        animator.Play(newState.ToString());

        oldAnimationState = newState.ToString();
    }
}
