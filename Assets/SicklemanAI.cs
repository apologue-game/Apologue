using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SicklemanAI : MonoBehaviour
{
    //Utilities
    //TODO: Centralize random number generation by creating a function in the game master-> utilities class
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
    public Decision basicAttack;
    public Decision screamAttack;
    public Decision stompAttack;
    public Decision teleportAttack;
    List<Decision> decisions;
    public DecisionMaking decisionMaking;
    public Decision currentDecision;
    //Attack decision
    //public enum AttackDecision
    //{
    //    basic,
    //    scream,
    //    stomp,
    //    teleportStrike,
    //    none
    //}
    //public AttackDecision attackDecision = new AttackDecision();
    public float decisionTimer = 0f;

    //Attacks
    public bool currentlyAttacking = false;
    float lastTimeAttack = 0f;
    public bool currentlyDashing = false;

    //Animations manager
    string oldState = "";

    const string IDLEANIMATION = "idle";
    const string WALKANIMATION = "walk";
    const string DEATHANIMATION = "death";
    const string BASICATTACKANIMATION = "basic";
    const string SCREAMATTACKANIMATION = "scream";
    const string STOMPPREPARATIONANIMATION = "stompPreparation";
    const string STOMPFALLINGNIMATION = "stompFalling";
    const string STOMPLANDINGANIMATION = "stompLanding";
    const string SICKLETHROWANIMATION = "sickleThrow";
    const string SICKLETHROWCATCHANIMATION = "sickleThrowCatch";
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
        //New decision system
        basicAttack = new Decision(0, 2f);
        screamAttack = new Decision(1, 7f);
        stompAttack = new Decision(2, 5f);
        teleportAttack = new Decision(3, 10f);
        decisions = new List<Decision>();
        decisions.Add(basicAttack);
        decisions.Add(screamAttack);
        decisions.Add(stompAttack);
        decisions.Add(teleportAttack);
        decisionMaking = new DecisionMaking(decisions);

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

        //attackDecision = AttackDecision.none;
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

        if (currentTarget == karasuTransform)
        {
            CalculateDirection(hDistance);
        }
        else
        {
            CalculateDirection(spawnHorizontalDistance);
        }

        //Attacking
        if (/*attackDecision == AttackDecision.none*/currentDecision == null && Time.time > decisionTimer)
        {
            currentDecision = decisionMaking.DecisionCalculation();
        }
        if (currentDecision != null && !currentlyAttacking)
        {
            if (currentDecision.id == 0)
            {
                //if (hDistance < stoppingDistance && !currentlyAttacking)
                //{
                //    currentlyAttacking = true;
                //    BasicAttack();
                //}
                //else
                //{
                //    rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
                //}
                currentlyAttacking = true;
                BasicAttack();
            }
            else if (currentDecision.id == 1)
            {
                currentlyAttacking = true;
                ScreamAttack();
            }
            else if (currentDecision.id == 2)
            {
                currentlyAttacking = true;
                StompAttack();
            }
            else if (currentDecision.id == 3)
            {
                currentlyAttacking = true;
                TeleportStrikeAttack();
            }

        }

        //Animations
        speed = Mathf.Abs(rigidBody2D.velocity.x);

        if (!currentlyAttacking && !sickleman.isTakingDamage)
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
    //void CalculateDecision()
    //{
    //    randomNumber = rnd.Next(0, 4);
    //    if (randomNumber == 0)
    //    {
    //        attackDecision = AttackDecision.basic;
    //    }
    //    else if (randomNumber == 1)
    //    {
    //        attackDecision = AttackDecision.stomp;
    //    }
    //    else if (randomNumber == 2)
    //    {
    //        attackDecision = AttackDecision.scream;
    //    }
    //    else if (randomNumber == 3)
    //    {
    //        attackDecision = AttackDecision.teleportStrike;
    //    }
    //    currentlyDeciding = false;
    //}

    void BasicAttack()
    {
        Debug.Log("basic");
        basicAttack.currentCooldown = basicAttack.baseCooldown + Time.time;
        AnimatorSwitchState(BASICATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void ScreamAttack()
    {
        Debug.Log("scream");
        screamAttack.currentCooldown = screamAttack.baseCooldown + Time.time;
        AnimatorSwitchState(SCREAMATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void StompAttack()
    {
        Debug.Log("stomp");
        stompAttack.currentCooldown = stompAttack.baseCooldown + Time.time;
        AnimatorSwitchState(STOMPPREPARATIONANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void TeleportStrikeAttack()
    {
        Debug.Log("teleport");
        teleportAttack.currentCooldown = teleportAttack.baseCooldown + Time.time;
        AnimatorSwitchState(SICKLETHROWANIMATION);
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
        //attackDecision = AttackDecision.none;
        decisionTimer = Time.time + 2f;
        currentDecision = null;
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
