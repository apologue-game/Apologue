using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FemaleArcher : MonoBehaviour, IEnemy
{
    FemaleArcherAI femaleArcherAI;
    public HealthBar healthBar;
    public Image healthBarFill;
    public Image healthBarBorder;
    public GameObject healthBarFillGO;
    public GameObject healthBarBorderGO;
    public Image yellowHealthBarFill;
    public GameObject yellowHealthBarFillGO;
    public Image healthBarShadingFill;
    public GameObject healthBarShadingFillGO;

    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public float currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }
    public bool inCombat { get; set; }

    private void Awake()
    {
        femaleArcherAI = GetComponent<FemaleArcherAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 1;
        enemyType = IEnemy.EnemyType.ranged;
        inCombat = false;
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaximumHealth(maxHealth);

        healthBarFill.canvasRenderer.SetAlpha(0f);
        healthBarBorder.canvasRenderer.SetAlpha(0f);
        yellowHealthBarFill.canvasRenderer.SetAlpha(0f);
        healthBarShadingFill.canvasRenderer.SetAlpha(0f);
    }

    public void TakeDamage(float damage, bool? specialInteraction)
    {
        if (isDead)
        {
            return;
        }
        else
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                if (specialInteraction == true)
                {
                    StartCoroutine(DeathByArrow());
                    return;
                }
                StartCoroutine(Death());
                return;
            }
            //StartCoroutine(ArcherStaggered());
            ShowHealthBar();
        }
    }

    public void FadeOutHealthBars()
    {
        healthBarFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        healthBarBorderGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        yellowHealthBarFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        healthBarShadingFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
    }

    void ShowHealthBar()
    {
        healthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarBorder.canvasRenderer.SetAlpha(1f);
        yellowHealthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarShadingFill.canvasRenderer.SetAlpha(1f);
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
        animator.Play("deathAnimation");
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(femaleArcherAI.spawn);
    }

    public IEnumerator DeathByArrow()
    {
        isDead = true;
        animator.Play("deathByArrowAnimation");
        FadeOutHealthBars();
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(femaleArcherAI.spawn);
    }
}
