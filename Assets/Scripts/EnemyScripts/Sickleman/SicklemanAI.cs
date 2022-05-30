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
    public EnemyHealthBar healthBar;
    public GameObject healthBarGO;
    int sicklemanWeaponSpecificID = 1;

    //Targeting
    GameObject karasu;
    Transform karasuTransform;
    Vector3 spawnLocation;
    Transform currentTarget;
    [HideInInspector]
    public GameObject spawn;
    public Transform groundCheck;
    public float groundCheckRange = 0.1f;
    public LayerMask whatIsGround;

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
    bool grounded = true;

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
    public float decisionTimer = 0f;

    //Attacks
    public bool currentlyAttacking = false;
    public bool currentlyStomping = false;
    public float stompingSpeed = 30f;
    public float jumpForce = 1000f;
    //Teleport strike
    public bool currentlyTeleporting = false;
    Throwable weapon;
    public Transform sickleWeapon;
    public bool weaponThrow = false;
    public bool weaponTraveling = false;
    public float throwSpeed = 15f;
    Vector3 rotate;
    Quaternion rotationQuaternion;
    Vector3 teleportOutOfBounds;
    Vector3 offsetTeleportExit;
    Vector3 putWeaponBackInThePool_Teleport;

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
    const string PORTALENTERANIMATION = "portalEnter";
    const string PORTALEXITANIMATION = "portalExit";

    private void Awake()
    {
        //Self references and initializations
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        sickleman = GetComponent<Sickleman>();

        rotate = new Vector3(0f, 0f, 15f);
        rotationQuaternion = new Quaternion(0f, 0f, 0f, 0f);
        putWeaponBackInThePool_Teleport = new Vector3(-43.9000015f, -57.5999985f, 0);
    }

    void Start()
    {
        //New decision system
        basicAttack = new Decision(0, 2f);
        screamAttack = new Decision(1, 7f, 4f);
        stompAttack = new Decision(2, 5f);
        teleportAttack = new Decision(3, 5f);
        decisions = new List<Decision>();
        //decisions.Add(basicAttack);
        //decisions.Add(screamAttack);
        //decisions.Add(stompAttack);
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

        movementSpeedHelper = movementSpeed;
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        InvokeRepeating(nameof(InCombatOrGoBackToSpawn), 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        if (sickleman.inCombat)
        {
            currentTarget = karasuTransform;
        }
        if (!sickleman.inCombat)
        {
            currentTarget = null;
            healthBar.SetHealth(sickleman.maxHealth);
            if (transform.position.x != spawn.transform.position.x)
            {
                transform.position = spawn.transform.position;
            }
            if (!facingLeft)
            {
                healthBar.Flip();
                Flip();
            }
            currentlyAttacking = false;
            currentDecision = null;
            AnimatorSwitchState(IDLEANIMATION);
        }
        if (weaponThrow)
        {
            weapon.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            weapon.transform.Rotate(rotate);
            if (weaponTraveling)
            {
                if (!weapon.stop)
                {
                    weapon.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * throwSpeed * Time.deltaTime, 0);
                }
            }
        }
        //Karasu parry collider needs to be ignored repeatedly because it is getting disabled and enabled multiple times
        if (currentTarget == karasuTransform)
        {
            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
        }

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
        if (sickleman.isDead)
        {
            if (grounded)
            {
                rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                AnimatorSwitchState(DEATHANIMATION);
            }
            return;
        }
        if (KarasuEntity.dead || currentlyTeleporting)
        {
            return;
        }
        if (currentlyStomping)
        {
            if (grounded)
            {
                StompLanding();
            }
            else if (!grounded)
            {
                //TODO: Sickleman and samurai should lock on to the last known location, not follow around
                rigidBody2D.velocity = new Vector2(direction * stompingSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            }
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
        if (currentDecision == null && Time.time > decisionTimer && currentTarget == karasuTransform)
        {
            currentDecision = decisionMaking.DecisionCalculation_Cooldown_Range(hDistance);
        }
        if (currentDecision != null && !currentlyAttacking)
        {
            if (currentDecision.Id == 0)
            {
                if (hDistance < stoppingDistance && !currentlyAttacking)
                {
                    currentlyAttacking = true;
                    BasicAttack();
                }
                else
                {
                    rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
                }
            }
            else if (currentDecision.Id == 1)
            {
                currentlyAttacking = true;
                ScreamAttack();
            }
            else if (currentDecision.Id == 2)
            {
                currentlyAttacking = true;
                StompAttack();
            }
            else if (currentDecision.Id == 3)
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
        basicAttack.CurrentCooldown = basicAttack.BaseCooldown + Time.time;
        AnimatorSwitchState(BASICATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void ScreamAttack()
    {
        screamAttack.CurrentCooldown = screamAttack.BaseCooldown + Time.time;
        AnimatorSwitchState(SCREAMATTACKANIMATION);
        StartCoroutine(StopMovingWhileAttacking());
    }

    void StompAttack()
    {
        stompAttack.CurrentCooldown = stompAttack.BaseCooldown + Time.time;
        AnimatorSwitchState(STOMPPREPARATIONANIMATION);
    }

    void JumpEvent()
    {
        rigidBody2D.AddForce(Vector2.up * jumpForce);
    }

    void StompFalling()
    {
        currentlyStomping = true;
        AnimatorSwitchState(STOMPFALLINGNIMATION);
    }

    void StompLanding()
    {
        AnimatorSwitchState(STOMPLANDINGANIMATION);
        currentlyStomping = false;
        rigidBody2D.velocity = Vector2.zero;
    }

    void TeleportStrikeAttack()
    {
        currentlyTeleporting = true;
        teleportAttack.CurrentCooldown = teleportAttack.BaseCooldown + Time.time;
        AnimatorSwitchState(PORTALENTERANIMATION);
    }

    void ThrowSickle()
    {
        for (int i = 0; i < Throwables.throwables[sicklemanWeaponSpecificID].Count; i++)
        {
            if (!Throwables.throwables[sicklemanWeaponSpecificID][i].inUse)
            {
                weapon = Throwables.throwables[sicklemanWeaponSpecificID][i];
                break;
            }
        }
        weapon.transform.position = sickleWeapon.position;
        weapon.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        weapon.inUse = true;
        weaponThrow = true;
        weaponTraveling = true;
        StartCoroutine(WeaponTravelDuration());
    }

    IEnumerator WeaponTravelDuration()
    {
        yield return new WaitForSeconds(1.1f);
        weaponTraveling = false;
        weapon.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void DisappearThroughPortal()
    {
        teleportOutOfBounds = new Vector3(transform.position.x, transform.position.y + 1000f, 0f);
        transform.position = teleportOutOfBounds;
        StartCoroutine(TravelingThroughThePortal());
    }

    IEnumerator TravelingThroughThePortal()
    {
        yield return new WaitForSeconds(1.2f);
        ExitPortal();
    }

    void ExitPortal()
    {
        AnimatorSwitchState(PORTALEXITANIMATION);
        if (facingLeft)
        {
            offsetTeleportExit = weapon.transform.position;
            offsetTeleportExit.x -= 0.5f;
            transform.position = offsetTeleportExit;
        }
        else if (!facingLeft)
        {
            offsetTeleportExit = weapon.transform.position;
            offsetTeleportExit.x += 0.5f;
            transform.position = offsetTeleportExit;
        }
    }

    void FinishTeleporting()
    {
        currentlyTeleporting = false;
        currentlyAttacking = false;
        currentDecision = null;
    }

    void CatchWeapon()
    {
        weaponThrow = false;
        weapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        weapon.transform.rotation = rotationQuaternion;
        weapon.inUse = false;
        weapon.transform.position = putWeaponBackInThePool_Teleport;
        weapon.hasDamaged = false;
    }

    //Utilities
    IEnumerator StopMovingWhileAttacking()
    {
        rigidBody2D.velocity = Vector2.zero;
        movementSpeed = 0;
        yield return new WaitForSeconds(1f);
        movementSpeed = movementSpeedHelper;
    }

    void NotAttacking()
    {
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
