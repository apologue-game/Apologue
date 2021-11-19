using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public static ApologuePlayerInput_Actions playerinputActions;
    public static PlayerInput playerInput;
    Rigidbody2D rigidBody2D;
    Animator animator;
    WallTilemaps wallTilemaps;
    public ParticleSystem dust;
    public ParticleSystem dashParticleEffect;
    public AudioManager audioManager;

    KarasuEntity karasuEntity;
    public HealthBar healthBar;
    public Image healthBarFill;
    public Color healthBarColorInvulnerable;

    //Interactable objects -> icon above the head
    public GameObject interactionIconPrefab;
    GameObject interactionTooltip;
    SpriteRenderer interactionTooltipSR;
    public SpriteRenderer tooltipInteractionSR;
    public Sprite keyboardInteractionIcon;
    public Sprite gamepadInteractionIcon;

    //Movement system
    //Move
    public float movementSpeed = 5.8f;
    float movementSpeedHelper;
    public bool facingRight = true;
    public float inputX;

    //Jump
    public float jumpForce = 25f;
    public float verticalSpeedAbsolute;
    float verticalSpeed;
    public bool grounded;
    public Transform groundCheck;
    public Vector3 groundCheckRange;
    private const float groundCheckRadius = .3f;
    private const float ceilingCheckRadius = .3f;
    public LayerMask whatIsGround;
    int jumpHoldCounter = 0;
    bool holdingJump = false;

    //Double jump
    public float doubleJumpForce = 20f;
    public int jumpCounter = 0;
    bool doubleJump;

    //Wall jump
    public Transform wallHangingCollider;
    Coroutine coroutineWallHanging = null;
    public float wallHangingColliderRange = 0f;
    public static bool hangingOnTheWall = false;
    public static bool wallJump = false;
    public static float hangingOnTheWallTimer = 1f;
    public float wallJumpAdditionalForce = 50f;
    public int wallJumpPushBackCounter = 0;
    public bool initiatePushBackCounter = false;

    //Falling
    bool falling = false;

    //Dash
    public float dashSpeed = 5f;
    float timeUntilNextDash;
    bool dashDirectionIfStationary = true;
    public bool isDashing = false;
    public bool dashInAirAvailable = true;

    //Crouch
    public LayerMask whatIsCeiling;
    Transform ceilingCheck;
    public BoxCollider2D regularCollider;
    public BoxCollider2D slideCollider;
    public bool isCrouching = false;
    public bool canStandUp;
    public float crouchSpeedMultiplier = 0.5f;
    //Crouch roll
    public bool isRolling = false;
    public float rollDirection;

    //Slide
    public bool isSliding = false;
    public float slideDirection;
    public float slideSpeed = 15f;

    //Slopes
    public bool onASlope = false;
    public bool jumpAvailable = true;
    public float slopeXPosition = 0f;

    //Combat system
    public LayerMask enemiesLayers;
    public float globalAttackCooldown = 5f;
    float nextGlobalAttack = 0f;
    enum AttackState
    {
        notAttacking,
        cannotAttack,
        lightAttack,
        lightAttackUpwards,
        mediumAttack,
        heavyAttack
    }
    AttackState attackState;
    bool combo;
    bool comboFinished;
    //Light attack
    public Transform swordCollider;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public float attackSpeed = 0.75f;
    float nextAttackTime = 0f;
    //Light attack combos
    public Transform swordUppercutCollider;
    public float attackRangeUppercut = 0.5f;
    float comboTimeWindow = 0f;
    public int numberOfAttacks = 0;
    int attackDamageCombo = 2;

    //Medium attack
    public Transform spearCollider;
    public Vector3 spearRange;
    public float attackRangeSpear = 0.5f;
    public int attackDamageSpear = 2;
    public float attackSpeedSpear = 0.2f;
    float nextAttackTimeSpear = 0f;

    //Heavy attack
    public Transform axeCollider;
    public float attackRangeAxe = 0.5f;
    public int attackDamageAxe = 2;
    public float attackSpeedAxe = 0.2f;
    float nextAttackTimeAxe = 0f;
    public static bool axePickedUp = false;

    //Parry and block
    public Transform parryCollider;
    public GameObject parryColliderGO;
    public GameObject blockColliderGO;
    bool blocking = false;
    float parryWindow = 0.4f;
    float nextParry = 0;
    float parryCooldown = 4f;
    //Enemy blocking
    public bool currentlyAttacking = false;

    //Utilities
    //Pause menu
    public GameObject pauseMenuPanel;
    public GameObject tooltipPanel;
    public static bool isGamePaused = false;

    //Testing

    //Animation manager
    string oldAnimationState;

    //Animation names
    const string IDLEANIMATION = "karasuIdleAnimation";
    const string WALKANIMATION = "karasuWalkAnimation";
    const string JUMPANIMATION = "karasuJumpAnimation";
    const string DOUBLEJUMPANIMATION = "karasuDoubleJumpAnimation";
    const string WALLJUMPANIMATION = "karasuWallJumpAnimation";
    const string WALLHANGINGANIMATION = "karasuWallHangingAnimation";
    const string FALLINGANIMATION = "karasuFallingAnimation";
    const string CROUCHMOVEANIMATION = "karasuCrouchMoveAnimation";
    const string CROUCHIDLEANIMATION = "karasuCrouchIdleAnimation";
    const string SLIDEANIMATION = "karasuSlideAnimation";
    const string CROUCHROLLANIMATION = "karasuCrouchRollAnimation";
    const string DEATHANIMATION = "karasuDeathAnimation";
    const string DASHANIMATION = "karasuDashAnimation";
    const string PARRYANIMATION = "karasuParryAnimation";
    const string BLOCKANIMATION = "karasuBlockAnimation";
    const string BLOCKEDANDHITANIMATION = "karasuBlockedAndHitAnimation";
    const string LIGHTATTACKANIMATION = "karasuLightAttackAnimation";
    const string LIGHTATTACKUPWARDSANIMATION = "karasuLightAttackUpwardsAnimation";
    const string MEDIUMATTACKANIMATION = "karasuMediumAttackAnimation";
    const string HEAVYATTACKANIMATION = "karasuHeavyAttackAnimation";
    const string KARASUDEATHANIMATION = "karasuDeathAnimation";

    //Audio manager
    const string WALKSOUND = "walk";
    const string STABSOUND = "stab";
    const string SLIDESOUND = "slide";
    const string JUMPLANDINGSOUND = "jumpLanding";
    const string PARRYSOUND = "parry";
    const string WOODBREAKINGSOUND = "woodBreaking";

    //Help
    public float horizontalSpeed;
    void Awake()
    {
        playerinputActions = new ApologuePlayerInput_Actions();
        playerinputActions.Enable();
        playerInput = GetComponent<PlayerInput>();
        wallTilemaps = GameObject.Find("WallTilemapTrigger").GetComponent<WallTilemaps>();
        karasuEntity = GetComponent<KarasuEntity>();

        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");

        movementSpeedHelper = movementSpeed;
        spearRange = new Vector3(2.44f, 0.34f, 0);

        groundCheckRange = new Vector3(2.44f, 0.34f, 0);

        attackState = new AttackState();

        interactionTooltip = transform.Find("InteractionTooltip").gameObject;
        interactionTooltipSR = interactionTooltip.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {

    }

    void FixedUpdate()
    {
        if (KarasuEntity.dead)
        {
            if (!KarasuEntity.spikesDeath)
            {
                AnimatorSwitchState(KARASUDEATHANIMATION);
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
            AnimatorSwitchState(SLIDEANIMATION);
            return;
        }
        if (isSliding)
        {
            if (slideDirection == inputX)
            {
                AnimatorSwitchState(SLIDEANIMATION);
                return;
            }
            else
            {
                isSliding = false;
            }
        }
        else if (isRolling)
        {
            if (rollDirection == inputX)
            {
                AnimatorSwitchState(CROUCHROLLANIMATION);
                return;
            }
            else
            {
                isRolling = false;
            }
        }

        grounded = false;
        //Colliders->check to see if the player is currently on the ground
        Collider2D[] collidersGround = Physics2D.OverlapBoxAll(groundCheck.position, groundCheckRange, whatIsGround);
        for (int i = 0; i < collidersGround.Length; i++)
        {
            if (collidersGround[i].name == "PlatformsTilemap" || collidersGround[i].name == "GroundTilemap" || collidersGround[i].CompareTag("Box") || collidersGround[i].CompareTag("FallingPlatforms"))
            {
                grounded = true;
                falling = false;
                dashInAirAvailable = true;
            }
        }

        //Check to see if there is a ceiling above the player so they can get up from the crouching state
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
        horizontalSpeed = Math.Abs(rigidBody2D.velocity.y);
        verticalSpeedAbsolute = Math.Abs(rigidBody2D.velocity.y);
        verticalSpeed = rigidBody2D.velocity.y;
        if (!blocking && !parryColliderGO.activeSelf && !hangingOnTheWall)
        {
            if (attackState == AttackState.notAttacking || attackState == AttackState.cannotAttack)
            {
                //Falling
                if (!grounded && verticalSpeed < -12 && !falling)
                {
                    falling = true;
                    AnimatorSwitchState(FALLINGANIMATION);
                }
                //Walking and idle
                if (grounded && inputX != 0 && !isCrouching)
                {
                    AnimatorSwitchState(WALKANIMATION);
                }
                else if (grounded && inputX == 0 && !isCrouching)
                {
                    AnimatorSwitchState(IDLEANIMATION);
                }
                else if (grounded && inputX != 0 && isCrouching)
                {
                    AnimatorSwitchState(CROUCHMOVEANIMATION);
                }
                else if (grounded && inputX == 0 && isCrouching)
                {
                    AnimatorSwitchState(CROUCHIDLEANIMATION);
                }
                //Jumping
                else if (!grounded && verticalSpeedAbsolute > 0)
                {
                    if (jumpCounter == 2)
                    {
                        AnimatorSwitchState(DOUBLEJUMPANIMATION);
                        goto DONE;
                    }
                    AnimatorSwitchState(JUMPANIMATION);
                }
            }
        DONE:;
        }
        else if (hangingOnTheWall)
        {
            AnimatorSwitchState(WALLHANGINGANIMATION);
        }
        else if (parryColliderGO.activeSelf)
        {
            AnimatorSwitchState(PARRYANIMATION);
        }
        else if (blockColliderGO.activeSelf)
        {
            AnimatorSwitchState(BLOCKANIMATION);
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
        if (inputX > 0 && !facingRight && !onASlope && !blocking && !hangingOnTheWall)
        {
            Flip();
            interactionTooltipSR.flipX = false;
        }
        else if (inputX < 0 && facingRight && !onASlope && !blocking && !hangingOnTheWall)
        {
            Flip();
            interactionTooltipSR.flipX = true;
        }
        AudioManager();
    }

    //Audio manager
    void AudioManager()
    {
        if (grounded && inputX != 0 && !currentlyAttacking)
        {
            audioManager.PlaySound(WALKSOUND);
        }
        else if (!grounded || inputX == 0 || currentlyAttacking)
        {
            audioManager.StopSound(WALKSOUND);
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
        if (grounded && callbackContext.performed && !blocking && attackState == AttackState.notAttacking && !onASlope)
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
        if (!grounded && callbackContext.performed && doubleJump && !blocking)
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

    public void OnCrouchRoll(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && isCrouching && inputX != 0)
        {
            rollDirection = inputX;
            rigidBody2D.velocity = new Vector2(inputX * movementSpeed * 1.2f, rigidBody2D.velocity.y);
            StartCoroutine(IsRolling());
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
            audioManager.PlaySound(SLIDESOUND);
            StartCoroutine(IsSliding());
        }
    }

    IEnumerator IsSliding()
    {
        isSliding = true;
        yield return new WaitForSeconds(0.35f);
        isSliding = false;
    }

    IEnumerator IsRolling()
    {
        isRolling = true;
        yield return new WaitForSeconds(0.35f);
        isRolling = false;
    }

    IEnumerator IsDashing()
    {
        isDashing = true;
        yield return new WaitForSeconds(0.25f);
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
            if (Time.time >= nextAttackTime && Time.time >= nextGlobalAttack /*&& !currentlyAttacking*/)
            {
                combo = false;
                attackState = AttackState.lightAttack;
                AnimatorSwitchState(LIGHTATTACKANIMATION);
                numberOfAttacks = 0;
                numberOfAttacks++;
                currentlyAttacking = true;
                nextAttackTime = Time.time + 0.8f;
                nextGlobalAttack = Time.time + 0.11f + 0.5f;
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
            if (enemy.GetComponent<IEnemy>() != null)
            {
                audioManager.PlaySound(STABSOUND);
                enemy.GetComponent<IEnemy>().TakeDamage(attackDamage, null);
            }
        }
        currentlyAttacking = false;
    }

    void LightAttackUpwardsAnimation()
    {
        if (numberOfAttacks == 2 && combo)
        {
            numberOfAttacks++;
            attackState = AttackState.lightAttackUpwards;
            currentlyAttacking = true;
            AnimatorSwitchState(LIGHTATTACKUPWARDSANIMATION);
        }
    }

    void LightAttackUpwards()
    {
        StartCoroutine(StopMovingWhileAttackingCombos(animator.GetCurrentAnimatorStateInfo(0).length - 0.2f));
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordUppercutCollider.position, attackRangeUppercut, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {
                continue;
            }
            if (enemy.name == "SoldierSight")
            {
                continue;
            }
            Debug.Log("We hit " + enemy + " with a sword uppercut");
            audioManager.PlaySound(STABSOUND);
            if (enemy.GetComponent<IEnemy>() != null)
            {
                enemy.GetComponent<IEnemy>().TakeDamage(attackDamageCombo, null);
            }
        }
        currentlyAttacking = false;
    }

    //public void OnMediumAttack(InputAction.CallbackContext callbackContext)
    //{
    //    if (hangingOnTheWall || attackState == AttackState.cannotAttack || onASlope)
    //    {
    //        return;
    //    }
    //    if (callbackContext.control.IsPressed())
    //    {
    //        if (Time.time >= nextAttackTimeSpear && Time.time >= nextGlobalAttack)
    //        {
    //            attackState = AttackState.mediumAttack;
    //            AnimatorSwitchState(MEDIUMATTACKANIMATION);
    //            nextAttackTimeSpear = Time.time + 1f / attackSpeedSpear;
    //            nextGlobalAttack = Time.time + 1.5f;
    //            currentlyAttacking = true;
    //        }
    //    }
    //}

    //public void MediumAttack()
    //{
    //    StartCoroutine(StopMovingWhileAttacking(animator.GetCurrentAnimatorStateInfo(0).length));
    //    Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(spearCollider.position, spearRange, 0, enemiesLayers);
    //    foreach (Collider2D enemy in hitEnemies)
    //    {
    //        if (enemy.name == "BlockColliderSoldier")
    //        {
    //            continue;
    //        }
    //        if (enemy.name == "SoldierSight")
    //        {
    //            continue;
    //        }
    //        Debug.Log("We hit " + enemy + " with a spaer");
    //        if (enemy.GetComponent<IEnemy>() != null)
    //        {
    //            enemy.GetComponent<IEnemy>().TakeDamage(attackDamage, null);
    //        }
    //    }
    //    currentlyAttacking = false;
    //}

    public void OnHeavyAttack(InputAction.CallbackContext callbackContext)
    { 
        if (hangingOnTheWall || attackState == AttackState.cannotAttack || onASlope || !axePickedUp)
        {
            return;
        }
        if (callbackContext.control.IsPressed())
        {
            if (Time.time >= nextAttackTimeAxe && Time.time >= nextGlobalAttack)
            {
                attackState = AttackState.heavyAttack;
                AnimatorSwitchState(HEAVYATTACKANIMATION);
                nextAttackTimeAxe = Time.time +/* 1f / attackSpeedAxe + 3f*/+ 1f;
                nextGlobalAttack = Time.time + 1.5f;
                currentlyAttacking = true;
            }
        }
    }

    void StopMovingWhileAttackingAnimationEvent()
    {
        StartCoroutine(StopMovingWhileAttacking(animator.GetCurrentAnimatorStateInfo(0).length - 0.4f));
    }

    void HeavyAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(axeCollider.position, attackRangeAxe, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BlockColliderSoldier")
            {
                continue;
            }
            if (enemy.name == "SoldierSight")
            {
                continue;
            }
            if (enemy.CompareTag("Box"))
            {
                enemy.GetComponent<Box>().MoveOrDestroy(false);
            }
            if (enemy.name.Contains("Shieldman"))
            {
                enemy.GetComponent<IEnemy>().TakeDamage(attackDamageAxe, true);
                audioManager.PlaySound(WOODBREAKINGSOUND);
            }
            else
            {
                Debug.Log("We hit " + enemy + " with an axe");
                audioManager.PlaySound(STABSOUND);
                if (enemy.GetComponent<IEnemy>() != null)
                {
                    enemy.GetComponent<IEnemy>().TakeDamage(attackDamage, null);
                }
            }
        }
        
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

    public void OnBlock(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || onASlope || attackState != AttackState.notAttacking || currentlyAttacking)
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

    IEnumerator StopMovingWhileAttackingCombos(float waitingDuration)
    {
        if (grounded)
        {
            movementSpeed = 0;
            yield return new WaitForSeconds(waitingDuration - 0.2f);
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
        else
        {
            yield return new WaitForSeconds(waitingDuration - 0.2f);
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
        currentlyAttacking = false;
    }

    IEnumerator StopMovingWhileAttacking(float waitingDuration)
    {
        if (grounded)
        {
            movementSpeed = 0;
            yield return new WaitForSeconds(waitingDuration - 0.15f);
            movementSpeed = movementSpeedHelper;
            attackState = AttackState.notAttacking;
        }
        else
        {
            yield return new WaitForSeconds(waitingDuration - 0.15f);
            attackState = AttackState.notAttacking;
        }
        currentlyAttacking = false;
    }

    IEnumerator ComboWindow()
    {
        yield return new WaitForSeconds(0.4f);
        combo = false;
        comboFinished = false;
        attackState = AttackState.notAttacking;
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

    Vector2 interactionSize = new Vector2(0.1f, 1);

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

    //public static void ShowInteractionIcon()
    //{
    //    playerControl.interactionTooltip.SetActive(true);
    //    if (playerInput.currentControlScheme != "Gamepad")
    //    {
    //        playerControl.tooltipInteractionSR.sprite = playerControl.keyboardInteractionIcon;
    //    }
    //    else
    //    {
    //        playerControl.tooltipInteractionSR.sprite = playerControl.gamepadInteractionIcon;
    //    }
    //}

    //public static void HideInteractionIcon()
    //{
    //    playerControl.interactionTooltip.SetActive(false);
    //}

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }
        Gizmos.DrawWireCube(groundCheck.position, groundCheckRange);
    }
    
    void CreateDust()
    {
        dust.Play();
    }

    void CreateDashParticleEffect()
    {
        dashParticleEffect.Play();
    }   

    //Animation manager
    public void AnimatorSwitchState(string newState)
    {
        if (oldAnimationState == newState)
        {
            return;
        }

        animator.Play(newState);

        oldAnimationState = newState;
    }
}
