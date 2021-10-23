using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : MonoBehaviour, IEnemy
{
    HeavyEnemyAI heavyEnemyAI;

    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }

    private void Awake()
    {
        heavyEnemyAI = GetComponent<HeavyEnemyAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 10;
        enemyType = IEnemy.EnemyType.elite;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, bool? arrowDamage)
    {
        if (isDead)
        {
            return;
        }
        else
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
            StartCoroutine(HeavyEnemyStaggered());
        }
    }

    IEnumerator HeavyEnemyStaggered()
    {
        isTakingDamage = true;
        //play stagger animation
        yield return new WaitForSeconds(0.35f);
        isTakingDamage = false;
    }

    public IEnumerator Death()
    {
        isDead = true;
        //play death animation
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(heavyEnemyAI.spawn);
    }
}