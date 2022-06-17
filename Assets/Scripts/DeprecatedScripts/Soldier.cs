using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soldier : MonoBehaviour, IEnemy
{
    SoldierAI soldierAI;
    public HealthBar healthBar;
    public Image healthBarFill;
    public Image healthBarBorder;
    public GameObject healthBarFillGO;
    public GameObject healthBarBorderGO;

    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public float currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }
    public bool inCombat { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool isStaggered { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        soldierAI = GetComponent<SoldierAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 25;
        enemyType = IEnemy.EnemyType.normal;
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaximumHealth(maxHealth);

        healthBarFill.canvasRenderer.SetAlpha(0f);
        healthBarBorder.canvasRenderer.SetAlpha(0f);
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
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
            StartCoroutine(SoldierStaggered());
            ShowHealthBar();
        }
    }

    public void FadeOutHealthBars()
    {
        healthBarFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        healthBarBorderGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
    }

    void ShowHealthBar()
    {
        healthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarBorder.canvasRenderer.SetAlpha(1f);
    }

    IEnumerator SoldierStaggered()
    {
        isTakingDamage = true;
        animator.SetTrigger("animSoldierTakingDamage");
        yield return new WaitForSeconds(0.35f);
        isTakingDamage = false;
    }

    public void SoldierParryStaggerCall()
    {
        StartCoroutine(SoldierParryStaggered());
    }

    IEnumerator SoldierParryStaggered()
    {
        isTakingDamage = true;
        animator.SetTrigger("animSoldierTakingDamage");
        yield return new WaitForSeconds(1f);
        isTakingDamage = false;
    }

    public IEnumerator Death()
    {
        isDead = true;
        Debug.Log("Soldier died");
        animator.SetTrigger("animSoldierDeath");
        yield return new WaitForSeconds(3f);
        FadeOutHealthBars();
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(soldierAI.spawn);
    }
}
