using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackSystem;

public class HeavyEnemyAI : MonoBehaviour
{
    //Utilities
    System.Random rnd = new System.Random();
    int myID;
    string myName = "";
    HeavyEnemy heavyEnemy;
    public HealthBar healthBar;
    public ParticleSystem attackIndicatorGreen;
    public ParticleSystem attackIndicatorRed;
    public ParticleSystem test;

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
    float movementSpeedHelper;
    readonly float stoppingDistance = 1.3f;
    readonly float flipDistance = 0.2f;
    public bool facingLeft = true;
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
    BoxCollider2D karasuSlideCollider;

    //Combat system
    public LayerMask enemiesLayers;
    float nextGlobalAttackHeavyEnemy = 0f;
    bool currentlyAttacking = false;
    int numberOfAttacks = 0;
    float lastTimeAttack = 0f;
    //float globalAttackCooldown = 0f;
    //Heavy enemy overhead attack
    public AttackSystem overHeadAttack;
    public AttackType overHeadAttackType = AttackType.onlyParryable;
    public Transform axeOverheadAttack;
    public float axeOverheadAttackRange = 0.5f;
    int attackDamageOverheadAttack = 4;
    float attackSpeedOverheadAttack = 0.75f;
    float nextOverheadAttackTime = 0f;
    //Heavy enemy sideslash attack
    public AttackSystem sideslashAttack;
    public AttackType sideslashAttackType = AttackType.normal;
    public Transform axeSideslashAttack;
    public float axeSideslashAttackRange = 0.5f;
    int attackDamageSideslashAttack = 3;
    float attackSpeedSideslashAttack = 0.75f;
    float nextSideslashAttackTime = 0f;
    //Parry and block system for Player
    bool parriedOrBlocked = false;

    //Animations manager
    string oldState = "";

    const string IDLEANIMATION = "idleAnimation";
    const string WALKANIMATION = "walkAnimation";
    const string OVERHEADATTACKANIMATION = "overheadAttackAnimation";
    const string SIDESLASHATTACKANIMATION = "sideslashAttackAnimation";


    private void Awake()
    {
        overHeadAttack = new AttackSystem(attackDamageOverheadAttack, AttackType.onlyParryable);
        sideslashAttack = new AttackSystem(attackDamageSideslashAttack, sideslashAttackType);
        //Self references and initializations
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        heavyEnemy = GetComponent<HeavyEnemy>();
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
        karasuBlockCollider = karasu.transform.Find("BlockCollider").GetComponent<CircleCollider2D>();
        karasuSlideCollider = karasu.transform.Find("SlideCollider").GetComponent<BoxCollider2D>();

        //Spawn location references
        myID = GameMaster.enemyID++;
        myName = "HeavyEnemy" + myID + "_SpawnLocation";
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawn = new GameObject(myName);
        spawn.transform.position = spawnLocation;
        currentTarget = spawn.transform;

        movementSpeedHelper = movementSpeed;
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        InvokeRepeating(nameof(InCombatOrGoBackToSpawn), 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        //Exceptions
        if (heavyEnemy.isDead)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        if (heavyEnemy.isTakingDamage || KarasuEntity.dead)
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

        //Actual movement
        rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);

        //Attacking
        if (hDistance < stoppingDistance && vDistance < stoppingDistance && Time.time >= nextGlobalAttackHeavyEnemy
        && numberOfAttacks == 0 && !currentlyAttacking && !heavyEnemy.isTakingDamage && currentTarget == karasuTransform)
        {
            if (Time.time >= nextSideslashAttackTime && Time.time >= nextOverheadAttackTime)
            {
                int chooseAttack = rnd.Next(0, 20);
                if (chooseAttack < 10)
                {
                    currentlyAttacking = true;
                    OverheadAttack();
                }
                else
                {
                    currentlyAttacking = true;
                    SideslashAttack();
                }
            }
            else if (Time.time >= nextSideslashAttackTime || Time.time >= nextOverheadAttackTime)
            {
                if (Time.time >= nextSideslashAttackTime)
                {
                    currentlyAttacking = true;
                    SideslashAttack();
                }
                else
                {
                    currentlyAttacking = true;
                    OverheadAttack();
                }
            }
            
        }
        //If the target hits the enemy while he is winding up an attack, the enemy gets confused, so we gotta set their attack conditions manually
        //If the enemy hasn't attacked within 1.75 seconds, they're probably stuck and need some help
        if (Time.time > lastTimeAttack + 1.75 && currentTarget == karasuTransform)
        {
            ManuallySetAttackConditions();
        }

        //Animations
        speed = Mathf.Abs(rigidBody2D.velocity.x);

        if (!currentlyAttacking && !heavyEnemy.isTakingDamage)
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
            Physics2D.IgnoreCollision(boxCollider2D, karasuSlideCollider);
        }
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
        else
        {
            if (distanceFromTarget > flipDistance)
            {
                if (transform.position.x > currentTarget.position.x && !facingLeft && !currentlyAttacking)
                {
                    Flip();
                    healthBar.Flip();
                }
                else if (transform.position.x < currentTarget.position.x && facingLeft && !currentlyAttacking)
                {
                    Flip();
                    healthBar.Flip();
                }
            }
            direction = 0;
        }
    }

    //Combat system
    void CreateAttackIndicatorGreen()
    {
        attackIndicatorGreen.Play();
    }
    
    void CreateAttackIndicatorRed()
    {
        attackIndicatorRed.Play();
    }

    void CreateTest()
    {
        test.Play();
    }

    void OverheadAttack()
    {
        lastTimeAttack = Time.time;
        numberOfAttacks++;
        AnimatorSwitchState(OVERHEADATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void HeavyEnemyOverheadAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(axeOverheadAttack.position, axeOverheadAttackRange, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "ParryCollider")
            {
                Debug.Log("Successfully parried an attack");
                parriedOrBlocked = true;
            }
        }
        if (!parriedOrBlocked)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Heavy hit " + enemy + " with an overhead attack");
                if (enemy.name == "SlideCollider")
                {
                    enemy.GetComponentInParent<KarasuEntity>().TakeDamage(overHeadAttack.AttackDamage, overHeadAttack.AttackMake);
                }
                else if (enemy.CompareTag("Player"))
                {
                    enemy.GetComponent<KarasuEntity>().TakeDamage(overHeadAttack.AttackDamage, overHeadAttack.AttackMake);
                }             
            }
        }
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextOverheadAttackTime = Time.time + 1f / attackSpeedOverheadAttack;
        nextGlobalAttackHeavyEnemy = Time.time + 2f;
    }

    void SideslashAttack()
    {
        lastTimeAttack = Time.time;
        numberOfAttacks++;
        AnimatorSwitchState(SIDESLASHATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void HeavyEnemySideslashAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(axeSideslashAttack.position, axeSideslashAttackRange, enemiesLayers);
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
                if (enemy.name == "SlideCollider")
                {
                    enemy.GetComponentInParent<KarasuEntity>().TakeDamage(sideslashAttack.AttackDamage, sideslashAttack.AttackMake);
                }
                else
                {
                    enemy.GetComponent<KarasuEntity>().TakeDamage(sideslashAttack.AttackDamage, sideslashAttack.AttackMake);
                }
            }
        }
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextSideslashAttackTime = Time.time + 1f / attackSpeedSideslashAttack;
        nextGlobalAttackHeavyEnemy = Time.time + 2f;
    }

    //Utilities
    void ManuallySetAttackConditions()
    {
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextOverheadAttackTime = 0;
        nextGlobalAttackHeavyEnemy = 0;
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
            heavyEnemy.currentHealth = heavyEnemy.maxHealth;
            healthBar.SetMaximumHealth(heavyEnemy.maxHealth);
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
        if (axeSideslashAttack == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(axeSideslashAttack.position, axeSideslashAttackRange);
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
