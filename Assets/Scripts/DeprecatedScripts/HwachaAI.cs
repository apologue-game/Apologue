using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HwachaAI : MonoBehaviour
{
    //Utilities
    int myID;
    string myName = "";
    Hwacha hwacha;
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

    //Movement
    public float hDistanceAbsolute;
    public float vDistance;
    float spawnHorizontalDistance;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    CircleCollider2D karasuParryCollider;

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
        //Self references and initializations
        animator = GetComponent<Animator>();
        hwacha = GetComponent<Hwacha>();

        currentTarget = null;
    }

    void Start()
    {

        //Neccessary references for targeting
        karasu = GameObject.FindGameObjectWithTag("Player");
        karasuTransform = karasu.transform;

        //Ignore collider collisions
        karasuParryCollider = karasu.transform.Find("ParryCollider").GetComponent<CircleCollider2D>();

        //Spawn location references
        myID = GameMaster.enemyID++;
        myName = "Dasher" + myID + "_SpawnLocation";
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawn = new GameObject(myName);
        spawn.transform.position = spawnLocation;

        //InvokeRepeating("InCombatOrGoBackToSpawn", 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        if (hwacha.inCombat)
        {
            currentTarget = karasuTransform;
        }
        if (!hwacha.inCombat)
        {
            currentTarget = null;
            hwacha.currentHealth = hwacha.maxHealth;
            healthBar.SetHealth(hwacha.maxHealth);
            if (transform.position.x != spawn.transform.position.x)
            {
                transform.position = spawn.transform.position;
            }

            currentlyAttacking = false;
            AnimatorSwitchState(IDLEANIMATION);
        }
        //Exceptions
        if (hwacha.isDead)
        {
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
        if (currentTarget == karasuTransform)
        {
            if (Time.time > nextAttack && !currentlyAttacking/* && distance > 1*/)
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
        }
    }

    //Movement

    //Combat system
    void Attack()
    {
        nextAttack = Time.time + 5f;
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
        if (hDistanceAbsolute < 10 && currentTarget != karasuTransform)
        {
            currentTarget = karasuTransform;
        }
        else if (hDistanceAbsolute > 15 && currentTarget != spawn.transform)
        {
            currentTarget = spawn.transform;
            //heal enemy if target gets out of range
            //hwacha.currentHealth = hwacha.maxHealth;
            //healthBar.SetMaximumHealth(hwacha.maxHealth);
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
