using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boxTest : MonoBehaviour, IEnemy
{
    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public float currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 500;
        enemyType = IEnemy.EnemyType.normal;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, bool? specialInteraction)
    {
        if (isDead)
        {
            return;
        }
        if (BlockCollider.blockedOrParried)
        {
            Debug.Log("Soldier blocked an attack!!");
            return;
        }
        else
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                return;
            }
        }
    }


    public IEnumerator Death()
    {
        //isDead = true;
        //Debug.Log("Soldier died");
        //animator.SetTrigger("animSoldierDeath");
        yield return new WaitForSeconds(3f);
        //GameMaster.DestroyGameObject(gameObject);
    }
}
