using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IEnemy
{
    public GameObject healthBarGO;
    public HealthBar healthBar;
    public Animator animator { get; set; }
    Rigidbody2D rigidBody2D;

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }
    public bool isStaggered { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public float currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }
    public bool inCombat { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    bool beforeDeath = false;
    bool executed = false;

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
        rigidBody2D = GetComponent<Rigidbody2D>();
        healthBar.SetMaximumHealth(maxHealth);
    }

    public void TakeDamage(float damage, bool? specialInteraction)
    {
        if (beforeDeath)
        {
            executed = true;
            DeathCall();
        }
        if (isDead)
        {
            return;
        }
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            BeforeDeath();
            return;
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
        beforeDeath = true;
        StartCoroutine(ExecutionWindow());
    }

    IEnumerator ExecutionWindow()
    {
        yield return new WaitForSeconds(2f);
        if (!executed)
        {
            isDead = false;
            currentHealth += 5;
            healthBar.SetHealth(currentHealth);
            beforeDeath = false;
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            animator.Play("idle");
        }
    }

    void DeathCall()
    {
        healthBarGO.SetActive(false);
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
