using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : MonoBehaviour, IEnemy
{
    HeavyEnemyAI heavyEnemyAI;

    public GameObject heavyAxeRight;
    public GameObject heavyAxeLeft;
    Vector3 axeSpawnLocation;
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
        maxHealth = 1;
        enemyType = IEnemy.EnemyType.elite;
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
        animator.Play("deathAnimation");
        yield return new WaitForSeconds(3.5f);

        if (heavyEnemyAI.facingLeft)
        {
            axeSpawnLocation = new Vector3(transform.position.x + 0.4f, transform.position.y - 0.52f, transform.position.z);
            Instantiate(heavyAxeRight, axeSpawnLocation, transform.rotation);
        }
        else
        {
            axeSpawnLocation = new Vector3(transform.position.x - 0.4f, transform.position.y - 0.52f, transform.position.z);
            Instantiate(heavyAxeLeft, axeSpawnLocation, transform.rotation);
        }
        
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(heavyEnemyAI.spawn);
    }
}
