using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hwacha : MonoBehaviour, IEnemy
{
    HwachaAI hwachaAI;

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
        hwachaAI = GetComponent<HwachaAI>();
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
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
        }
    }

    public IEnumerator Death()
    {
        isDead = true;
        animator.Play("death");
        yield return new WaitForSeconds(3.5f);
        //GameMaster.DestroyGameObject(gameObject);
    }
}
