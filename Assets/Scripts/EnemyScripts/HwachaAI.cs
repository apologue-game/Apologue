using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HwachaAI : MonoBehaviour
{
    //Utilities
    int myID;
    string myName = "";
    Hwacha hwacha;

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
    public float hDistance;
    public float vDistance;
    float spawnHorizontalDistance;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    BoxCollider2D boxCollider2DKarasu;
    CircleCollider2D karasuParryCollider;
    CircleCollider2D karasuBlockCollider;

    //Combat system
    public GameObject hawchaArrowPrefab;
    public Transform firePointRight;
    public Transform firePointLeft;
    bool currentlyAttacking = false;
    float nextAttack = 0f;

    //Animations manager
    string oldState = "";

    const string IDLEANIMATION = "idle";
    const string FIRERIGHT = "fireRight";
    const string FIRELEFT = "fireLeft";
    const string AFTERFIRERIGHT = "afterFireRight";
    const string AFTERFIRELEFT = "afterFireLeft";
    const string RELOADRIGHT = "reloadRight";
    const string RELOADLEFT = "reloadLeft";

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
        hwacha = GetComponent<Hwacha>();

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
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        InvokeRepeating("InCombatOrGoBackToSpawn", 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        //Exceptions
        if (hwacha.isDead)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        if (hwacha.isTakingDamage || KarasuEntity.dead)
        {
            return;
        }

        if (currentTarget == null)
        {
            AnimatorSwitchState(IDLEANIMATION);
        }

        //Attacking
        if (currentTarget != null)
        {
            if (Time.time > nextAttack && !currentlyAttacking)
            {
                currentlyAttacking = true;
                Attack();
            }
            else if (!currentlyAttacking)
            {
                AnimatorSwitchState(IDLEANIMATION);
            }
            //Karasu parry and block colliders need to be ignored repeatedly because they're getting disabled and enabled multiple times
            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
            Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
        }
    }

    //Movement

    //Combat system
    void Attack()
    {
        nextAttack = Time.time + 8f;
        AnimatorSwitchState(FIRERIGHT);
    }

    void HwachaAttackRight()
    {
        Instantiate(hawchaArrowPrefab, firePointRight.position, firePointRight.rotation);
    }

    void AfterFireRight()
    {
        AnimatorSwitchState(AFTERFIRERIGHT);
    }

    void AttackLeft()
    {
        AnimatorSwitchState(FIRELEFT);
    }

    void HwachaAttackLeft()
    {
        Instantiate(hawchaArrowPrefab, firePointLeft.position, firePointLeft.rotation);
    }

    void AfterFireLeft()
    {
        AnimatorSwitchState(AFTERFIRELEFT);
    }

    void ReloadRight()
    {
        AnimatorSwitchState(RELOADRIGHT);
    }

    void ReloadLeft()
    {
        AnimatorSwitchState(RELOADLEFT);
    }

    void ReadyToShootAgain()
    {
        currentlyAttacking = false;
    }

    //Utilities
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
            hwacha.currentHealth = hwacha.maxHealth;
        }
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
