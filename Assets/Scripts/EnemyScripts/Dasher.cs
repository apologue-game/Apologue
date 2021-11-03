using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dasher : MonoBehaviour, IEnemy
{
    DasherAI dasherAI;

    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }
    public bool isStaggered = false;

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }

    private void Awake()
    {
        dasherAI = GetComponent<DasherAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 5;
        enemyType = IEnemy.EnemyType.ranged;
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
        else
        {
            if (isStaggered)
            {
                currentHealth -= damage * 5;
            }
            else
            {
                currentHealth -= damage;
            }
            
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
            StartCoroutine(DasherIsTakingDamage());
        }
    }

    public void Stagger()
    {
        StartCoroutine(DasherStaggered());
    }

    public IEnumerator DasherIsTakingDamage()
    {
        isStaggered = true;
        //animator.Play("stagger");
        yield return new WaitForSeconds(1.5f);
        isStaggered = false;
    }

    public IEnumerator DasherStaggered()
    {
        isTakingDamage = true;
        animator.Play("stagger");
        yield return new WaitForSeconds(1.5f);
        isTakingDamage = false;
    }

    public IEnumerator Death()
    {
        isDead = true;
        animator.Play("death");
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(dasherAI.spawn);
    }
}
