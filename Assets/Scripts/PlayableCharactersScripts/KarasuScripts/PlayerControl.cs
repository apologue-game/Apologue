using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //Helpers
    public TMP_Text currentStanceText;
    readonly string swordStance = "Sword stance";
    readonly string axeStance = "Axe stance";
    public GameObject shadow;
    public StaminaBar staminaBar;
    public float comboWindow = 0.3f;
    public float comboEnd;
    public float switchStanceCooldown = 0.5f;
    public GameObject BoxPrefab;
    public float axeMediumAttackDashSpeed;
    Vector3 theScale;
    public float stuckTimer = 0f;
    public Transform currentParent;
    public bool groundedCoroutineActive = false;
    Vector3 slopeRotation = new Vector3(0, 0, -45f);
    public float crouchMoveOnMovingPlatformSpeed = 0f;


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
    Vector2 interactionSize = new Vector2(0.1f, 1); //Interactable object's range in which the interact button will be shown

    //Pause menu
    public GameObject interactionIconPrefab;
    public Sprite keyboardInteractionIcon;
    public Sprite gamepadInteractionIcon;
    public bool swordOrAxeStance = true;

    //Movement system
    public LayerMask movingPlatforms;
    //Move
    public float movementSpeed = 5.8f;
    float movementSpeedHelper; //No need for it if the movement doesn't stop while attacking
    public bool facingRight = true;
    [SerializeField]
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
    //Coroutine coroutineWallHanging = null;
    //float wallHangingColliderRange = 0f;
    public static bool hangingOnTheWall = false;
    public static bool wallJump = false;
    public static float hangingOnTheWallTimer = 1f;
    //public float wallJumpAdditionalForce = 150f;
    int wallJumpPushBackCounter = 0;
    bool initiatePushBackCounter = false;
    public float fallingClamp = 5f;

    //Falling
    bool falling = false;

    //Roll
    public float rollSpeedMultiplier = 2f;
    public float rollDuration;
    public float rollEnd;
    float timeUntilNextRoll;
    bool rollDirectionIfStationary = true;
    public bool isRolling = false;

    //Crouch
    public LayerMask whatIsCeiling;
    Transform ceilingCheck;
    public bool isCrouching = false;
    bool canStandUp;
    public float crouchSpeedMultiplier = 0.5f;

    //Slide
    bool isSliding = false;
    float slideDirection;
    public float slideSpeed = 15f;

    //Slopes
    public bool onASlope = false;
    public float slopeXPosition = 0f;
    public Vector2 slopeCheckOffset;
    public Vector2 slopeCheck = new Vector2(0.7f, 0.7f);
    public bool isOnSlope = false;
    public bool isOnGround = false;
    public LayerMask whatIsSlope;
    bool exitInterruption = false;
    bool rotated = false;

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
    public AttackState helperAttackState;
    public float nextTimeAttack;

    //Combos
    public bool mediumAttackSword2_Available = false;
    public bool heavyAttackSword2_Available = false;
    public bool mediumAttackAxe2_Available = false;
    public bool heavyAttackAxe2_Available = false;

    public float swordHeavyAttack1JumpForce;

    //Axe attacks
    public static bool axePickedUp = false; //Axe attacks only usable after finding the axe

    //Parry and block
    public Transform parryCollider;
    public GameObject parryColliderGO;
    float parryWindow = 0.27f;
    float nextParry = 0;
    float parryCooldown = 4f;

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
        roll,
        doubleJump,
        wallJump,
        wallSlide,
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
        swordLight1,
        swordLight2,
        swordLight3,
        swordHeavy1,
        swordHeavy2Start,
        swordHeavy2Fall,
        swordHeavy2Landing,
        swordMedium1,
        swordMedium2,
        axeLight1,
        axeLight2,
        axeLight3,
        axeHeavy1,
        axeHeavy2,
        axeMedium1,
        axeMedium2
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

        attackState = new AttackState();
        animationState = AnimationState.idle;

        interactionTooltip = transform.Find("InteractionTooltip").gameObject;
        interactionTooltipSR = interactionTooltip.GetComponent<SpriteRenderer>();
    }

    IEnumerator GroundedDelay()
    {
        yield return new WaitForSeconds(0.12f);
        if (rigidBody2D.IsTouchingLayers(whatIsGround))
        {
            grounded = true;
            falling = false;
        }
        else 
        {
            grounded = false;
        }
        groundedCoroutineActive = false; 
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
        if (isRolling)
        {
            return;
        }

        if (rigidBody2D.IsTouchingLayers(whatIsGround))
        {
            grounded = true;
            falling = false;
        }
        else
        {
            if (!groundedCoroutineActive)
            {
                groundedCoroutineActive = true;
                StartCoroutine(GroundedDelay());
            }
            
        }

        if (animationState == AnimationState.swordHeavy2Fall)
        {
            return;
        }

        if (grounded)
        {
            shadow.SetActive(true);
        }
        else
        {
            shadow.SetActive(false);
        }

        Slopes();

        if (onASlope)
        {
            rigidBody2D.velocity = Vector2.ClampMagnitude(rigidBody2D.velocity, 10);
            animationState = AnimationState.slide;
            doubleJump = true;
            return;
        }
        if (isSliding)
        {
            if (slideDirection == inputX) 
            {
                animationState = AnimationState.slide;
                return;
            }
            else //If the player stops inputing movement, the slide will stop
            {
                isSliding = false;
            }
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

        //Move character
        if (!isCrouching && attackState != AttackState.mediumAttackSword1)
        {
            rigidBody2D.velocity = new Vector2(inputX * movementSpeed, rigidBody2D.velocity.y);
        }
        else if (isCrouching && attackState != AttackState.mediumAttackSword1)
        {
            if (transform.parent != null)
            {
                rigidBody2D.velocity = new Vector2(inputX * crouchMoveOnMovingPlatformSpeed, rigidBody2D.velocity.y);
            }
            else
            {
                rigidBody2D.velocity = new Vector2(inputX * movementSpeed * crouchSpeedMultiplier, rigidBody2D.velocity.y);
            }
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
            animationState = AnimationState.wallSlide;
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, Mathf.Clamp(rigidBody2D.velocity.y, fallingClamp, Mathf.Infinity));
        }
        else if (parryColliderGO.activeSelf)
        {
            animationState = AnimationState.parry;
        }
        if (holdingJump)
        {
            rigidBody2D.AddForce(jumpForce * 2 * Vector2.up);
        }
        if (verticalSpeed < -1)
        {
            holdingJump = false;
            rigidBody2D.AddForce(25 * Vector2.down);
        }
        //if (initiatePushBackCounter)
        //{
        //    wallJumpPushBackCounter++;
        //    if (facingRight)
        //    {
        //        rigidBody2D.AddForce(wallJumpAdditionalForce * Vector2.left);
        //    }
        //    else
        //    {
        //        rigidBody2D.AddForce(wallJumpAdditionalForce * Vector2.right);
        //    }
        //    if (wallJumpPushBackCounter > 10)
        //    {
        //        initiatePushBackCounter = false;
        //        wallJumpPushBackCounter = 0;
        //    }
        //}
        if (attackState != AttackState.notAttacking || attackState != AttackState.cannotAttack)
        {
            if (helperAttackState != attackState)
            {
                helperAttackState = attackState;
                stuckTimer = Time.time + 1f;
            }
            if (Time.time >= stuckTimer)
            {
                attackState = AttackState.notAttacking;
            }
        }
    }

    private void Slopes()
    {
        isOnSlope = false;
        isOnGround = false;
        RaycastHit2D[] slopes = Physics2D.LinecastAll(transform.position, (Vector2)transform.position - slopeCheckOffset, whatIsSlope);
        foreach (RaycastHit2D tilemap in slopes)
        {
            if (tilemap.collider.transform.name == "SlopesTilemap")
            {
                exitInterruption = true;
                slopeXPosition = transform.position.x;
                isOnSlope = true;
                doubleJump = true;
            }
        }
        if (isOnSlope)
        {
            if (!facingRight)
            {
                Flip();
            }
            if (!rotated)
            {
                transform.Rotate(slopeRotation);
                rotated = true;
            }
            onASlope = true;
        }
        else
        {
            StartCoroutine(SlopeExitDelay());
        }
    }

    IEnumerator SlopeExitDelay()
    {
        exitInterruption = false;
        if (rotated)
        {
            transform.Rotate(-slopeRotation);
            rotated = false;
        }
        yield return new WaitForSeconds(0.16f);
        if (!exitInterruption)
        {
            onASlope = false;
        }
    }

    void Update()
    {
        if (rigidBody2D.IsTouchingLayers(movingPlatforms))
        {
            if (transform.parent != null)
            {
                currentParent = transform.parent;
            }
            if (inputX != 0 && !isCrouching)
            {
                transform.SetParent(null);
            }
            else
            {
                transform.SetParent(currentParent);
            }
        }
        if (inputX > 0)
        {
            rollDirectionIfStationary = true;
        }
        else if (inputX < 0)
        {
            rollDirectionIfStationary = false;
        }
        if (grounded)
        {
            doubleJump = true;
        }
        //if (transform.position.x > slopeXPosition + 3)
        //{
        //    jumpAvailable = true;
        //}
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
        if (isRolling)
        {
            if (Time.time > rollEnd)
            {
                isRolling = false;
            }
        }
        if (animationState == AnimationState.swordHeavy2Fall)
        {
            if (grounded)
            {
                SwordHeavyAttack2Landing();
                return;
            }
        }
    }

    //Movement
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        inputX = callbackContext.ReadValue<Vector2>().x;
        if (onASlope && inputX < 0)
        {
            inputX = 0;
        }
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
        if (onASlope && callbackContext.performed/* && jumpAvailable*/)
        {
            //jumpAvailable = false;
            rigidBody2D.velocity = new Vector2(1 * jumpForce/2, 1 * jumpForce);
            CreateDust();
            return;
        }
        if (grounded && callbackContext.performed && attackState == AttackState.notAttacking && !onASlope)
        {
            rigidBody2D.velocity = Vector2.up * jumpForce;
            isCrouching = false;
            isSliding = false;
            jumpCounter = 1;
            staminaBar.currentStamina -= 20;
            staminaBar.regenerationDelay = Time.time + 2;
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
            CreateDust();
        }
    }

    public void OnRoll(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || onASlope)
        {
            return;
        }
        if (grounded && Time.time > timeUntilNextRoll && !isCrouching && callbackContext.performed && attackState == AttackState.notAttacking)
        {
            if (inputX > 0)
            {
                rigidBody2D.velocity = new Vector2(inputX * movementSpeed * rollSpeedMultiplier, rigidBody2D.velocity.y);
                timeUntilNextRoll = Time.time + 1;
            }
            else if (inputX < 0)
            {
                rigidBody2D.velocity = new Vector2(inputX * movementSpeed * rollSpeedMultiplier, rigidBody2D.velocity.y);
                timeUntilNextRoll = Time.time + 1;
            }
            else
            {
                if (rollDirectionIfStationary)
                {
                    rigidBody2D.velocity = new Vector2(movementSpeed * rollSpeedMultiplier, rigidBody2D.velocity.y);
                    timeUntilNextRoll = Time.time + 1;
                }
                else if (!rollDirectionIfStationary)
                {
                    rigidBody2D.velocity = new Vector2(-1 * movementSpeed * rollSpeedMultiplier, rigidBody2D.velocity.y);
                    timeUntilNextRoll = Time.time + 1;
                }
            }
            staminaBar.currentStamina -= 20;
            staminaBar.regenerationDelay = Time.time + 2;
            rollEnd = Time.time + rollDuration;
            animationState = AnimationState.roll;
            isRolling = true;
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
            StartCoroutine(IsSliding());
        }
    }

    IEnumerator IsSliding()
    {
        isSliding = true;
        yield return new WaitForSeconds(0.35f);
        isSliding = false;
    }


    //Combat system
    public void OnSwordLightAttack1(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && attackState == AttackState.notAttacking && !mediumAttackSword2_Available)
        {
            attackState = AttackState.lightAttackSword1;
            animationState = AnimationState.swordLight1;
        }
    }

    public void OnSwordLightAttack2(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && attackState == AttackState.notAttacking)
        {
            attackState = AttackState.lightAttackSword2;
            animationState = AnimationState.swordLight2;
        }
    }

    public void OnSwordLightAttack3(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed  && attackState == AttackState.notAttacking)
        {
            attackState = AttackState.lightAttackSword3;
            animationState = AnimationState.swordLight3;
        }
    }

    public void OnSwordMediumAttack1(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed  && attackState == AttackState.notAttacking &&!mediumAttackSword2_Available && inputX != 0)
        {
            attackState = AttackState.mediumAttackSword1;
            animationState = AnimationState.swordMedium1;
        }
    }
    
    void SwordMediumAttackDash()
    {
        rigidBody2D.velocity = new Vector2(inputX * movementSpeed * 2.15f, rigidBody2D.velocity.y);
    }
    
    public void OnSwordMediumAttack2(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed  && attackState == AttackState.notAttacking && mediumAttackSword2_Available)
        {
            attackState = AttackState.mediumAttackSword2;
            animationState = AnimationState.swordMedium2;
            mediumAttackSword2_Available = false;
        }
    }

    
    public void OnSwordHeavyAttack1(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed  && attackState == AttackState.notAttacking && !heavyAttackSword2_Available)
        {
            attackState = AttackState.heavyAttackSword1;
            animationState = AnimationState.swordHeavy1;
        }
    }

    void SwordHeavyAttackJump()
    {
        rigidBody2D.velocity = (Vector2.up * swordHeavyAttack1JumpForce);
    }

    public void OnSwordHeavyAttack2(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && heavyAttackSword2_Available)
        {
            attackState = AttackState.heavyAttackSword2;
            animationState = AnimationState.swordHeavy2Start;
            heavyAttackSword2_Available = false;
        }
    }

    void SwordHeavyAttack2Fall()
    {
        animationState = AnimationState.swordHeavy2Fall;
    }

    void SwordHeavyAttack2Landing()
    {
        animationState = AnimationState.swordHeavy2Landing;
        NotAttacking();
    }

    public void OnAxeLightAttack1(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && attackState == AttackState.notAttacking)
        {
            attackState = AttackState.lightAttackAxe1;
            animationState = AnimationState.axeLight1;
        }
    }

    public void OnAxeLightAttack2(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && attackState == AttackState.notAttacking)
        {
            attackState = AttackState.lightAttackAxe2;
            animationState = AnimationState.axeLight2;
        }
    }

    public void OnAxeLightAttack3(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && attackState == AttackState.notAttacking && !mediumAttackAxe2_Available)
        {
            attackState = AttackState.lightAttackAxe3;
            animationState = AnimationState.axeLight3;
        }
    }

    public void OnAxeMediumAttack1(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && attackState == AttackState.notAttacking && !mediumAttackAxe2_Available && inputX != 0)
        {
            attackState = AttackState.mediumAttackAxe1;
            animationState = AnimationState.axeMedium1;
        }
    }

    void AxeMediumAttackDash()
    {
        rigidBody2D.velocity = new Vector2(inputX * movementSpeed * axeMediumAttackDashSpeed, rigidBody2D.velocity.y);
    }

    public void OnAxeMediumAttack2(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && attackState == AttackState.notAttacking && mediumAttackAxe2_Available)
        {
            attackState = AttackState.mediumAttackAxe2;
            animationState = AnimationState.axeMedium2;
            mediumAttackAxe2_Available = false;
        }
    }

    public void OnAxeHeavyAttack1(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && attackState == AttackState.notAttacking && !heavyAttackAxe2_Available && !mediumAttackAxe2_Available)
        {
            attackState = AttackState.heavyAttackAxe1;
            animationState = AnimationState.axeHeavy1;
        }
    }

    public void OnAxeHeavyAttack2(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed/* && attackState == AttackState.notAttacking*/ && heavyAttackAxe2_Available)
        {
            attackState = AttackState.heavyAttackAxe2;
            animationState = AnimationState.axeHeavy2;
            heavyAttackAxe2_Available = false;
        }
    }

    public void ChangeCurrentStance(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && Time.time >= switchStanceCooldown)
        {
            if (swordOrAxeStance)
            {
                playerInput.SwitchCurrentActionMap("PlayerAxe");
                currentStanceText.text = axeStance;
            }
            else if (!swordOrAxeStance)
            {
                playerInput.SwitchCurrentActionMap("PlayerSword");
                currentStanceText.text = swordStance;
            }
            switchStanceCooldown = Time.time + 0.5f;
            attackState = AttackState.notAttacking;
            swordOrAxeStance = !swordOrAxeStance;

            ChangeStance.ChangeCurrentStance();
        }
    }

    public void OnParry(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || onASlope || attackState != AttackState.notAttacking)
        {
            return;
        }
        if (callbackContext.performed  && attackState == AttackState.notAttacking)
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

                if (swordOrAxeStance)
                {
                    playerInput.SwitchCurrentActionMap("PlayerSword");
                }
                else
                {
                    playerInput.SwitchCurrentActionMap("PlayerAxe");
                }
                
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

    public void NotAttacking()
    {
        attackState = AttackState.notAttacking;
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
        theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }   

    IEnumerator ParryWindow()
    {
        parryColliderGO.SetActive(true);
        yield return new WaitForSeconds(parryWindow);
        parryColliderGO.SetActive(false);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireCube((Vector2)transform.position - slopeCheckOffset, a);
    //}

    void CreateDust()
    {
        movementAndJumpDust.Play();
    }

    void CreateDashParticleEffect()
    {
        dashParticleEffect.Play();
    }   

    public void CreateBox(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Quaternion spawnRotation = new Quaternion();
            Vector3 boxSpawnPosition = new Vector3(transform.position.x + 3, transform.position.y + 3, transform.position.z);
            Instantiate(BoxPrefab, boxSpawnPosition, spawnRotation);
        }
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
