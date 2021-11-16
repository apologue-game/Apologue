using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IEnemy
{
    public HealthBar healthBar;
    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }
    public bool isStaggered { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 15;
        enemyType = IEnemy.EnemyType.elite;
    }

    void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetMaximumHealth(maxHealth);
    }

    public void TakeDamage(int damage, bool? specialInteraction)
    {
        if (isDead)
        {
            return;
        }
        else
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                BeforeDeath();
                return;
            }
        }
        //StartCoroutine(BossTakingDamage());
    }

    public IEnumerator BossTakingDamage()
    {
        isTakingDamage = true;
        yield return new WaitForSeconds(1f);
        isTakingDamage = false;
    }
    
    public void BossStagger()
    {
        StartCoroutine(BossStaggered());
    }

    public IEnumerator BossStaggered()
    {
        isStaggered = true;
        animator.Play("stagger");
        yield return new WaitForSeconds(1f);
        isStaggered = false;
    }

    public void BeforeDeath()
    {
        isDead = true;
        animator.Play("beforeDeath");
    }

    void DeathCall()
    {
        animator.Play("death");
        StartCoroutine(Death());
    }

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(3.5f);
        //GameMaster.DestroyGameObject(gameObject);
        //GameMaster.DestroyGameObject(bossAI.spawn);
    }
}
