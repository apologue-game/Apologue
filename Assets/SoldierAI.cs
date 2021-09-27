using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAI : MonoBehaviour
{
    System.Random rnd = new System.Random();
    static SoldierAI soldierAI;

    Animator animator;
    public Animator karasuAnimator;
    KarasuEntity karasuEntity;
    PlayerControl playerControl;

    //Pathseeking
    private Rigidbody2D rigidBody2D;
    public Transform target;

    //movement
    float movementSpeed = 150f;
    float movementSpeedHelper;
    readonly float stoppingDistance = 0.75f;
    readonly float flipDistance = 0.2f;
    bool facingLeft = true;
    bool grounded = true;
    int direction = 0;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    public BoxCollider2D boxCollider2DKarasu;
    public CircleCollider2D karasuParryCollider;
    public CircleCollider2D karasuBlockCollider;

    //Combat system
    public LayerMask enemiesLayers;
    public LayerMask parryLayer;
    public LayerMask blockLayer;
    Collider2D[] shouldIAttack;
    public float nextGlobalAttackSoldier = 0f;
    public bool currentlyAttacking = false;
    //Soldier basic attack
    public Transform swordColliderSoldier;
    public int numberOfAttacks = 0;
    float attackRangeSoldier = 0.5f;
    int attackDamageSoldier = 3;
    float attackSpeedSoldier = 0.75f;
    public float nextAttackTimeSoldier = 0f;
    public float lastTimeAttack = 0f;
    //Parry and block system for Player
    public bool parriedOrBlocked = false;
    //Soldier block
    public Transform blockCollider;
    public GameObject blockColliderGO;
    float blockColliderRange;
    public bool currentlyBlocking = false;
    public int blockChance;

    private void Awake()
    {
        karasuEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<KarasuEntity>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        animator = GetComponent<Animator>();
        soldierAI = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        movementSpeedHelper = movementSpeed;
        rigidBody2D = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
        Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
        InvokeRepeating("GenerateRandomNumber", 0f, 0.2f);
    }

    private void FixedUpdate()
    {
        //Exceptions
        if (Soldier.takingDamage || Soldier.soldierDead || karasuEntity.dead)
        {
            return;
        }

        ////Movement
        float distance = Mathf.Abs(transform.position.x - target.position.x);

        if (distance > stoppingDistance)
        {
            if (transform.position.x > target.position.x)
            {
                if (!facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
                direction = -1;
            }
            else if (transform.position.x < target.position.x)
            {
                if (facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
                direction = 1;
            }
        }
        else
        {
            if (distance > flipDistance)
            {
                if (transform.position.x > target.position.x && !facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
                else if (transform.position.x < target.position.x && facingLeft && !currentlyAttacking)
                {
                    Flip();
                }
            }
            direction = 0;
        }

        rigidBody2D.velocity = new Vector2(direction * movementSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);

        //Blocking interaction
        if (PlayerControl.currentlyAttacking && !currentlyBlocking && !currentlyAttacking && distance < 0.75 && !Soldier.takingDamage)
        {
            currentlyBlocking = true;
            //StartCoroutine(SoldierBlockConditions());
            SoldierBlock();
        }

        animator.SetFloat("animSoldierSpeed", Math.Abs(rigidBody2D.velocity.x));

        Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
        Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SoldierAttackConditions()
    {
        if (Time.time >= soldierAI.nextAttackTimeSoldier && Time.time >= soldierAI.nextGlobalAttackSoldier
            && soldierAI.numberOfAttacks == 0 && !soldierAI.currentlyBlocking && !soldierAI.currentlyAttacking && !Soldier.takingDamage)
        {
            lastTimeAttack = 0f;
            currentlyAttacking = true;
            soldierAI.numberOfAttacks++;
            soldierAI.animator.SetTrigger("animSoldierAttack");
            soldierAI.StartCoroutine(soldierAI.StopMovingWhileAttacking());
        }
        else
        {
            if (lastTimeAttack == 0f)
            {
                lastTimeAttack = Time.time + 2f;
            }
            else if (Time.time > lastTimeAttack)
            {
                ManuallySetAttackConditions();
            }
        }
    }

    //Combat system
    void SoldierAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordColliderSoldier.position, attackRangeSoldier, enemiesLayers);
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
                //StartCoroutine(BlockedAndHitAnimation());
                playerControl.AnimatorSwitchState("karasuBlockedAndHitAnimation");
                parriedOrBlocked = true;
            }
        }
        if (!parriedOrBlocked && hitEnemies.Length > 0)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Soldier hit " + enemy + " with a sword");
                enemy.GetComponent<KarasuEntity>().TakeDamage(attackDamageSoldier);
            }
        }
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextAttackTimeSoldier = Time.time + 1f / attackSpeedSoldier;
        nextGlobalAttackSoldier = Time.time + 1f;
    }

    void SoldierBlock()
    {
        if (blockChance == 1 || blockChance == 5)
        {
            animator.SetTrigger("animSoldierBlock");
            blockColliderGO.SetActive(true);
            StartCoroutine(SoldierBlockWindow());
        }
        else
        {
            currentlyBlocking = false;
        }
    }

    //Utilities
    void ManuallySetAttackConditions()
    {
        parriedOrBlocked = false;
        numberOfAttacks = 0;
        nextAttackTimeSoldier = 0;
        nextGlobalAttackSoldier = 0;
    }

    void GenerateRandomNumber()
    {
        blockChance = rnd.Next(0, 10);
    }

    IEnumerator BlockedAndHitAnimation()
    {
        //karasuAnimator.SetBool("animBlock", false);
        playerControl.AnimatorSwitchState("karasuBlockedAndHitAnimation");
        yield return new WaitForSeconds(0.3f);
        //karasuAnimator.SetBool("animBlock", true);
    }

    IEnumerator SoldierBlockWindow()
    {
        yield return new WaitForSeconds(0.4f);
        blockColliderGO.SetActive(false);
        currentlyBlocking = false;
    }

    IEnumerator StopMovingWhileAttacking()
    {
        if (grounded)
        {
            movementSpeed = 0;
            yield return new WaitForSeconds(1f);
            movementSpeed = movementSpeedHelper;
            currentlyAttacking = false;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            currentlyAttacking = false;
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
        if (swordColliderSoldier == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(swordColliderSoldier.position, attackRangeSoldier);
    }
}
