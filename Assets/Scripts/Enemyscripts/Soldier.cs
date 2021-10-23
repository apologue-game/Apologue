using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour, IEnemy
{
    SoldierAI soldierAI;

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
        soldierAI = GetComponent<SoldierAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 5;
        enemyType = IEnemy.EnemyType.normal;
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
        if (BlockCollider.blockedOrParried)
        {
            Debug.Log("Soldier blocked an attack!!");
        }
        else
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
            StartCoroutine(SoldierStaggered());
        }
    }

    IEnumerator SoldierStaggered()
    {
        isTakingDamage = true;
        animator.SetTrigger("animSoldierTakingDamage");
        yield return new WaitForSeconds(0.35f);
        isTakingDamage = false;
    }

    public IEnumerator Death()
    {
        isDead = true;
        Debug.Log("Soldier died");
        animator.SetTrigger("animSoldierDeath");
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(soldierAI.spawn);
    }
}
