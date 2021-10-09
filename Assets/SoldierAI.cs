using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SoldierAI : MonoBehaviour
{
    //Utilities
    System.Random rnd = new System.Random();
    SoldierAI soldierAI;
    int myID;
    string myName = "";
    Soldier soldier;

    //Targeting
    GameObject karasu;
    Transform karasuTransform;
    Vector3 spawnLocation;
    public Transform currentTarget;
    public GameObject spawn;

    //Animation control
    Animator animator;
    KarasuEntity karasuEntity;
    PlayerControl playerControl;

    //Ground check
    Transform groundCheckSoldier;
    float groundCheckSoldierRadius = 0.3f;
    public LayerMask whatIsGround;
    public Transform isThereGroundAhead;
    public float isThereGroundAheadRange = 0.25f;
    public bool goingDownFromAPlatform = false;
    public bool onPlatform;
    public bool onGround;

    //Pathseeking
    private Rigidbody2D rigidBody2D;
    SoldierSight soldierSight;

    //Tilemaps
    Tilemap platformsTilemap;
    //Tilemap groundTilemap; -> maybe i will need this
    public int leftCounter = 0;
    public int rightCounter = 0;

    //Movement
    float movementSpeed = 150f;
    float movementSpeedHelper;
    readonly float stoppingDistance = 0.75f;
    readonly float flipDistance = 0.2f;
    bool facingLeft = true;
    bool grounded = true;
    public int direction = 0;
    public int goingDownDirection;
    public float hDistance;
    public float vDistance;
    //Jumping
    public float jumpForceSoldier = 250f;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    BoxCollider2D boxCollider2DKarasu;
    CircleCollider2D karasuParryCollider;
    CircleCollider2D karasuBlockCollider;

    //Combat system
    public LayerMask enemiesLayers;
    public LayerMask parryLayer;
    public LayerMask blockLayer;
    float nextGlobalAttackSoldier = 0f;
    bool currentlyAttacking = false;
    //Soldier basic attack
    public Transform swordColliderSoldier;
    int numberOfAttacks = 0;
    float swordRangeSoldier = 0.5f;
    int attackDamageSoldier = 3;
    float attackSpeedSoldier = 0.75f;
    float nextAttackTimeSoldier = 0f;
    float lastTimeAttack = 0f;
    //Parry and block system for Player
    bool parriedOrBlocked = false;
    //Soldier block
    public Transform blockCollider;
    public GameObject blockColliderGO;
    float blockColliderRange;
    bool currentlyBlocking = false;
    int blockChance;

    private void Awake()
    {
        //Neccessary references for targeting
        karasu = GameObject.FindGameObjectWithTag("Player");
        karasuEntity = karasu.GetComponent<KarasuEntity>();
        playerControl = karasu.GetComponent<PlayerControl>();
        karasuTransform = karasu.transform;

        //Own references and initializations
        animator = GetComponent<Animator>();
        soldierAI = this;
        soldier = GetComponent<Soldier>();
        soldierSight = GetComponentInChildren<SoldierSight>();

        //Ignore collider collisions
        boxCollider2DKarasu = karasu.GetComponent<BoxCollider2D>();
        karasuParryCollider = karasu.transform.Find("ParryCollider").GetComponent<CircleCollider2D>();
        karasuBlockCollider = karasu.transform.Find("BlockCollider").GetComponent<CircleCollider2D>();

        //Spawn location references
        myID = GameMaster.enemyID++;
        myName = "Soldier" + myID + "_SpawnLocation";
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawn = new GameObject(myName);
        spawn.transform.position = spawnLocation;
        currentTarget = spawn.transform;

        platformsTilemap = GameObject.Find("PlatformsTilemap").GetComponent<Tilemap>();
        //groundTilemap = GameObject.Find("GroundTilemap").GetComponent<Tilemap>();

        groundCheckSoldier = transform.Find("GroundCheckSoldier");
    }
    void Start()
    {
        movementSpeedHelper = movementSpeed;
        rigidBody2D = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
        Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
        InvokeRepeating("GenerateRandomNumber", 0f, 0.2f);
        InvokeRepeating("InCombatOrGoBackToSpawn", 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        //Exceptions
        if (soldier.takingDamage || soldier.soldierDead || karasuEntity.dead)
        {
            return;
        }


        grounded = false;

        //Colliders -> check to see if the soldier is currently on the ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckSoldier.position, groundCheckSoldierRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }
        }

        if (grounded)
        {
            soldierSight.jumpCounterSoldier = 0;
        }

        //Movement
        hDistance = Mathf.Abs(transform.position.x - karasuTransform.position.x);
        vDistance = Mathf.Abs(transform.position.y - karasuTransform.position.y);
        float spawnHorizontalDistance = Mathf.Abs(transform.position.x - spawn.transform.position.x);

        //Keep moving towards the target
        //Stopping distance from the target so the soldier won't try to go directly inside of them
        if (currentTarget == karasuTransform)
        {
            if (hDistance > stoppingDistance)
            {
                if (transform.position.x > currentTarget.position.x)
                {
                    if (!facingLeft && !currentlyAttacking)
                    {
                        Flip();
                    }
                    direction = -1;
                    if (goingDownFromAPlatform && goingDownDirection == 0)
                    {
                        goingDownDirection = direction;
                    }
                }
                else if (transform.position.x < currentTarget.position.x)
                {
                    if (facingLeft && !currentlyAttacking)
                    {
                        Flip();
                    }
                    direction = 1;
                    if (goingDownFromAPlatform && goingDownDirection == 0)
                    {
                        goingDownDirection = direction;
                    }
                }
            }
            //If the target is within stopping distance, but the soldier is turned the opposite way, flip the soldier
            else
            {
                if (hDistance > flipDistance)
                {
                    if (transform.position.x > currentTarget.position.x && !facingLeft && !currentlyAttacking)
                    {
                        Flip();
                        if (goingDownFromAPlatform && goingDownDirection == 0)
                        {
                            goingDownDirection = direction;
                        }
                    }
                    else if (transform.position.x < currentTarget.position.x && facingLeft && !currentlyAttacking)
                    {
                        Flip(); 
                        if (goingDownFromAPlatform && goingDownDirection == 0)
                        {
                            goingDownDirection = direction;
                        }
                    }
                }
                if (!goingDownFromAPlatform)
                {
                    direction = 0;
                }
            }
        }
        else
        {
            if (spawnHorizontalDistance > stoppingDistance)
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
            //If the target is within stopping distance, but the soldier is turned the opposite way, flip the soldier
            else
            {
                if (spawnHorizontalDistance > flipDistance)
                {
                    if (!facingLeft && grounded)
                    {
                        Flip();
                    }
                }
                direction = 0;
            }
        }

        if (currentTarget == karasuTransform)
        {
            Collider2D[] isThereGroundAheadCheck = Physics2D.OverlapCircleAll(isThereGroundAhead.position, isThereGroundAheadRange, whatIsGround);
            foreach (Collider2D collider in isThereGroundAheadCheck)
            {
                if (collider.name == "PlatformsTilemap")
                {
                    onGround = false;
                    onPlatform = true;
                }
                else if (collider.name == "GroundTilemap")
                {
                    onPlatform = false;
                    goingDownFromAPlatform = false;
                    onGround = true;
                }
            }
            //If the target is too high up for the soldier, soldier should throw a rock at them and go mad
            //if (SoldierSight.iDontWantToFightAnymoreCounter > 10 && !IsFloatInRange(rigidBody2D.position.y - 0.3f, rigidBody2D.position.y + 1.5f, target.position.y))
            //{
            //    Debug.Log("Going mad"); 
            //    //Go mad

            //    //Throw a rock
            //    return;
            //}
            //If the target is directly beneath the soldier while the soldier is on a platform, he will come down and fight like a man
            if (onPlatform && vDistance > 1 && hDistance < 2 && transform.position.y > karasuTransform.position.y)
            {
                //Go down from the platform
                if (!goingDownFromAPlatform)
                {
                    goingDownFromAPlatform = true;
                    goingDownDirection = 0;
                } 
            }
            if (goingDownFromAPlatform)
            {
                GoDownFromThePlatform();
                animator.SetFloat("animSoldierSpeed", Math.Abs(rigidBody2D.velocity.x));
                animator.SetFloat("animSoldiervSpeed", Math.Abs(rigidBody2D.velocity.y));
                animator.SetBool("animSoldierGrounded", grounded);
                if (isThereGroundAheadCheck.Length == 0)
                {
                    StartCoroutine(GoingDown());
                }
                return;
            }
        }

        //Actual movement
        rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);

        //Blocking interaction
        if (PlayerControl.currentlyAttacking && !currentlyBlocking && !currentlyAttacking && hDistance < stoppingDistance && vDistance < stoppingDistance && !soldier.takingDamage)
        {
            currentlyBlocking = true;
            SoldierBlock();
        }
        //Attacking
        if (hDistance < stoppingDistance && vDistance < stoppingDistance && Time.time >= soldierAI.nextAttackTimeSoldier && Time.time >= soldierAI.nextGlobalAttackSoldier
        && soldierAI.numberOfAttacks == 0 && !soldierAI.currentlyBlocking && !soldierAI.currentlyAttacking && !soldier.takingDamage && currentTarget == karasuTransform)
        {
            currentlyAttacking = true;
            Attack();
        }
        //If the target hits the soldier while he is winding up an attack, the soldier gets confused, so we gotta set his attack conditions manually
        //If the soldier hasn't attacked within 1.75 seconds, he's probably stuck and needs some help
        if (Time.time > lastTimeAttack + 1.75 && currentTarget == karasuTransform)
        {
            ManuallySetAttackConditions();
        }
        //Animations
        animator.SetFloat("animSoldierSpeed", Math.Abs(rigidBody2D.velocity.x));
        animator.SetFloat("animSoldiervSpeed", Math.Abs(rigidBody2D.velocity.y));
        animator.SetBool("animSoldierGrounded", grounded);
        //Karasu parry and block colliders need to be ignored repeatedly because they're getting disabled and enabled multiple times
        if (currentTarget == karasuTransform)
        {
            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
            Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
        }
    }

    //Movement
    public void Jump()
    {
        if (grounded && soldierSight.jumpCounterSoldier == 0 && !soldierAI.currentlyAttacking && !soldierAI.currentlyBlocking )
        {
            //If the soldier's target is out of range vertically, but not horizontally and if soldier detects a platform ahead of him
            //He will jump repeatedly. This stops him from doing that
            if (soldierAI.vDistance > 1 && soldierAI.hDistance < 1)
            {
                return;
            }
            //If the target is ahead but on lower ground than soldier, but there is a platform on which the soldier can jump,
            //The soldier will jump on the platform instead of chasing the target. This stops him from doing just that
            if (soldierAI.transform.position.y > soldierAI.currentTarget.position.y && soldierAI.onPlatform)
            {
                return;
            }
            soldierSight.jumpCounterSoldier++;
            soldierAI.rigidBody2D.velocity = new Vector2 (soldierAI.rigidBody2D.velocity.x, soldierAI.jumpForceSoldier);
        }
    }

    private void GoDownFromThePlatform()
    {
        if (goingDownDirection == -1 && !facingLeft)
        {
            Flip();
        }
        else if (goingDownDirection == 1 && facingLeft)
        {
            Flip();
        }
        rigidBody2D.velocity = new Vector2(goingDownDirection * movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
    }

    private IEnumerator GoingDown()
    {
        yield return new WaitForSeconds(0.5f);
        goingDownFromAPlatform = false;
        StopCoroutine(GoingDown());
    }

    //Combat system
    void Attack()
    {
        lastTimeAttack = Time.time;
        soldierAI.numberOfAttacks++;
        soldierAI.animator.SetTrigger("animSoldierAttack");
        soldierAI.StartCoroutine(soldierAI.StopMovingWhileAttacking());
    }

    void SoldierAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordColliderSoldier.position, swordRangeSoldier, enemiesLayers);
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
                Debug.Log("Soldier hit " + enemy + " with a sword");
                enemy.GetComponent<KarasuEntity>().TakeDamage(attackDamageSoldier);
            }
        }
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextAttackTimeSoldier = Time.time + 1f / attackSpeedSoldier;
        nextGlobalAttackSoldier = Time.time + 1f;
    }

    void SoldierBlock()
    {
        if (blockChance == 1 || blockChance == 5)
        {
            animator.SetTrigger("animSoldierBlock");
            blockColliderGO.SetActive(true);
            StartCoroutine(SoldierBlockWindow());
        }
        else
        {
            currentlyBlocking = false;
        }
    }

    //Utilities
    void ManuallySetAttackConditions()
    {
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextAttackTimeSoldier = 0;
        nextGlobalAttackSoldier = 0;
    }

    void GenerateRandomNumber()
    {
        blockChance = rnd.Next(0, 10);
    }

    IEnumerator BlockedAndHitAnimation()
    {
        playerControl.AnimatorSwitchState("karasuBlockedAndHitAnimation");
        yield return new WaitForSeconds(0);
        playerControl.AnimatorSwitchState("karasuBlockAnimation");
    }

    IEnumerator SoldierBlockWindow()
    {
        yield return new WaitForSeconds(0.4f);
        blockColliderGO.SetActive(false);
        currentlyBlocking = false;
    }

    IEnumerator StopMovingWhileAttacking()
    {
        if (grounded)
        {
            movementSpeed = 0;
            yield return new WaitForSeconds(1f);
            movementSpeed = movementSpeedHelper;
            currentlyAttacking = false;
        }
        else
        {
            rigidBody2D.AddForce(Vector2.down * 250);
            movementSpeed = 0;
            yield return new WaitForSeconds(1f);
            movementSpeed = movementSpeedHelper;
            currentlyAttacking = false;
        }
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
            //heal soldier when he's out of combat
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    static bool IsFloatInRange(float min, float max, float valueToCheck)
    {
        if (valueToCheck >= min && valueToCheck <= max)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (isThereGroundAhead == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(isThereGroundAhead.position, isThereGroundAheadRange);
    }
}
