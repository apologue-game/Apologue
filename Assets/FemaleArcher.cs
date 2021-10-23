using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleArcher : MonoBehaviour, IEnemy
{
    FemaleArcherAI femaleArcherAI;

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
        femaleArcherAI = GetComponent<FemaleArcherAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 5;
        enemyType = IEnemy.EnemyType.ranged;
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
                if (arrowDamage == true)
                {
                    StartCoroutine(DeathByArrow());
                    return;
                }
                StartCoroutine(Death());
                return;
            }
            //StartCoroutine(ArcherStaggered());
        }
    }

    //IEnumerator ArcherStaggered()
    //{
    //    isTakingDamage = true;
    //    animator.SetTrigger("animSoldierTakingDamage");
    //    yield return new WaitForSeconds(0.35f);
    //    isTakingDamage = false;
    //}

    public IEnumerator Death()
    {
        isDead = true;
        Debug.Log("Archer died");
        animator.Play("deathAnimation");
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(femaleArcherAI.spawn);
    }

    public IEnumerator DeathByArrow()
    {
        isDead = true;
        Debug.Log("Archer died by deflection");
        animator.Play("deathByArrowAnimation");
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(femaleArcherAI.spawn);
    }
}
