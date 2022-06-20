using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleArcherAI : MonoBehaviour
{
    //Self references
    FemaleArcher femaleArcher;
    Animator animator;
    public HealthBar healthBar;
    public Rigidbody2D rigidBody2D;

    //Targeting
    GameObject karasu;
    Transform karasuTransform;
    public Transform currentTarget = null;
    public float hDistance;
    public float vDistance;
    public bool facingLeft = true;
    public bool targetInLine = false;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    BoxCollider2D boxCollider2DKarasu;
    CircleCollider2D karasuParryCollider;

    //Spawn location
    int myID;
    string myName = "";
    Vector3 spawnLocation;
    [HideInInspector]
    public GameObject spawn;

    //Attacking
    public GameObject arrowPrefab;
    public float attackCooldown = 2.5f;
    public float nextAttack = 0f;
    public bool currentlyAttacking = false;
    public Transform arrowPointMiddle;
    public Transform arrowPointUp;
    public Transform arrowPointDown;

    public Transform groundCheck;
    float groundCheckRange = 0.11f;
    public LayerMask whatIsGround;
    public bool grounded = false;

    //Animations manager
    string oldState = "";

    const string IDLEANIMATION = "idleAnimation";
    const string ATTACKANIMATION = "attackAnimation";
    const string ATTACKUPWARDSANIMATION = "archerAttackUpwardsAnimation";
    const string ATTACKDOWNWARDSANIMATION = "archerAttackDownwardsAnimation";
    const string RELOADANIMATION = "archerReloadAnimation";

    //help

    private void Awake()
    {
        //Self references and initializations
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        femaleArcher = GetComponent<FemaleArcher>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Targeting references
        karasu = GameObject.FindGameObjectWithTag("Player");
        karasuTransform = karasu.transform;

        //Ignore collider collisions
        boxCollider2DKarasu = karasu.GetComponent<BoxCollider2D>();
        karasuParryCollider = karasu.transform.Find("ParryCollider").GetComponent<CircleCollider2D>();

        //Spawn location references
        myID = GameMaster.enemyID++;
        myName = "Archer" + myID + "_SpawnLocation";
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawn = new GameObject(myName);
        spawn.transform.position = spawnLocation;
        currentTarget = spawn.transform;

        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] collidersGround = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRange, whatIsGround);
        for (int i = 0; i < collidersGround.Length; i++)
        {
            if (collidersGround[i].name == "PlatformsTilemap" || collidersGround[i].name == "GroundTilemap")
            {
                grounded = true;
            }
        }
        if (femaleArcher.isDead)
        {
            if (grounded)
            {
                rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            boxCollider2D.enabled = false;
            return;
        }
        if (femaleArcher.inCombat)
        {
            currentTarget = karasuTransform;
        }
        if (!femaleArcher.inCombat)
        {
            currentTarget = null;
            healthBar.SetHealth(femaleArcher.maxHealth);

            if (!facingLeft)
            {
                healthBar.Flip();
                Flip();
            }
            currentlyAttacking = false;
            AnimatorSwitchState(IDLEANIMATION);
        }
        hDistance = Mathf.Abs(transform.position.x - karasuTransform.position.x);
        vDistance = Mathf.Abs(transform.position.y - karasuTransform.position.y);
        if (transform.position.x > karasuTransform.position.x && !facingLeft && !currentlyAttacking)
        {
            Flip();
            healthBar.Flip();
        }
        else if (transform.position.x < karasuTransform.position.x && facingLeft && !currentlyAttacking)
        {
            Flip();
            healthBar.Flip();
        }
        if (currentTarget == null)
        {
            AnimatorSwitchState(IDLEANIMATION);
        }   
        if (currentTarget != null)
        {
            if (Time.time > nextAttack)
            {
                Attack();
            }
            else if (!currentlyAttacking)
            {
                AnimatorSwitchState(IDLEANIMATION);
            }
            //Karasu parry and block colliders need to be ignored repeatedly because they're getting disabled and enabled multiple times
            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
        }
    }

    //Combat system
    void Attack()
    {
        nextAttack = Time.time + attackCooldown;
        currentlyAttacking = true;
        targetInLine = GameMaster.Utilities.IsFloatInRange(transform.position.y - 1.7f, transform.position.y + 1.7f, karasuTransform.position.y + 0.51f);
        if (targetInLine)
        {
            AnimatorSwitchState(ATTACKANIMATION);
        }
        else
        {
            AnimatorSwitchState(ATTACKUPWARDSANIMATION);
        }
    }

    void IdleAfterReload()
    {
        AnimatorSwitchState(IDLEANIMATION);
    }
    
    void Shoot(int position)
    {
        //No need for the projectiles to have parents :(
        if (position == 0)
        {
            Instantiate(arrowPrefab, arrowPointMiddle.position, arrowPointMiddle.rotation);
        }
        else if (position == 1)
        {
            Instantiate(arrowPrefab, arrowPointUp.position, arrowPointUp.rotation);
        }
    }

    void Reload()
    {
        //AnimatorSwitchState(RELOADANIMATION);
        currentlyAttacking = false;
    }

    //Utilities
    //void InCombat()
    //{
    //    if (hDistance <= 17 && hDistance >= 4 && currentTarget != karasuTransform)
    //    {
    //        currentTarget = karasuTransform;
    //    }
    //    else if (hDistance > 17 || hDistance <= 4 && currentTarget != null)
    //    {
    //        currentTarget = null;
    //        //Heal archer if target gets out of range
    //        //femaleArcher.currentHealth = femaleArcher.maxHealth;
    //    }
    //}

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

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
