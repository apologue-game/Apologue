using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shieldman : MonoBehaviour, IEnemy
{
    ShieldmanAI shieldmanAI;

    BoxCollider2D boxCollider2D;
    BoxCollider2D boxCollider2DKarasu;

    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }

    public bool shieldBroken = false;
    public bool isBlocking = false;

    private void Awake()
    {
        shieldmanAI = GetComponent<ShieldmanAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 5;
        enemyType = IEnemy.EnemyType.normal;

        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2DKarasu = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, bool? specialInteraction)
    {
        if (isDead)
        {
            return;
        }
        if (!shieldBroken)
        {
            if (specialInteraction == true)
            {
                shieldBroken = true;
                Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
                StartCoroutine(ShieldmanStaggered());
                animator.Play("shieldbreakAnimation");
            }
            else
            {
                StartCoroutine(ShieldmanBlock());
            }
        }
        else
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
            StartCoroutine(ShieldmanStaggered());
        }
    }

    //staggered only if the shield is broken
    IEnumerator ShieldmanStaggered()
    {
        isTakingDamage = true;
        //TODO: shieldman stagger animation
        yield return new WaitForSeconds(0.35f);
        isTakingDamage = false;
    }

    IEnumerator ShieldmanBlock()
    {
        isBlocking = true;
        animator.Play("blockAnimation");
        yield return new WaitForSeconds(0.383f);
        isBlocking = false;
    }

    public IEnumerator Death()
    {
        isDead = true;
        Debug.Log("Shieldman died");
        animator.Play("deathAnimation");
        yield return new WaitForSeconds(3.83f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(shieldmanAI.spawn);
    }
}
