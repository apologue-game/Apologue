using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shieldman : MonoBehaviour, IEnemy
{
    ShieldmanAI shieldmanAI;

    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }

    bool shieldBroken = false;

    private void Awake()
    {
        shieldmanAI = GetComponent<ShieldmanAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 5;
        enemyType = IEnemy.EnemyType.normal;
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
        else if (!shieldBroken)
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

    IEnumerator ShieldmanStaggered()
    {
        //staggered only if the shield is broken
        isTakingDamage = true;
        animator.SetTrigger("animSoldierTakingDamage");
        yield return new WaitForSeconds(0.35f);
        isTakingDamage = false;
    }

    public IEnumerator Death()
    {
        isDead = true;
        Debug.Log("Shieldman died");
        animator.Play("deathAnimation");
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(shieldmanAI.spawn);
    }

}
