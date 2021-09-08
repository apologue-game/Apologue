using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class AI_Soldier : MonoBehaviour
{
    Animator animator;

    //Pathseeking
    Seeker seeker;
    private Rigidbody2D rigidBody2D;
    public Transform target;
    Path path;
    int currentWaypoint = 0;
    bool endOfPath = false;

    //movement
    public float movementSpeed = 100f;
    float nextWaypointDistance = 0.5f;
    bool facingLeft = true;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    public BoxCollider2D boxCollider2DKarasu;

    //Combat system
    public LayerMask enemiesLayers;
    Collider2D[] shouldIAttack;
    float nextGlobalAttackSoldier = 0f;
    //Soldier basic attack
    public Transform swordColliderSoldier;
    public float attackRangeSoldier = 0.5f;
    int attackDamageSoldier = 3;
    float attackSpeedSoldier = 0.75f;
    float nextAttackTimeSoldier = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.75f);
    }

    void FixedUpdate()
    {
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

        //Using the commented code below, characters can fly
        //Good for flying enemies
        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidBody2D.position).normalized;
        //Vector2 force = direction * movementSpeed * Time.deltaTime;
        //rigidBody2D.AddForce(force);

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

        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);

        animator.SetFloat("animSoldierSpeed", Math.Abs(rigidBody2D.velocity.x));
    }

    private void Update()
    {
        //check to see if there are enemies in attack range
        shouldIAttack = Physics2D.OverlapCircleAll(swordColliderSoldier.position, attackRangeSoldier, enemiesLayers);
        int enemiesInRange = shouldIAttack.Length;
        if (enemiesInRange > 0)
        {
            SoldierAttack();
        }
    }

    //Combat system
    void SoldierAttack()
    {
        if (Time.time >= nextAttackTimeSoldier && Time.time >= nextGlobalAttackSoldier)
        {
            Debug.Log("Conditions for an attack passed");
            animator.SetTrigger("animSoldierAttack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordColliderSoldier.position, attackRangeSoldier, enemiesLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Soldier hit " + enemy + " with a sword");
                enemy.GetComponent<KarasuEntity>().TakeDamage(attackDamageSoldier);
            }
            nextAttackTimeSoldier = Time.time + 1f / attackSpeedSoldier;
            nextGlobalAttackSoldier = Time.time + 1f;
        }
    }

    //Utilities
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rigidBody2D.position, target.position, OnPathComplete);
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
