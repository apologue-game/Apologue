using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class AI_Soldier : MonoBehaviour
{
    Animator animator;
    public Animator karasuAnimator;
    KarasuEntity karasuEntity;

    //Pathseeking
    Seeker seeker;
    private Rigidbody2D rigidBody2D;
    public Transform target;
    Path path;
    public Vector3 offsetTargetPathLeft;
    public Vector3 offsetTargetPathRight;
    int currentWaypoint = 0;
    public bool endOfPath = false;

    //movement
    public float movementSpeed = 5f;
    public float nextWaypointDistance = 1.3f;
    bool facingLeft = true;
    bool grounded = true;
    public bool test;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    public BoxCollider2D boxCollider2DKarasu;
    public CircleCollider2D karasuParryCollider;
    public CircleCollider2D karasuBlockCollider;

    //Combat system
    public LayerMask enemiesLayers;
    Collider2D[] shouldIAttack;
    float nextGlobalAttackSoldier = 0f;
    //Soldier basic attack
    public Transform swordColliderSoldier;
    int numberOfAttacks = 0;
    public float attackRangeSoldier = 0.5f;
    int attackDamageSoldier = 3;
    float attackSpeedSoldier = 0.75f;
    float nextAttackTimeSoldier = 0f;
    //Parry and block system
    public bool parriedOrBlocked = false;

    private void Awake()
    {
        karasuEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<KarasuEntity>();
        animator = GetComponent<Animator>();
        offsetTargetPathLeft = new Vector3(-0.75f, 0, 0);
        offsetTargetPathRight = new Vector3(0.75f, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
        Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
        InvokeRepeating("UpdatePath", 0f, 0.75f);
    }

    void FixedUpdate()
    {
        test = karasuEntity.dead;
        if (karasuEntity.dead == true)
        {
            movementSpeed = 0;
            return;
        }
        else
        {
            movementSpeed = 5;
        }
        if (target == null)
        {
            StartCoroutine("SearchForPlayer");
            return;
        }
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            endOfPath = true;
            return;
        }
        else
        {
            endOfPath = false;
        }
        if (endOfPath)
        {
            //PathUtilities.IsPathPossible()
            //GraphNode graphNode;
            //AstarPath.GetNearest();
            return;
        }

        float direction = (path.vectorPath[currentWaypoint].x - rigidBody2D.position.x);
        rigidBody2D.velocity = new Vector2(direction * movementSpeed, rigidBody2D.velocity.y);
        if (rigidBody2D.velocity.x > 0 && facingLeft)
        {
            Flip();
        }
        else if (rigidBody2D.velocity.x < 0 && !facingLeft)
        {
            Flip();
        }

        float distance = Vector2.Distance(rigidBody2D.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (boxCollider2DKarasu == null)
        {
            StartCoroutine("SearchForPlayerBoxCollider");
        }
        if (karasuParryCollider == null)
        {
            StartCoroutine("SearchForPlayerParryCollider");
        }
        if (karasuBlockCollider == null)
        {
            StartCoroutine("SearchForPlayerBlockCollider");
        }
        animator.SetFloat("animSoldierSpeed", Math.Abs(rigidBody2D.velocity.x));
    }

    private void Update()
    {
        if (karasuEntity.dead == true)
        {
            return;
        }
        //check to see if there are enemies in attack range
        shouldIAttack = Physics2D.OverlapCircleAll(swordColliderSoldier.position, attackRangeSoldier, enemiesLayers);
        int enemiesInRange = shouldIAttack.Length;
        if (enemiesInRange > 0 && Time.time >= nextAttackTimeSoldier && Time.time >= nextGlobalAttackSoldier && numberOfAttacks == 0)
        {
            numberOfAttacks++;
            animator.SetTrigger("animSoldierAttack");
            StartCoroutine("StopMovingWhileAttacking");
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
                StartCoroutine("BlockedAndHitAnimation");
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

    //Utilities
    void UpdatePath()
    {
        //float distance = Vector3.Distance(rigidBody2D.position, target.position);
        ////If the soldier is walking from the left side to the right
        //if (distance > 2)
        //{
        //    if (seeker.IsDone())
        //    {
        //        Debug.Log("we're going from left to right");
        //        seeker.StartPath(rigidBody2D.position, target.position + offsetTargetPathRight, OnPathComplete);
        //    }
        //    //If the soldier is walking from the right side to the left
        //    else if (seeker.IsDone())
        //    {
        //        Debug.Log("we're going from right to left");
        //        seeker.StartPath(rigidBody2D.position, target.position + offsetTargetPathLeft, OnPathComplete);
        //    }
        //}
        //else
        //{
        //    rigidBody2D.velocity = new Vector2(0, rigidBody2D.velocity.y);
        //    if (facingLeft && distance > 0)
        //    {
        //        Flip();
        //    }
        //    else if (!facingLeft && distance < 0)
        //    {
        //        Flip();
        //    }
        //}
        if (seeker.IsDone())
        {
            seeker.StartPath(rigidBody2D.position, target.position, OnPathComplete);
        }
        //seeker.pathCallback(path);
    }


    //utilities
    //IEnumerator SearchForPlayer()
    //{
    //    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    //    if (playerObject != null)
    //    {
    //        target = playerObject.transform;
    //        yield break;
    //    }
    //    else if (playerObject == null)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        StartCoroutine("SearchForPlayer");
    //    }
    //}

    //IEnumerator SearchForPlayerBoxCollider()
    //{
    //    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    //    if (playerObject != null)
    //    {
    //        boxCollider2DKarasu = playerObject.GetComponent<BoxCollider2D>();
    //        yield break;
    //    }
    //    else if (playerObject == null)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        StartCoroutine("SearchForPlayer");
    //    }
    //}

    //IEnumerator SearchForPlayerParryCollider()
    //{
    //    GameObject parry;
    //    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    //    parry = playerObject.transform.Find("ParryCollider").gameObject;
    //    if (parry != null)
    //    {
    //        parry.SetActive(true);
    //        karasuParryCollider = parry.GetComponent<CircleCollider2D>();
    //        parry.SetActive(false);
    //    }
    //    else if (parry == null)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        StartCoroutine("SearchForPlayerParryCollider");
    //    }
    //}

    //IEnumerator SearchForPlayerBlockCollider()
    //{
    //    GameObject block = GameObject.Find("BlockCollider");
    //    if (block != null)
    //    {
    //        block.SetActive(true);
    //        karasuBlockCollider = block.GetComponent<CircleCollider2D>();
    //        block.SetActive(false);
    //    }
    //    else if (block == null)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        StartCoroutine("SearchForPlayerBlockCollider");
    //    }
    //}

    IEnumerator BlockedAndHitAnimation()
    {
        karasuAnimator.SetBool("animBlock", false);
        karasuAnimator.SetTrigger("animBlockedAndHit");
        yield return new WaitForSeconds(0.3f);
        karasuAnimator.SetBool("animBlock", true);
    }

    IEnumerator StopMovingWhileAttacking()
    {
        if (grounded)
        {
            movementSpeed = 0;
            yield return new WaitForSeconds(1f);
            movementSpeed = 5;
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
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
