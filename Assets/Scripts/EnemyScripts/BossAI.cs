using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackSystem;

public class BossAI : MonoBehaviour
{
    //Utilities
    System.Random rnd = new System.Random();
    int myID;
    string myName = "";
    Boss boss;

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
    readonly float flipDistance = 0.2f;
    bool facingLeft = true;
    public int direction = 0;
    public float hDistance;
    public float vDistance;
    float spawnHorizontalDistance;
    public float speed;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    BoxCollider2D boxCollider2DKarasu;
    CircleCollider2D karasuParryCollider;
    CircleCollider2D karasuBlockCollider;

    //Combat system
    //Attack decision
    enum AttackDecision
    {
        basic,
        lunge,
        jumpForward,
        lungeDown,
        none
    }
    AttackDecision attackDecision = new AttackDecision();
    public float decisionTimer = 3f;
    public float timeUntilNextDecision = 0f;
    public int basicAttackChance = 0;
    public int lungeAttackChance = 0;
    public int jumpForwardAttackChance = 0;
    public int lungeDownChance = 0;
    public int chooseAttack;
    public bool lungeAttackCheck;
    public bool usedBasicAttack = false;
    public bool usedLungeAttack = false;
    public bool usedJumpForwardAttack = false;
    //Attacks
    AttackType attackType;
    public LayerMask enemiesLayers;
    float nextGlobalAttack = 0f;
    bool currentlyAttacking = false;
    int numberOfAttacks = 0;
    float lastTimeAttack = 0f;
    //float globalAttackCooldown = 0f;
    //Boss basic attack
    public AttackSystem basicAttack;
    public Transform basicAttackPosition;
    public AttackType basicAttackType = AttackType.normal;
    public Vector3 basicAttackRange;
    int basicAttackDamage = 3;
    float basicAttackSpeed = 0.75f;
    float nextBasicAttack = 0f;
    //Boss lunge attack
    public AttackSystem lungeAttack;
    public Transform lungeAttackPosition;
    public AttackType lungeAttackType = AttackType.normal;
    public Vector3 lungeAttackRange;
    int lungeAttackDamage = 3;
    float lungeAttackSpeed = 0.75f;
    float nextLungeAttack = 0f;
    //Boss jump forward attack
    public AttackSystem jumpForwardAttack;
    public Transform jumpForwardAttackPosition;
    public AttackType jumpForwardAttackType = AttackType.onlyParryable;
    public float jumpForwardAttackRange = 0.5f;
    int jumpForwardAttackDamage = 3;
    float jumpForwardAttackSpeed = 0.75f;
    float nextJumpForwardAttack = 0f;
    //Boss lunge down attack
    public AttackSystem lungeDownAttack;
    public Transform lungeDownAttackPosition;
    public AttackType lungeDownAttackType = AttackType.none;
    public Vector3 lungeDownAttackRange;
    int lungeDownAttackDamage = 3;
    float lungeDownAttackSpeed = 0.75f;
    float nextLungeDownAttack = 0f;
    //Boss overhead attack
    public AttackSystem overheadAttack;
    public Transform overheadAttackPosition;
    public AttackType overheadAttackType = AttackType.none;
    public float overheadAttackRange = 0.5f;
    int overheadAttackDamage = 3;
    float overheadAttackSpeed = 0.75f;
    float nextOverheadAttack = 0f;
    bool isKarasuAboveTheBossByX = false;
    bool isKarasuAboveTheBossByY = false;
    //Parry and block system for Player
    bool parriedOrBlocked = false;

    //Animations manager
    string oldState = "";

    const string IDLEANIMATION = "idle";
    const string WALKANIMATION = "walk";
    const string INTROANIMATION = "intro";
    const string ATTACKANIMATION = "attack";
    const string ATTACKPOSEANIMATION = "attackPose";
    const string JUMPFORWARDATTACKANIMATION = "jumpForwardAttack";
    const string OVERHEADATTACKANIMATION = "overheadAttack";
    const string LUNGEANIMATION = "lunge";
    const string LUNGEDOWNANIMATION = "lungeDown";

    private void Awake()
    {
        //Attack types
        basicAttack = new AttackSystem(basicAttackDamage, basicAttackType);
        lungeAttack = new AttackSystem(lungeAttackDamage, lungeAttackType);
        jumpForwardAttack = new AttackSystem(jumpForwardAttackDamage, jumpForwardAttackType);
        lungeDownAttack = new AttackSystem(lungeDownAttackDamage, lungeDownAttackType);
        overheadAttack = new AttackSystem(overheadAttackDamage, overheadAttackType);

        //Neccessary references for targeting
        karasu = GameObject.FindGameObjectWithTag("Player");
        karasuEntity = karasu.GetComponent<KarasuEntity>();
        playerControl = karasu.GetComponent<PlayerControl>();
        karasuTransform = karasu.transform;

        //Self references and initializations
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        boss = GetComponent<Boss>();

        //Ignore collider collisions
        boxCollider2DKarasu = karasu.GetComponent<BoxCollider2D>();
        karasuParryCollider = karasu.transform.Find("ParryCollider").GetComponent<CircleCollider2D>();
        karasuBlockCollider = karasu.transform.Find("BlockCollider").GetComponent<CircleCollider2D>();

        //Spawn location references
        myID = GameMaster.enemyID++;
        myName = "HeavyEnemy" + myID + "_SpawnLocation";
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawn = new GameObject(myName);
        spawn.transform.position = spawnLocation;
        currentTarget = spawn.transform;
    }

    void Start()
    {
        movementSpeedHelper = movementSpeed;
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        InvokeRepeating(nameof(InCombatOrGoBackToSpawn), 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        //Exceptions
        if (boss.isDead)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        if (boss.isTakingDamage || karasuEntity.dead)
        {
            return;
        }

        //Movement
        hDistance = Mathf.Abs(transform.position.x - karasuTransform.position.x);
        vDistance = Mathf.Abs(transform.position.y - karasuTransform.position.y);
        spawnHorizontalDistance = Mathf.Abs(transform.position.x - spawn.transform.position.x);

        //Keep moving towards the target
        //Stopping distance from the target so the enemy won't try to go directly inside of them
        if (currentTarget == karasuTransform)
        {
            CalculateDirection(hDistance);
        }
        else
        {
            CalculateDirection(spawnHorizontalDistance);
        }

        //If boss decided on using the basic attack, walk up to the target and attack
        //If boss decided on using the lunge attack, dash forward while attacking
        //If boss decided on using the jump forward attack, jump to target's location and attack
        if (attackDecision == AttackDecision.basic)
        {
            rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            if (hDistance < stoppingDistance && !currentlyAttacking && !boss.isTakingDamage && currentTarget == karasuTransform)
            {
                currentlyAttacking = true;
                BasicAttack();
            }
        }
        else if (attackDecision == AttackDecision.lunge)
        {
            currentlyAttacking = true;
            LungeAttack();
        }
        else if (attackDecision == AttackDecision.jumpForward)
        {

        }

        //If boss had already used all three of his attacks, there is a ~50% chance that his next one will be the lunge down attack
        //Other attacks are reset after using the lunge down attack
        if (usedBasicAttack && usedLungeAttack && usedJumpForwardAttack)
        {
            lungeDownChance = rnd.Next(0, 10);
            if (lungeDownChance >= 5)
            {
                attackDecision = AttackDecision.lungeDown;
                currentlyAttacking = true;
                LungeDownAttack();

                usedBasicAttack = false;
                usedLungeAttack = false;
                usedJumpForwardAttack = false;
            }
        }

        //Attacking
        //Check whether Karasu is directly above the boss (i.e. trying to jump over him), and if he is, and the boss isn't currently attacking or taking damage, attack him during his jump
        isKarasuAboveTheBossByX = GameMaster.Utilities.IsFloatInRange(transform.position.x - 1f, transform.position.x + 1f, karasuTransform.position.x);
        isKarasuAboveTheBossByY = GameMaster.Utilities.IsFloatInRange(transform.position.y, transform.position.y + 1f, karasuTransform.position.y);
        if (!currentlyAttacking && !boss.isTakingDamage && isKarasuAboveTheBossByY && isKarasuAboveTheBossByX)
        {
            currentlyAttacking = true;
            OverheadAttack();
        }

        //Only decide on attacks if no decision has yet been made
        if (attackDecision == AttackDecision.none)
        {
            CalculateDecision();
        }

        //If the target hits the enemy while he is winding up an attack, the enemy gets confused, so we gotta set their attack conditions manually
        //If the enemy hasn't attacked within 1.75 seconds, they're probably stuck and need some help
        if (Time.time > lastTimeAttack + 1.75 && currentTarget == karasuTransform)
        {
            ManuallySetAttackConditions();
        }

        //Animations
        speed = Mathf.Abs(rigidBody2D.velocity.x);

        if (!currentlyAttacking && !boss.isTakingDamage)
        {
            if (speed > 0)
            {
                AnimatorSwitchState(WALKANIMATION);
            }
            else
            {
                AnimatorSwitchState(IDLEANIMATION);
            }
        }

        //Karasu parry and block colliders need to be ignored repeatedly because they're getting disabled and enabled multiple times
        if (currentTarget == karasuTransform)
        {
            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
            Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
        }
    }

    private void CalculateDecision()
    {
        //Make a decision based on distance -> the most appropriate decision has the highest chance to be the final one
        if (hDistance < stoppingDistance && !currentlyAttacking && !boss.isTakingDamage && currentTarget == karasuTransform) //Maybe I don't need to check whether the current target is Karasu because if he is not the current target, it means he is dead, so it automatically can be assumed that he is not in attack range. Needs testing.
        {
            basicAttackChance = 70;
            lungeAttackChance = 15;
            jumpForwardAttackChance = 15;
        }
        lungeAttackCheck = GameMaster.Utilities.IsFloatInRange(stoppingDistance, stoppingDistance + 4, hDistance);
        if (lungeAttackCheck && !currentlyAttacking && !boss.isTakingDamage && currentTarget == karasuTransform)
        {
            basicAttackChance = 15;
            lungeAttackChance = 70;
            jumpForwardAttackChance = 15;
        }
        if (hDistance > stoppingDistance + 4 && !currentlyAttacking && !boss.isTakingDamage && currentTarget == karasuTransform)
        {
            basicAttackChance = 15;
            lungeAttackChance = 15;
            jumpForwardAttackChance = 70;
        }
        //Make the final decision
        chooseAttack = rnd.Next(0, 100);
        if (basicAttackChance > lungeAttackChance)
        {
            if (chooseAttack <= 70)
            {
                attackDecision = AttackDecision.basic;
                usedBasicAttack = true;
            }
            else if (chooseAttack > 70 && chooseAttack <= 85)
            {
                attackDecision = AttackDecision.lunge;
                usedLungeAttack = true;
            }
            else if (chooseAttack > 85 && chooseAttack <= 100)
            {
                attackDecision = AttackDecision.jumpForward;
                usedJumpForwardAttack = true;
            }
        }
        else if (lungeAttackChance > basicAttackChance)
        {
            if (chooseAttack <= 70)
            {
                attackDecision = AttackDecision.lunge;
                usedLungeAttack = true;
            }
            else if (chooseAttack > 70 && chooseAttack <= 85)
            {
                attackDecision = AttackDecision.basic;
                usedBasicAttack = true;
            }
            else if (chooseAttack > 85 && chooseAttack <= 100)
            {
                attackDecision = AttackDecision.jumpForward;
                usedJumpForwardAttack = true;
            }
        }
        else if (jumpForwardAttackChance > basicAttackChance)
        {
            if (chooseAttack <= 70)
            {
                attackDecision = AttackDecision.jumpForward;
                usedJumpForwardAttack = true;
            }
            else if (chooseAttack > 70 && chooseAttack <= 85)
            {
                attackDecision = AttackDecision.basic;
                usedBasicAttack = true;
            }
            else if (chooseAttack > 85 && chooseAttack <= 100)
            {
                attackDecision = AttackDecision.lunge;
                usedLungeAttack = true;
            }
        }
        timeUntilNextDecision = Time.time + decisionTimer;
    }

    //Movement
    void CalculateDirection(float distanceFromTarget)
    {
        if (distanceFromTarget > stoppingDistance)
        {
            if (transform.position.x > currentTarget.position.x)
            {
                if (!facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
                direction = -1;
            }
            else if (transform.position.x < currentTarget.position.x)
            {
                if (facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
                direction = 1;
            }
        }
        //If the target is within stopping distance, but the enemy is turned the opposite way, flip the enemy
        else
        {
            if (distanceFromTarget > flipDistance)
            {
                if (transform.position.x > currentTarget.position.x && !facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
                else if (transform.position.x < currentTarget.position.x && facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
            }
            direction = 0;
        }
    }

    //Combat system
    void BasicAttack()
    {
        lastTimeAttack = Time.time;
        numberOfAttacks++;
        AnimatorSwitchState(ATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void BossBasicAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(basicAttackPosition.position, basicAttackRange, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "ParryCollider")
            {
                Debug.Log("Successfully parried an attack");
                parriedOrBlocked = true;
            }
            else if (enemy.name == "BlockCollider")
            {
                Debug.Log("Successfully blocked an attack");
                parriedOrBlocked = true;
            }
        }
        if (!parriedOrBlocked)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Heavy hit " + enemy + " with a sideslash attack");
                enemy.GetComponent<KarasuEntity>().TakeDamage(basicAttack.AttackDamage, basicAttack.AttackMake);
            }
        }
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextBasicAttack = Time.time + 5f;
        nextGlobalAttack = Time.time + 2f;
        attackDecision = AttackDecision.none;
    }

    void LungeAttack()
    {
        rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.fixedDeltaTime * dashSpeed, rigidBody2D.velocity.y);
        lastTimeAttack = Time.time;
        numberOfAttacks++;
        AnimatorSwitchState(LUNGEANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void BossLungeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(lungeAttackPosition.position, lungeAttackRange, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "ParryCollider")
            {
                Debug.Log("Successfully parried an attack");
                parriedOrBlocked = true;
            }
            else if (enemy.name == "BlockCollider")
            {
                Debug.Log("Successfully blocked an attack");
                parriedOrBlocked = true;
            }
        }
        if (!parriedOrBlocked)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Heavy hit " + enemy + " with a sideslash attack");
                enemy.GetComponent<KarasuEntity>().TakeDamage(lungeAttack.AttackDamage, lungeAttack.AttackMake);
            }
        }
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextLungeAttack = Time.time + 5f;
        nextGlobalAttack = Time.time + 2f;
        attackDecision = AttackDecision.none;
    }
    void JumpForwardAttack()
    {
        lastTimeAttack = Time.time;
        numberOfAttacks++;
        AnimatorSwitchState(JUMPFORWARDATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void BossJumpForwardAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(jumpForwardAttackPosition.position, jumpForwardAttackRange, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "ParryCollider")
            {
                Debug.Log("Successfully parried an attack");
                parriedOrBlocked = true;
            }
            else if (enemy.name == "BlockCollider")
            {
                Debug.Log("Successfully blocked an attack");
                parriedOrBlocked = true;
            }
        }
        if (!parriedOrBlocked)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Heavy hit " + enemy + " with a sideslash attack");
                enemy.GetComponent<KarasuEntity>().TakeDamage(jumpForwardAttack.AttackDamage, jumpForwardAttack.AttackMake);
            }
        }
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextJumpForwardAttack = Time.time + 5f;
        nextGlobalAttack = Time.time + 2f;
        attackDecision = AttackDecision.none;
    }


    void LungeDownAttack()
    {
        lastTimeAttack = Time.time;
        numberOfAttacks++;
        AnimatorSwitchState(LUNGEDOWNANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void BossLungeDownAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(lungeAttackPosition.position, lungeDownAttackRange, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "ParryCollider")
            {
                Debug.Log("Successfully parried an attack");
                parriedOrBlocked = true;
            }
            else if (enemy.name == "BlockCollider")
            {
                Debug.Log("Successfully blocked an attack");
                parriedOrBlocked = true;
            }
        }
        if (!parriedOrBlocked)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Heavy hit " + enemy + " with a sideslash attack");
                enemy.GetComponent<KarasuEntity>().TakeDamage(lungeDownAttack.AttackDamage, lungeDownAttack.AttackMake);
            }
        }
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextLungeDownAttack = Time.time + 5f;
        nextGlobalAttack = Time.time + 2f;
        attackDecision = AttackDecision.none;
    }
    void OverheadAttack()
    {
        lastTimeAttack = Time.time;
        numberOfAttacks++;
        AnimatorSwitchState(OVERHEADATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void BossOverheadAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(overheadAttackPosition.position, overheadAttackRange, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "ParryCollider")
            {
                Debug.Log("Successfully parried an attack");
                parriedOrBlocked = true;
            }
            else if (enemy.name == "BlockCollider")
            {
                Debug.Log("Successfully blocked an attack");
                parriedOrBlocked = true;
            }
        }
        if (!parriedOrBlocked)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Heavy hit " + enemy + " with an overhead attack");
                attackType = AttackType.none;
                enemy.GetComponent<KarasuEntity>().TakeDamage(overheadAttack.AttackDamage, overheadAttack.AttackMake);
            }
        }
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextOverheadAttack = Time.time + 5f;
        nextGlobalAttack = Time.time + 2f;
    }

    //Utilities
    void ManuallySetAttackConditions()
    {
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextBasicAttack = 0;
        nextLungeAttack = 0;
        nextJumpForwardAttack = 0;
        nextLungeDownAttack = 0;
        nextOverheadAttack = 0;
        nextGlobalAttack = 0;
    }

    IEnumerator BlockedAndHitAnimation()
    {
        playerControl.AnimatorSwitchState("karasuBlockedAndHitAnimation");
        yield return new WaitForSeconds(0);
        playerControl.AnimatorSwitchState("karasuBlockAnimation");
    }

    IEnumerator StopMovingWhileAttacking()
    {
        movementSpeed = 0;
        yield return new WaitForSeconds(1f);
        movementSpeed = movementSpeedHelper;
        currentlyAttacking = false;
    }

    void InCombatOrGoBackToSpawn()
    {
        if (hDistance < 25 && currentTarget != karasuTransform)
        {
            currentTarget = karasuTransform;
        }
        else if (hDistance > 25 && currentTarget != spawn.transform)
        {
            currentTarget = spawn.transform;
            //heal enemy if target gets out of range
            boss.currentHealth = boss.maxHealth;
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
        if (basicAttackPosition == null)
        {
            return;
        }
        Gizmos.DrawWireCube(basicAttackPosition.position, basicAttackRange);
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
