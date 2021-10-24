using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherAI : MonoBehaviour
{
    //Utilities
    int myID;
    string myName = "";
    Dasher dasher;

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
    public bool stopMoving;
    public float hDistance;
    public float vDistance;
    float spawnHorizontalDistance;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    BoxCollider2D boxCollider2DKarasu;
    CircleCollider2D karasuParryCollider;
    CircleCollider2D karasuBlockCollider;

    //Combat system
    public LayerMask enemiesLayers;
    bool currentlyAttacking = false;
    float lastTimeAttack = 0f;
    float globalAttackCooldown = 0f;
    //Dasher attack
    public Transform dashAttack;
    public float dashAttackRange;
    public float nextDashAttackTime;
    //Parry and block system for Player
    bool parriedOrBlocked = false;

    //Animations manager
    string oldState = "";

    const string IDLEANIMATION = "idle";
    const string DASHANIMATION = "dash";
    const string ATTACKANIMATION = "attack";
    const string RECOVERYANIMATION = "recovery";
    const string STAGGERANIMATION = "stagger";
    const string DEATHANIMATION = "death";


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
        dasher = GetComponent<Dasher>();

        //Ignore collider collisions
        boxCollider2DKarasu = karasu.GetComponent<BoxCollider2D>();
        karasuParryCollider = karasu.transform.Find("ParryCollider").GetComponent<CircleCollider2D>();
        karasuBlockCollider = karasu.transform.Find("BlockCollider").GetComponent<CircleCollider2D>();

        //Spawn location references
        myID = GameMaster.enemyID++;
        myName = "Dasher" + myID + "_SpawnLocation";
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawn = new GameObject(myName);
        spawn.transform.position = spawnLocation;
        currentTarget = spawn.transform;
    }

    void Start()
    {
        movementSpeedHelper = movementSpeed;
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
    }

    private void FixedUpdate()
    {
        //Exceptions
        if (dasher.isDead)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        if (dasher.isTakingDamage || karasuEntity.dead)
        {
            return;
        }

        //Movement
        hDistance = Mathf.Abs(transform.position.x - karasuTransform.position.x);
        vDistance = Mathf.Abs(transform.position.y - karasuTransform.position.y);
        spawnHorizontalDistance = Mathf.Abs(transform.position.x - spawn.transform.position.x);

        //Keep moving towards the target
        //Stopping distance from the target so the enemy won't try to go directly inside of them
        //Actual movement
        if (!stopMoving && vDistance < 1.2f && Time.time > lastTimeAttack)
        {
            rigidBody2D.velocity = new Vector2(movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
        }

        //Attacking
        if (hDistance < stoppingDistance && vDistance < stoppingDistance && !currentlyAttacking && !dasher.isTakingDamage && currentTarget == karasuTransform)
        {

        }

        //Animations

        //Karasu parry and block colliders need to be ignored repeatedly because they're getting disabled and enabled multiple times
        if (currentTarget == karasuTransform)
        {
            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
            Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
        }
    }

    //Movement

    //Combat system
    void Attack()
    {

    }

    void DasherAttack()
    {

    }

    //Utilities
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

    private void OnDrawGizmosSelected()
    {
        if (dashAttack == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(dashAttack.position, dashAttackRange);
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
