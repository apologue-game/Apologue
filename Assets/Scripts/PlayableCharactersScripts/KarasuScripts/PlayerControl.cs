using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public static ApologuePlayerInput_Actions playerinputActions;
    static PlayerInput playerInput;
    Rigidbody2D rigidBody2D;
    Animator animator;
    WallTilemaps wallTilemaps;

    //Movement system
    //Move
    public float movementSpeed = 8f;
    float movementSpeedHelper;
    bool facingRight = true;
    float inputX;

    //Jump
    public float jumpForce = 25f;
    float verticalSpeedAbsolute;
    float verticalSpeed;
    public bool grounded;
    public Transform groundCheck;
    public Vector3 groundCheckRange;
    private const float groundCheckRadius = .3f;
    private const float ceilingCheckRadius = .3f;
    public LayerMask whatIsGround;

    //Double jump
    public float doubleJumpForce = 20f;
    int jumpCounter = 0;
    bool doubleJump;

    //Wall jump
    public static bool hangingOnTheWall = false;
    public static bool wallJump = false;
    public static float hangingOnTheWallTimer = 2f;

    //Falling
    bool falling = false;

    //Dash
    public float dashSpeed = 5f;
    float timeUntilNextDash;
    bool dashDirectionIfStationary = true;

    //Crouch
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

    //Parry and block
    public Transform parryCollider;
    public GameObject parryColliderGO;
    public GameObject blockColliderGO;
    bool blocking = false;
    float parryWindow = 0.4f;
    float nextParry = 0;
    float parryCooldown = 4f;
    //Enemy blocking
    public static bool currentlyAttacking = false;
    //Testing

    //Animation manager
    string oldState;

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

    //Teleportation
    public Transform location1;
    public Transform location2;
    bool location = false;

    //Spawning
    public GameObject heavyPrefab;
    public GameObject shieldmanPrefab;
    public GameObject metalBox;
    public GameObject woodenBox;

    void Awake()
    {
        playerinputActions = new ApologuePlayerInput_Actions();
        playerinputActions.Enable();
        playerInput = GetComponent<PlayerInput>();
        wallTilemaps = GameObject.Find("WallTilemapTrigger").GetComponent<WallTilemaps>();

        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");

        movementSpeedHelper = movementSpeed;
        spearRange = new Vector3(2.44f, 0.34f, 0);

        groundCheckRange = new Vector3(2.44f, 0.34f, 0);

        attackState = new AttackState();
    }

    void FixedUpdate()
    {
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
            if (collidersGround[i].name == "PlatformsTilemap" || collidersGround[i].name == "GroundTilemap" || collidersGround[i].tag == "Box")
            {
                grounded = true;
                falling = false;
            }
        }
        canStandUp = true;
        //Check to see if there is a ceiling above the player so they can get up from the crouching state
        Collider2D[] collidersCeiling = Physics2D.OverlapCircleAll(ceilingCheck.position, ceilingCheckRadius, whatIsGround);
        for (int i = 0; i < collidersCeiling.Length; i++)
        {
            if (collidersCeiling[i].name == "PlatformsTilemap" || collidersGround[i].name == "GroundTilemap")
            {
                canStandUp = false;
            }
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
        if (!blocking && !parryColliderGO.activeSelf && !hangingOnTheWall)
        {
            if (attackState == AttackState.notAttacking || attackState == AttackState.cannotAttack)
            {
                //Falling
                if (!grounded && verticalSpeed < -6 && !falling)
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
        if (grounded)
        {
            wallTilemaps.oldPosition = wallTilemaps.newPosition - 50;
            if (attackState == AttackState.cannotAttack)
            {
                attackState = AttackState.notAttacking;
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
    }

    //Movement
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        inputX = callbackContext.ReadValue<Vector2>().x;
        if (hangingOnTheWall)
        {
            return;
        }

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
        if (grounded && callbackContext.performed && !blocking && attackState == AttackState.notAttacking)
        {
            rigidBody2D.velocity = Vector2.up * jumpForce;
            jumpCounter = 1;
        }
        if (hangingOnTheWall && wallJump && callbackContext.performed)
        {
            hangingOnTheWall = false;
            wallJump = false;
            rigidBody2D.velocity = Vector2.up * jumpForce;
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            attackState = AttackState.cannotAttack;
            if (inputX > 0 && !facingRight)
            {
                Flip();
            }
            else if (inputX < 0 && facingRight)
            {
                Flip();
            }
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
        if (Time.time > timeUntilNextDash && !isCrouching)
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

    public void OnCrouchRoll(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && isCrouching && inputX != 0)
        {
            isRolling = true;
            rollDirection = inputX;
            rigidBody2D.velocity = new Vector2(inputX * movementSpeed * 1.2f, rigidBody2D.velocity.y);
            StartCoroutine(IsRolling());
        }
    }

    public void OnCrouch(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
        if (callbackContext.performed && inputX == 0 && grounded && !isCrouching)
        {
            Debug.Log("Crouch");
            isCrouching = true;
        }
        else if (callbackContext.performed && isCrouching && !isSliding)
        {
            if (canStandUp)
            {
                Debug.Log("Stand up");
                isCrouching = false;
            }
        }
    }

    public void OnSlide(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall)
        {
            return;
        }
        if (callbackContext.performed && inputX != 0 && grounded && !isCrouching)
        {
            Debug.Log("Slide");
            isCrouching = true;
            slideDirection = inputX;
            rigidBody2D.velocity = new Vector2(inputX * movementSpeed * 2f, rigidBody2D.velocity.y);
            regularCollider.enabled = false;
            slideCollider.enabled = true;
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

    //Combat system
    public void OnLightAttack(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || attackState == AttackState.cannotAttack)
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
                attackState = AttackState.lightAttack;
                AnimatorSwitchState(LIGHTATTACKANIMATION);
                numberOfAttacks = 0;
                currentlyAttacking = true;
                nextAttackTime = Time.time + 0.11f + 1f;
                nextGlobalAttack = Time.time + 0.11f + 1.5f;
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
            if (enemy.tag == "Box")
            {
                enemy.GetComponent<Box>().MoveOrDestroy(true);
            }
            Debug.Log("We hit " + enemy + " with a sword");
            if (enemy.GetComponent<IEnemy>() != null)
            {
                enemy.GetComponent<IEnemy>().TakeDamage(attackDamage, null);
            }
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
                continue;
            }
            if (enemy.name == "SoldierSight")
            {
                continue;
            }
            Debug.Log("We hit " + enemy + " with a sword uppercut");
            if (enemy.GetComponent<IEnemy>() != null)
            {
                enemy.GetComponent<IEnemy>().TakeDamage(attackDamage, null);
            }
        }
        currentlyAttacking = false;
    }

    public void OnMediumAttack(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || attackState == AttackState.cannotAttack)
        {
            return;
        }
        if (callbackContext.control.IsPressed())
        {
            if (Time.time >= nextAttackTimeSpear && Time.time >= nextGlobalAttack)
            {
                attackState = AttackState.mediumAttack;
                AnimatorSwitchState(MEDIUMATTACKANIMATION);
                nextAttackTimeSpear = Time.time + 1f / attackSpeedSpear;
                nextGlobalAttack = Time.time + 1.5f;
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
                continue;
            }
            if (enemy.name == "SoldierSight")
            {
                continue;
            }
            Debug.Log("We hit " + enemy + " with a spaer");
            if (enemy.GetComponent<IEnemy>() != null)
            {
                enemy.GetComponent<IEnemy>().TakeDamage(attackDamage, null);
            }
        }
        currentlyAttacking = false;
    }

    public void OnHeavyAttack(InputAction.CallbackContext callbackContext)
    {
        if (hangingOnTheWall || attackState == AttackState.cannotAttack)
        {
            return;
        }
        if (callbackContext.control.IsPressed())
        {
            if (Time.time >= nextAttackTimeAxe && Time.time >= nextGlobalAttack)
            {
                attackState = AttackState.heavyAttack;
                AnimatorSwitchState(HEAVYATTACKANIMATION);
                nextAttackTimeAxe = Time.time + 1f / attackSpeedAxe;
                nextGlobalAttack = Time.time + 1.5f;
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
                continue;
            }
            if (enemy.name == "SoldierSight")
            {
                continue;
            }
            if (enemy.tag == "Box")
            {
                enemy.GetComponent<Box>().MoveOrDestroy(false);
            }
            if (enemy.name.Contains("Shieldman"))
            {
                enemy.GetComponent<IEnemy>().TakeDamage(attackDamageAxe, true);
            }
            else
            {
                Debug.Log("We hit " + enemy + " with an axe");
                if (enemy.GetComponent<IEnemy>() != null)
                {
                    enemy.GetComponent<IEnemy>().TakeDamage(attackDamage, null);
                }
            }
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
            yield return new WaitForSeconds(waitingDuration - 0.15f);
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
            yield return new WaitForSeconds(waitingDuration - 0.15f);
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
            yield return new WaitForSeconds(waitingDuration - 0.15f);
            movementSpeed = movementSpeedHelper;
            attackState = AttackState.notAttacking;
        }
        else
        {
            yield return new WaitForSeconds(waitingDuration - 0.15f);
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

    public void Teleport(InputAction.CallbackContext callbackContext)
    {
        if (location && callbackContext.performed)
        {
            location = false;
            transform.position = location2.position;
        }
        else if (!location && callbackContext.performed)
        {
            location = true;
            transform.position = location1.position;
        }
    }

    public void SpawnHeavy(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + 10, transform.position.y + 5, 0);
            Quaternion spawnRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            Instantiate(heavyPrefab, spawnPosition, spawnRotation);
        }
    }

    public void SpawnShieldman(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + 10, transform.position.y + 5, 0);
            Quaternion spawnRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            Instantiate(shieldmanPrefab, spawnPosition, spawnRotation);
        }
    }

    public void SpawnMetalBox(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + 10, transform.position.y + 5, 0);
            Quaternion spawnRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            Instantiate(metalBox, spawnPosition, spawnRotation);
        }
    }

    public void SpawnWoodenBox(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + 10, transform.position.y + 5, 0);
            Quaternion spawnRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            Instantiate(woodenBox, spawnPosition, spawnRotation);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }
        Gizmos.DrawWireCube(groundCheck.position, groundCheckRange);
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
