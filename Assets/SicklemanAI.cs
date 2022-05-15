using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SicklemanAI : MonoBehaviour
{
    //Utilities
    System.Random rnd = new System.Random();
    public int randomNumber = 0;
    int myID;
    string myName = "";
    Sickleman sickleman;
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
    float speed;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    BoxCollider2D boxCollider2DKarasu;
    CircleCollider2D karasuParryCollider;

    //Combat system
    //Attack decision
    public enum AttackDecision
    {
        basic,
        scream,
        stomp,
        teleportStrike,
        none
    }
    public AttackDecision attackDecision = new AttackDecision();
    public float decisionTimer = 2.5f;

    //Attacks
    public bool currentlyAttacking = false;
    float lastTimeAttack = 0f;
    public bool currentlyDashing = false;

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
    const string STAGGERANIMATION = "stagger";
    const string STAGGERNOSHIELDANIMATION = "staggerNoShield";

    private void Awake()
    {
        //Self references and initializations
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        sickleman = GetComponent<Sickleman>();
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
        myName = "SicklemanEnemy" + myID + "_SpawnLocation";
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
        //Exceptions
        if (sickleman.isDead)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            AnimatorSwitchState(DEATHANIMATION);
            return;
        }
        if (KarasuEntity.dead)
        {
            return;
        }

        //Movement
        hDistance = Mathf.Abs(transform.position.x - karasuTransform.position.x);
        vDistance = Mathf.Abs(transform.position.y - karasuTransform.position.y);
        spawnHorizontalDistance = Mathf.Abs(transform.position.x - spawn.transform.position.x);

        if (hDistance > stoppingDistance)
        {
            rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
        }

        if (currentTarget == karasuTransform)
        {
            CalculateDirection(hDistance);
        }
        else
        {
            CalculateDirection(spawnHorizontalDistance);
        }

        //Attacking
        if (hDistance < stoppingDistance && Time.time > lastTimeAttack + decisionTimer && !currentlyAttacking)
        {
            currentlyAttacking = true;
        }


        //Animations
        speed = Mathf.Abs(rigidBody2D.velocity.x);

        if (!currentlyAttacking && !sickleman.isTakingDamage)
        {
            if (speed > 1)
            {
                AnimatorSwitchState(WALKNOSHIELDANIMATION);
            }
            else
            {
                AnimatorSwitchState(IDLENOSHIELDANIMATION);
            }
        }

        //Karasu parry collider needs to be ignored repeatedly because it is getting disabled and enabled multiple times
        if (currentTarget == karasuTransform)
        {
            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
        }
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
                    direction = -1;
                    if (!facingLeft && !currentlyAttacking)
                    {
                        Flip();
                        healthBar.Flip();
                    }
                }
                else if (transform.position.x < currentTarget.position.x)
                {
                    direction = 1;
                    if (facingLeft && !currentlyAttacking)
                    {
                        Flip();
                        healthBar.Flip();
                    }
                }
                return;
            }
            direction = 0;
        }
    }

    //Combat system
    void BasicAttack()
    {
        attackDecision = AttackDecision.basic;
        lastTimeAttack = Time.time;
        AnimatorSwitchState(BASICATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void ScreamAttack()
    {
        attackDecision = AttackDecision.scream;
        lastTimeAttack = Time.time;
        AnimatorSwitchState(SHIELDBASHANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void StompAttack()
    {
        attackDecision = AttackDecision.stomp;
        lastTimeAttack = Time.time;
        AnimatorSwitchState(ATTACKFLURRYANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void TeleportStrikeAttack()
    {
        attackDecision = AttackDecision.teleportStrike;
        lastTimeAttack = Time.time;
        AnimatorSwitchState(DASHATTACKANIMATION);
    }

    //Utilities
    IEnumerator StopMovingWhileAttacking()
    {
        movementSpeed = 0;
        yield return new WaitForSeconds(1f);
        movementSpeed = movementSpeedHelper;
        currentlyAttacking = false;
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
            sickleman.currentHealth = sickleman.maxHealth;
            healthBar.SetHealth(sickleman.currentHealth);
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
