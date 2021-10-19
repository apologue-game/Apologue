using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleArcherAI : MonoBehaviour
{
    //Self references
    FemaleArcher femaleArcher;
    Animator animator;

    //Targeting
    GameObject karasu;
    Transform karasuTransform;
    public Transform currentTarget = null;
    public float hDistance;
    public float vDistance;
    public bool facingLeft = true;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    BoxCollider2D boxCollider2DKarasu;
    CircleCollider2D karasuParryCollider;
    CircleCollider2D karasuBlockCollider;

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
        //Targeting references
        karasu = GameObject.FindGameObjectWithTag("Player");
        karasuTransform = karasu.transform;

        //Shooting position
        //bowPositionMiddle = new Vector3(-1.325005f, 0.474f, 0);
        //bowPositionUp = new Vector3(-1.15900004f, -0.246000007f, 0);
        //bowPositionDown = new Vector3(-1.15900004f, -0.246000007f, 0);
        //bowRotationUp = new Quaternion(0, 0, -0.211816221f, 0.977309525f);
        //bowRotationDown = new Quaternion(0, 0, 0.284491211f, 0.958678663f);

        //Self references and initializations
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        femaleArcher = GetComponent<FemaleArcher>();

        //Ignore collider collisions
        boxCollider2DKarasu = karasu.GetComponent<BoxCollider2D>();
        karasuParryCollider = karasu.transform.Find("ParryCollider").GetComponent<CircleCollider2D>();
        karasuBlockCollider = karasu.transform.Find("BlockCollider").GetComponent<CircleCollider2D>();

        //Spawn location references
        myID = GameMaster.enemyID++;
        myName = "Archer" + myID + "_SpawnLocation";
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawn = new GameObject(myName);
        spawn.transform.position = spawnLocation;
        currentTarget = spawn.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        InvokeRepeating("InCombat", 0f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hDistance = Mathf.Abs(transform.position.x - karasuTransform.position.x);
        vDistance = Mathf.Abs(transform.position.y - karasuTransform.position.y);
        if (transform.position.x > karasuTransform.position.x && !facingLeft && !currentlyAttacking)
        {
            Flip();
        }
        else if (transform.position.x < karasuTransform.position.x && facingLeft && !currentlyAttacking)
        {
            Flip();
        }
        if (currentTarget == null)
        {
            AnimatorSwitchState(IDLEANIMATION);
        }
        //Karasu parry and block colliders need to be ignored repeatedly because they're getting disabled and enabled multiple times
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

            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
            Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
        }
    }

    //Combat system
    void Attack()
    {
        nextAttack = Time.time + attackCooldown;
        currentlyAttacking = true;
        if (GameMaster.Utilities.IsFloatInRange(transform.position.y - 1.5f, transform.position.y + 1.5f, karasuTransform.position.y + 0.51f))
        {
            AnimatorSwitchState(ATTACKANIMATION);
        }
        //else if (transform.position.y > karasuTransform.position.y)
        //{
        //    AnimatorSwitchState(ATTACKDOWNWARDSANIMATION);
        //}
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
        if (position == 0)
        {
            Instantiate(arrowPrefab, arrowPointMiddle.position, arrowPointMiddle.rotation, arrowPointMiddle);
        }
        else if(position == 1)
        {
            Instantiate(arrowPrefab, arrowPointUp.position, arrowPointUp.rotation, arrowPointUp);
        }
        else 
        {
            Instantiate(arrowPrefab, arrowPointDown.position, arrowPointDown.rotation, arrowPointDown);
        }
    }

    void Reload()
    {
        AnimatorSwitchState(RELOADANIMATION);
        currentlyAttacking = false;
    }

    //Utilities
    void InCombat()
    {
        if (hDistance < 15 && currentTarget != karasuTransform)
        {
            currentTarget = karasuTransform;
        }
        else if (hDistance > 15 && currentTarget != null)
        {
            currentTarget = null;
            //Heal archer if target gets out of range
            femaleArcher.currentHealth = femaleArcher.maxHealth;
        }
    }

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
