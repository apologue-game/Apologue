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
        healthBar.SetMaximumHealth(maxHealth);

        healthBarFill.canvasRenderer.SetAlpha(0f);
        healthBarBorder.canvasRenderer.SetAlpha(0f);
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
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
            StartCoroutine(SoldierStaggered());
            StartCoroutine(ShowHealthBar());
        }
    }

    public void FadeOutHealthBars()
    {
        healthBarFillGO.GetComponent<Image>().CrossFadeAlpha(0.1f, 2f, false);
        healthBarBorderGO.GetComponent<Image>().CrossFadeAlpha(0.1f, 2f, false);
    }

    IEnumerator ShowHealthBar()
    {
        healthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarBorder.canvasRenderer.SetAlpha(1f);
        yield return new WaitForSeconds(1.5f);
        FadeOutHealthBars();
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
