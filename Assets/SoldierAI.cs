using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoldierAI : MonoBehaviour
{
    //Utilities
    System.Random rnd = new System.Random();
    static SoldierAI soldierAI;

    //Animation control
    Animator animator;
    public Animator karasuAnimator;
    KarasuEntity karasuEntity;
    PlayerControl playerControl;

    //Ground check
    Transform groundCheckSoldier;
    float groundCheckSoldierRadius = 0.2f;
    public LayerMask whatIsGround;
    public Transform isThereGroundAhead;
    public float isThereGroundAheadRange = 0.25f;
    bool onPlatform;
    bool onGround;

    //Pathseeking
    private Rigidbody2D rigidBody2D;
    public Transform target;

    //movement
    float movementSpeed = 150f;
    float movementSpeedHelper;
    readonly float stoppingDistance = 0.75f;
    readonly float flipDistance = 0.2f;
    bool facingLeft = true;
    bool grounded = true;
    int direction = 0;
    float hDistance;
    float vDistance;
    //jumping
    public float jumpForceSoldier = 250f;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    public BoxCollider2D boxCollider2DKarasu;
    public CircleCollider2D karasuParryCollider;
    public CircleCollider2D karasuBlockCollider;

    //Combat system
    public LayerMask enemiesLayers;
    public LayerMask parryLayer;
    public LayerMask blockLayer;
    public float nextGlobalAttackSoldier = 0f;
    public bool currentlyAttacking = false;
    //Soldier basic attack
    public Transform swordColliderSoldier;
    public int numberOfAttacks = 0;
    float swordRangeSoldier = 0.5f;
    int attackDamageSoldier = 3;
    float attackSpeedSoldier = 0.75f;
    public float nextAttackTimeSoldier = 0f;
    public float lastTimeAttack = 0f;
    //Parry and block system for Player
    public bool parriedOrBlocked = false;
    //Soldier block
    public Transform blockCollider;
    public GameObject blockColliderGO;
    float blockColliderRange;
    public bool currentlyBlocking = false;
    public int blockChance;

    private void Awake()
    {
        karasuEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<KarasuEntity>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        animator = GetComponent<Animator>();
        soldierAI = this;

        groundCheckSoldier = transform.Find("GroundCheckSoldier");
    }

    // Start is called before the first frame update
    void Start()
    {
        movementSpeedHelper = movementSpeed;
        rigidBody2D = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
        Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
        InvokeRepeating("GenerateRandomNumber", 0f, 0.2f);
    }

    private void FixedUpdate()
    {
        //Exceptions
        if (Soldier.takingDamage || Soldier.soldierDead || karasuEntity.dead)
        {
            return;
        }

        Collider2D[] isThereGroundAheadCheck = Physics2D.OverlapCircleAll(isThereGroundAhead.position, isThereGroundAheadRange, whatIsGround);
        foreach (Collider2D collider in isThereGroundAheadCheck)
        {
            //if the target is too high up for the soldier, soldier should throw a rock at them and go mad
            if (onPlatform && target.position.y > rigidBody2D.position.y && !SoldierSight.platformInJumpRange)
            {
                //go mad
                return;
            }
            //if the target is directly beneath the soldier while the soldier is on a platform, next if teaches him how to come down and fight like a man
            if (onPlatform && target.position.y < rigidBody2D.position.y && Enumerable.Range((int)rigidBody2D.position.x - 1, (int)rigidBody2D.position.x + 1).Contains((int)target.position.x))
            {
                //go down from the platform
            }
            if (collider.name == "PlatformsTilemap")
            {
                onGround = false;
                onPlatform = true;
            }
            else if (collider.name == "GroundTilemap")
            {
                onPlatform = false;
                onGround = true;
            }
        }
        //if (isThereGroundAheadCheck.Length < 0)
        //{
        //    if (grounded && !SoldierSight.platformInJumpRange)
        //    {
        //        return;
        //    }
            
        //}

        grounded = false;

        //colliders -> check to see if the player is currently on the ground
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
            SoldierSight.jumpCounterSoldier = 0;
        }

        //Movement
        hDistance = Mathf.Abs(transform.position.x - target.position.x);
        vDistance = Mathf.Abs(transform.position.y - target.position.y);

        //keep moving towards the target
        //stopping distance from the target so the soldier won't try to go directly inside of them
        if (hDistance > stoppingDistance)
        {
            if (transform.position.x > target.position.x)
            {
                if (!facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
                direction = -1;
            }
            else if (transform.position.x < target.position.x)
            {
                if (facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
                direction = 1;
            }
        }
        //if the target is within stopping distance, but the soldier is turned the opposite way, flip him
        else
        {
            if (hDistance > flipDistance)
            {
                if (transform.position.x > target.position.x && !facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
                else if (transform.position.x < target.position.x && facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
            }
            direction = 0;
        }

        //acutal movement
        rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);

        //Blocking interaction
        if (PlayerControl.currentlyAttacking && !currentlyBlocking && !currentlyAttacking && hDistance < stoppingDistance && vDistance < stoppingDistance && !Soldier.takingDamage)
        {
            currentlyBlocking = true;
            SoldierBlock();
        }
        //Attacking
        if (hDistance < stoppingDistance && vDistance < stoppingDistance && Time.time >= soldierAI.nextAttackTimeSoldier && Time.time >= soldierAI.nextGlobalAttackSoldier
        && soldierAI.numberOfAttacks == 0 && !soldierAI.currentlyBlocking && !soldierAI.currentlyAttacking && !Soldier.takingDamage)
        {
            currentlyAttacking = true;
            Attack();
        }
        //If the target hits the soldier while he is winding up an attack, the soldier gets confused, so we gotta set his attack conditions manually
        //if the soldier hasn't attacked within 1.75 seconds, he's probably stuck and needs some help
        if (Time.time > lastTimeAttack + 1.75)
        {
            ManuallySetAttackConditions();
        }
        //animations
        animator.SetFloat("animSoldierSpeed", Math.Abs(rigidBody2D.velocity.x));
        animator.SetFloat("animSoldiervSpeed", Math.Abs(rigidBody2D.velocity.y));
        animator.SetBool("animSoldierGrounded", grounded);
        //karasu parry and block colliders need to be ignored repeatedly because they're getting disable and enable multiple times
        Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
        Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
    }

    //Movement
    public static void Jump()
    {
        if (soldierAI.grounded && SoldierSight.jumpCounterSoldier == 0 && !soldierAI.currentlyAttacking && !soldierAI.currentlyBlocking )
        {
            //if the soldier's target is out of range vertically, but not horizontally and if soldier detects a platform ahead of him
            //he will jump repeatedly. This stops him from doing that
            if (soldierAI.vDistance > 1 && soldierAI.hDistance < 1)
            {
                return;
            }
            //if the target is ahead but on lower ground than soldier, but there is a platform on which the soldier can jump,
            //the soldier will jump on the platform instead of chasing the target. This stops him from doing just that
            if (soldierAI.transform.position.y > soldierAI.target.position.y && soldierAI.onPlatform)
            {
                return;
            }
            SoldierSight.jumpCounterSoldier++;
            soldierAI.rigidBody2D.velocity = new Vector2 (soldierAI.rigidBody2D.velocity.x, soldierAI.jumpForceSoldier);
        }
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

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
