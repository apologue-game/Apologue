using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanAI : MonoBehaviour
{
    //Utilities
    System.Random rnd = new System.Random();
    int myID;
    string myName = "";
    Spearman spearman;
    public bool staggered = false;
    public float staggerTimer = 0f;
    public float staggerDuration = 0.5f;
    public HealthBar healthBar;


    //Targeting
    GameObject karasu;
    Transform karasuTransform;
    Vector3 spawnLocation;
    Transform currentTarget;
    [HideInInspector]
    public GameObject spawn;

    //Animation control
    Animator animator;
    KarasuEntity karasuEntity;
    PlayerControl playerControl;

    //Pathseeking
    private Rigidbody2D rigidBody2D;

    //Movement
    public float movementSpeed = 150f;
    public float dashSpeed = 50;
    float movementSpeedHelper;
    readonly float stoppingDistance = 1.3f;
    public float flipDistance = 0.12f;
    bool facingLeft = true;
    public int direction = 0;
    public float hDistance;
    public float vDistance;
    float spawnHorizontalDistance;
    public float speed;

    //Ground checking
    public Transform groundCheck;
    public bool grounded;
    public float groundCheckRange;
    public LayerMask whatIsGround;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    BoxCollider2D boxCollider2DKarasu;
    CircleCollider2D karasuParryCollider;

    //Combat system
    //Attack decision
    public enum AttackDecision
    {
        basic,
        dashAttack,
        attackFlurry,
        shieldBash,
        none
    }
    public AttackDecision attackDecision = new AttackDecision();
    public float decisionTimer = 2f;
    public float timeUntilNextDecision = 0f;
    public int basicAttackChance = 0;
    public int lungeAttackChance = 0;
    public int jumpForwardAttackChance = 0;
    public int lungeDownChance = 0;
    public int chooseAttack;
    public bool lungeAttackCheck;
    int higherChanceAttack = 1;
    int lowerChanceAttacks = 0;
    //Attacks
    public LayerMask enemiesLayers;
    float nextGlobalAttack = 0f;
    public bool currentlyAttacking = false;
    float lastTimeAttack = 0f;
    public bool currentlyLunging = false;
    public bool currentlyJumpingForward = false;
    float nextBasicAttack = 0f;
    float nextLungeAttack = 0f;
    float nextJumpForwardAttack = 0f;
    public float fallingSpeed = 0f;

    //Animations manager
    string oldState = "";

    const string IDLEANIMATION = "idle";
    const string WALKANIMATION = "walk";
    const string DEATHANIMATION = "death";
    const string IDLENOSHIELDANIMATION = "idleNoShield";
    const string WALKNOSHIELDANIMATION = "walkNoShield";
    const string BASICATTACKANIMATION = "basicAttack";
    const string DASHATTACKANIMATION = "dashAttack";
    const string ATTACKFLURRYANIMATION = "attackFlurry";
    const string SHIELDBASHANIMATION = "shieldBash";
    const string SHIELDBLOCKANIMATION = "shieldBlock";
    const string SHIELDDESTROYEDANIMATION = "shieldDestroyed";

    private void Awake()
    {
        //Self references and initializations
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spearman = GetComponent<Spearman>();
    }

    void Start()
    {
        //Neccessary references for targeting
        karasu = GameObject.FindGameObjectWithTag("Player");
        karasuEntity = karasu.GetComponent<KarasuEntity>();
        playerControl = karasu.GetComponent<PlayerControl>();
        karasuTransform = karasu.transform;

        //Ignore collider collisions
        boxCollider2DKarasu = karasu.GetComponent<BoxCollider2D>();
        karasuParryCollider = karasu.transform.Find("ParryCollider").GetComponent<CircleCollider2D>();

        //Spawn location references
        myID = GameMaster.enemyID++;
        myName = "SpearmanEnemy" + myID + "_SpawnLocation";
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawn = new GameObject(myName);
        spawn.transform.position = spawnLocation;
        currentTarget = spawn.transform;

        attackDecision = AttackDecision.none;
        movementSpeedHelper = movementSpeed;
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        InvokeRepeating(nameof(InCombatOrGoBackToSpawn), 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        grounded = false;
        //Colliders->check to see if the player is currently on the ground
        Collider2D[] collidersGround = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRange, whatIsGround);
        for (int i = 0; i < collidersGround.Length; i++)
        {
            if (collidersGround[i].name == "PlatformsTilemap" || collidersGround[i].name == "GroundTilemap")
            {
                grounded = true;
            }
        }
        //Exceptions
        if (spearman.isDead)
        {
            if (grounded)
            {
                rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                AnimatorSwitchState(DEATHANIMATION);
            }
            return;
        }
        if (KarasuEntity.dead)
        {
            return;
        }

        //if (staggered)
        //{
        //    AnimatorSwitchState(STAGGERANIMATION);
        //    if (Time.time < staggerTimer)
        //    {
        //        return;
        //    }
        //    staggered = false;
        //}

        if (!currentlyAttacking)
        {
            currentlyLunging = false;
        }

        //Movement
        hDistance = Mathf.Abs(transform.position.x - karasuTransform.position.x);
        vDistance = Mathf.Abs(transform.position.y - karasuTransform.position.y);
        spawnHorizontalDistance = Mathf.Abs(transform.position.x - spawn.transform.position.x);

        if (currentTarget == karasuTransform)
        {
            CalculateDirection(hDistance);
        }
        else
        {
            CalculateDirection(spawnHorizontalDistance);
        }


        if (currentlyJumpingForward)
        {
            if (grounded)
            {

            }
            else if (!grounded)
            {
                rigidBody2D.velocity = new Vector2(direction * fallingSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            }
        }

        if (!currentlyLunging && !currentlyJumpingForward)
        {
            if (direction == -1 && !facingLeft)
            {
                Flip();
                healthBar.Flip();
            }
            if (direction == 1 && facingLeft)
            {
                Flip();
                healthBar.Flip();
            }
        }

        //Attacking

        //Only decide on attacks if no decision has yet been made
        if (attackDecision == AttackDecision.none && Time.time > timeUntilNextDecision && !currentlyAttacking && Time.time > nextGlobalAttack)
        {
            CalculateDecision();
        }
        //If spearman decided on using the basic attack, walk up to the target and attack
        //If spearman decided on using the lunge attack, dash forward a fixed distance while attacking
        //If spearman decided on using the jump forward attack, jump to target's last location and attack
        if (attackDecision == AttackDecision.basic)
        {
            if (attackDecision == AttackDecision.none)
            {
                return;
            }
            rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            if (hDistance < stoppingDistance && !currentlyAttacking && !spearman.isTakingDamage && currentTarget == karasuTransform)
            {
                currentlyAttacking = true;
                BasicAttack();
            }
        }
        else if (attackDecision == AttackDecision.dashAttack)
        {
            if (!currentlyAttacking && !spearman.isTakingDamage && currentTarget == karasuTransform)
            {
                CalculateDirection(hDistance);
                currentlyAttacking = true;
            }
        }
        else if (attackDecision == AttackDecision.attackFlurry)
        {
            if (!currentlyAttacking && !spearman.isTakingDamage && currentTarget == karasuTransform)
            {
                currentlyAttacking = true;
            }
        }
        else if (attackDecision == AttackDecision.shieldBash)
        {
            if (!currentlyAttacking && !spearman.isTakingDamage && currentTarget == karasuTransform)
            {
                currentlyAttacking = true;
            }
        }

        //If the target hits the enemy while he is winding up an attack, the enemy gets confused, so we gotta set their attack conditions manually
        //If the enemy hasn't attacked within 1.75 seconds, they're probably stuck and need some help
        //if (Time.time > lastTimeAttack + 1.75 && currentTarget == karasuTransform)
        //{
        //    ManuallySetAttackConditions();
        //}

        //Animations
        speed = Mathf.Abs(rigidBody2D.velocity.x);

        if (!currentlyAttacking && !spearman.isTakingDamage)
        {
            if (speed > 1)
            {
                AnimatorSwitchState(WALKANIMATION);
            }
            else
            {
                AnimatorSwitchState(IDLEANIMATION);
            }
        }

        //Karasu parry collider needs to be ignored repeatedly because it is getting disabled and enabled multiple times
        if (currentTarget == karasuTransform)
        {
            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
        }
    }

    private void CalculateDecision()
    {
        //Make a decision based on distance -> the most appropriate decision has the highest chance to be the final one
        if (hDistance < stoppingDistance && !currentlyAttacking && !spearman.isTakingDamage && currentTarget == karasuTransform) //Maybe I don't need to check whether the current target is Karasu because if he is not the current target, it means he is dead, so it automatically can be assumed that he is not in attack range. Needs testing.
        {
            basicAttackChance = higherChanceAttack;
            lungeAttackChance = lowerChanceAttacks;
            jumpForwardAttackChance = lowerChanceAttacks;
        }
        lungeAttackCheck = GameMaster.Utilities.IsFloatInRange(stoppingDistance, stoppingDistance + 4, hDistance);
        if (lungeAttackCheck && !currentlyAttacking && !spearman.isTakingDamage && currentTarget == karasuTransform)
        {
            basicAttackChance = lowerChanceAttacks;
            lungeAttackChance = higherChanceAttack;
            jumpForwardAttackChance = lowerChanceAttacks;
        }
        if (hDistance > stoppingDistance + 4 && !currentlyAttacking && !spearman.isTakingDamage && currentTarget == karasuTransform)
        {
            basicAttackChance = lowerChanceAttacks;
            lungeAttackChance = lowerChanceAttacks;
            jumpForwardAttackChance = higherChanceAttack;
        }
        //Make the final decision
        chooseAttack = rnd.Next(0, 100);
        if (basicAttackChance > lungeAttackChance)
        {
            if (chooseAttack <= 50)
            {
                attackDecision = AttackDecision.basic;
            }
            //else if (chooseAttack > 50 && chooseAttack <= 75)
            //{
        //        attackDecision = AttackDecision.dashStrike;
        //    }
        //    else if (chooseAttack > 75 && chooseAttack <= 100)
        //    {
        //        attackDecision = AttackDecision.jumpForward;
        //    }
        //}
        //else if (lungeAttackChance > basicAttackChance)
        //{
        //    if (chooseAttack <= 50)
        //    {
        //        attackDecision = AttackDecision.dashStrike;
        //    }
        //    else if (chooseAttack > 50 && chooseAttack <= 75)
        //    {
        //        attackDecision = AttackDecision.basic;
        //    }
        //    else if (chooseAttack > 75 && chooseAttack <= 100)
        //    {
        //        attackDecision = AttackDecision.jumpForward;
        //    }
        //}
        //else if (jumpForwardAttackChance > basicAttackChance)
        //{
        //    if (chooseAttack <= 50)
        //    {
        //        attackDecision = AttackDecision.jumpForward;
        //    }
        //    else if (chooseAttack > 50 && chooseAttack <= 75)
        //    {
        //        attackDecision = AttackDecision.basic;
        //    }
        //    else if (chooseAttack > 75 && chooseAttack <= 100)
        //    {
        //        attackDecision = AttackDecision.dashStrike;
        //    }
        }
        timeUntilNextDecision = Time.time + decisionTimer;
    }

    //Movement
    void CalculateDirection(float distanceFromTarget)
    {
        //Stopping distance from the target so the enemy won't try to go directly inside of them
        if (distanceFromTarget > stoppingDistance)
        {
            if (transform.position.x > currentTarget.position.x)
            {
                if (!facingLeft && !currentlyAttacking)
                {
                    Flip();
                    healthBar.Flip();
                }
                direction = -1;
            }
            else if (transform.position.x < currentTarget.position.x)
            {
                if (facingLeft && !currentlyAttacking)
                {
                    Flip();
                    healthBar.Flip();
                }
                direction = 1;
            }
        }
        //If the target is within stopping distance, but the enemy is turned the opposite way, flip the enemy
        else if (distanceFromTarget < stoppingDistance)
        {
            if (distanceFromTarget > flipDistance)
            {
                if (transform.position.x > currentTarget.position.x)
                {
                    //if (attackDecision == AttackDecision.dashStrike || attackDecision != AttackDecision.jumpForward)
                    //{
                    //    direction = -1;
                    //}
                    if (!facingLeft && !currentlyAttacking)
                    {
                        Flip();
                        healthBar.Flip();
                    }
                }
                else if (transform.position.x < currentTarget.position.x)
                {
                    //if (attackDecision == AttackDecision.dashStrike || attackDecision != AttackDecision.jumpForward)
                    //{
                    //    direction = 1;
                    //}
                    if (facingLeft && !currentlyAttacking)
                    {
                        Flip();
                        healthBar.Flip();
                    }
                }
            }
            //if (attackDecision != AttackDecision.dashStrike || attackDecision != AttackDecision.jumpForward)
            //{
            //    direction = 0;
            //}
        }
    }

    //Combat system
    void BasicAttack()
    {
        lastTimeAttack = Time.time;
        AnimatorSwitchState(BASICATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
        nextBasicAttack = Time.time + 5f;
        nextGlobalAttack = Time.time + 2f;
    }

    //Utilities
    void ManuallySetAttackConditions()
    {
        nextBasicAttack = 0;
        nextLungeAttack = 0;
        nextJumpForwardAttack = 0;
        nextGlobalAttack = 0;
    }

    IEnumerator StopMovingWhileAttacking()
    {
        if (attackDecision == AttackDecision.dashAttack)
        {
            yield return new WaitForSeconds(1.15f);
            currentlyAttacking = false;
            rigidBody2D.velocity = Vector2.zero;
        }
        else if (attackDecision == AttackDecision.shieldBash)
        {
            yield return new WaitForSeconds(0.3f);
            currentlyAttacking = false;
            rigidBody2D.velocity = Vector2.zero;
        }
        else
        {
            movementSpeed = 0;
            yield return new WaitForSeconds(1f);
            movementSpeed = movementSpeedHelper;
            currentlyAttacking = false;
        }
    }

    public void SpearmanParryStagger()
    {
        attackDecision = AttackDecision.none;
        staggered = true;
        staggerTimer = Time.time + staggerDuration;
    }

    void NotAttacking()
    {
        attackDecision = AttackDecision.none;
        currentlyAttacking = false;
    }

    void InCombatOrGoBackToSpawn()
    {
        if (hDistance < 15 && currentTarget != karasuTransform)
        {
            currentTarget = karasuTransform;
        }
        else if (hDistance > 25 && currentTarget != spawn.transform)
        {
            currentTarget = spawn.transform;
            //heal enemy if target gets out of range
            spearman.currentHealth = spearman.maxHealth;
            healthBar.SetHealth(spearman.currentHealth);
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRange);
    }

    //Animations manager
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
