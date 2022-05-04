using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Samurai : MonoBehaviour, IEnemy
{
    SamuraiAI samuraiAI;
    public HealthBar healthBar;
    public Image healthBarFill;
    public Image healthBarBorder;
    public GameObject healthBarFillGO;
    public GameObject healthBarBorderGO;
    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }
    public bool isStaggered { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public float currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 15;
        enemyType = IEnemy.EnemyType.normal;
    }

    void Start()
    {
        samuraiAI = GetComponent<SamuraiAI>();

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

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            StartCoroutine(Death());
            return;
        }
        //if (!samuraiAI.currentlyAttacking)
        //{
        //    StartCoroutine(SamuraiStaggered()); //only if not currently attacking
        //}
        ShowHealthBar();
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

    IEnumerator SamuraiStaggered()
    {
        isTakingDamage = true;
        animator.SetTrigger("animSamuraiTakingDamage");
        yield return new WaitForSeconds(0.2f);
        isTakingDamage = false;
    }

    public void SamuraiParryStaggerCall()
    {
        StartCoroutine(SamuraiParryStaggered());
    }

    IEnumerator SamuraiParryStaggered()
    {
        isTakingDamage = true;
        animator.SetTrigger("animSamuraiTakingDamage");
        yield return new WaitForSeconds(1f);
        isTakingDamage = false;
    }

    public IEnumerator Death()
    {
        isDead = true;
        yield return new WaitForSeconds(3f);
        FadeOutHealthBars();
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(samuraiAI.spawn);
    }
}
