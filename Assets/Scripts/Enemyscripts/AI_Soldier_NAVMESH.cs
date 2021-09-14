using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Soldier_NAVMESH : MonoBehaviour
{
    Animator animator;

    //Pathseeking
    private Rigidbody2D rigidBody2D;
    public Transform target;
    public Vector3 offsetTargetPathLeft;
    public Vector3 offsetTargetPathRight;

    //movement
    public float movementSpeed = 100f;
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
    int numberOfAttacks = 0;
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
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //float direction = (path.vectorPath[currentWaypoint].x - rigidBody2D.position.x);
        //rigidBody2D.velocity = new Vector2(direction * movementSpeed, rigidBody2D.velocity.y);

        if (rigidBody2D.velocity.x > 0 && facingLeft)
        {
            Flip();
        }
        else if (rigidBody2D.velocity.x < 0 && !facingLeft)
        {
            Flip();
        }

        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);

        animator.SetFloat("animSoldierSpeed", Math.Abs(rigidBody2D.velocity.x));
    }

    private void Update()
    {
        //check to see if there are enemies in attack range
        shouldIAttack = Physics2D.OverlapCircleAll(swordColliderSoldier.position, attackRangeSoldier, enemiesLayers);
        int enemiesInRange = shouldIAttack.Length;
        if (enemiesInRange > 0 && Time.time >= nextAttackTimeSoldier && Time.time >= nextGlobalAttackSoldier && numberOfAttacks == 0)
        {
            numberOfAttacks++;
            animator.SetTrigger("animSoldierAttack");
        }
    }

    //Combat system
    void SoldierAttack()
    {
        //StartCoroutine("WindUpAttack");
        Debug.Log("we're attacking");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordColliderSoldier.position, attackRangeSoldier, enemiesLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Soldier hit " + enemy + " with a sword");
            enemy.GetComponent<KarasuEntity>().TakeDamage(attackDamageSoldier);
        }
        numberOfAttacks = 0;
        nextAttackTimeSoldier = Time.time + 1f / attackSpeedSoldier;
        nextGlobalAttackSoldier = Time.time + 1f;
    }

    //Utilities
    
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
