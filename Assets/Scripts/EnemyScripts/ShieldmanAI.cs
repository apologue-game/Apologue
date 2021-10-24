using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldmanAI : MonoBehaviour
{
    //Utilities
    int myID;
    string myName = "";
    Shieldman shieldman;

    //Targeting
    GameObject karasu;
    Transform karasuTransform;
    Vector3 spawnLocation;
    public Transform currentTarget;
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
    readonly float stoppingDistance = 1.2f;
    readonly float flipDistance = 0.2f;
    bool facingLeft = true;
    public int direction = 0;
    public float hDistance;
    public float vDistance;
    float spawnHorizontalDistance;
    public float speed;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    CircleCollider2D karasuParryCollider;
    CircleCollider2D karasuBlockCollider;

    //Combat system
    public LayerMask enemiesLayers;
    public float nextGlobalAttackShieldman = 0f;
    public bool currentlyAttacking = false;
    public int numberOfAttacks = 0;
    public float lastTimeAttack = 0f;
    public float globalAttackCooldown = 0f;
    //Shieldman overhead attack
    public Transform spearAttackShieldman;
    //public float spearAttackShieldmanRange = 0.5f;
    public Vector3 spearAttackShieldmanRange;
    public int attackDamageSpearAttackShieldman = 3;
    float attackSpeedSpearAttackShieldman = 0.75f;
    float nextspearAttackShieldman = 0f;
    //Parry and block system for Player
    bool parriedOrBlocked = false;

    //Animations manager
    string oldState = "";

    const string IDLEANIMATION = "idleAnimation";
    const string RUNANIMATION = "runAnimation";
    const string ATTACKANIMATION = "attackAnimation";
    const string IDLEANIMATIONNOSHIELD = "idleAnimationNoShield";
    const string RUNANIMATIONNOSHIELD = "runAnimationNoShield";
    const string ATTACKANIMATIONNOSHIELD = "attackAnimationNoShield";

    private void Awake()
    {
        //Neccessary references for targeting
        karasu = GameObject.FindGameObjectWithTag("Player");
        karasuEntity = karasu.GetComponent<KarasuEntity>();
        playerControl = karasu.GetComponent<PlayerControl>();
        karasuTransform = karasu.transform;

        //Self references and initializations
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        shieldman = GetComponent<Shieldman>();

        //Ignore collider collisions
        karasuParryCollider = karasu.transform.Find("ParryCollider").GetComponent<CircleCollider2D>();
        karasuBlockCollider = karasu.transform.Find("BlockCollider").GetComponent<CircleCollider2D>();

        //Spawn location references
        myID = GameMaster.enemyID++;
        myName = "Shieldman" + myID + "_SpawnLocation";
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawn = new GameObject(myName);
        spawn.transform.position = spawnLocation;
        currentTarget = spawn.transform;
    }

    void Start()
    {
        movementSpeedHelper = movementSpeed;
        InvokeRepeating("InCombatOrGoBackToSpawn", 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        //Exceptions
        if (shieldman.isDead)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        if (shieldman.isTakingDamage || karasuEntity.dead || shieldman.isBlocking)
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
        if (hDistance < stoppingDistance && vDistance < 3 && Time.time >= nextspearAttackShieldman && Time.time >= nextGlobalAttackShieldman
        && numberOfAttacks == 0 && !currentlyAttacking && !shieldman.isTakingDamage && currentTarget == karasuTransform)
        {
            currentlyAttacking = true;
            Attack();
        }
        //If the target hits the enemy while they're winding up an attack, the enemy gets confused, so we gotta set their attack conditions manually
        //If the enemy hasn't attacked within 1.75 seconds, they're probably stuck and need some help
        if (Time.time > lastTimeAttack + 1.75 && currentTarget == karasuTransform)
        {
            ManuallySetAttackConditions();
        }

        //Animations
        speed = Mathf.Abs(rigidBody2D.velocity.x);
        if (!shieldman.shieldBroken && !currentlyAttacking && !shieldman.isBlocking)
        {
            if (speed > 0)
            {
                AnimatorSwitchState(RUNANIMATION);
            }
            else
            {
                AnimatorSwitchState(IDLEANIMATION);
            }

        }
        else if(shieldman.shieldBroken && !currentlyAttacking && !shieldman.isBlocking)
        {
            if (speed > 0)
            {
                AnimatorSwitchState(RUNANIMATIONNOSHIELD);
            }
            else
            {
                AnimatorSwitchState(IDLEANIMATIONNOSHIELD);
            }

        }
        //Karasu parry and block colliders need to be ignored repeatedly because they're getting disabled and enabled multiple times
        if (currentTarget == karasuTransform)
        {
            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
            Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
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
    void Attack()
    {
        if (shieldman.shieldBroken)
        {
            Debug.Log("no");
            lastTimeAttack = Time.time;
            numberOfAttacks++;
            AnimatorSwitchState(ATTACKANIMATIONNOSHIELD);
            StartCoroutine(StopMovingWhileAttacking());
        }
        else
        {
            lastTimeAttack = Time.time;
            numberOfAttacks++;
            AnimatorSwitchState(ATTACKANIMATION);
            StartCoroutine(StopMovingWhileAttacking());
        }
    }

    void ShieldmanAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(spearAttackShieldman.position, spearAttackShieldmanRange, 0, enemiesLayers);
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
                Debug.Log("Shieldman hit " + enemy + " with a sword");
                enemy.GetComponent<KarasuEntity>().TakeDamage(attackDamageSpearAttackShieldman);
            }
        }
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextspearAttackShieldman = Time.time + 1f / attackSpeedSpearAttackShieldman;
        nextGlobalAttackShieldman = Time.time + 2f;
    }

    //Utilities
    void ManuallySetAttackConditions()
    {
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextspearAttackShieldman = 0;
        nextGlobalAttackShieldman = 0;
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
        yield return new WaitForSeconds(0.6f);
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
            shieldman.currentHealth = shieldman.maxHealth;
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
        if (spearAttackShieldman == null)
        {
            return;
        }
        Gizmos.DrawWireCube(spearAttackShieldman.position, spearAttackShieldmanRange);
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
