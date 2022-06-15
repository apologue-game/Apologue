using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spearman : MonoBehaviour, IEnemy
{
    SpearmanAI spearmanAI;
    public HealthBar healthBar;
    public Image healthBarFill;
    public Image healthBarBorder;
    public GameObject healthBarFillGO;
    public GameObject healthBarBorderGO;
    public Image yellowHealthBarFill;
    public GameObject yellowHealthBarFillGO;
    public Image healthBarShadingFill;
    public GameObject healthBarShadingFillGO;

    public HealthBar shieldHealthBar;
    public Image shieldHealthBarFill;
    public Image shieldHealthBarBorder;
    public GameObject shieldHealthBarFillGO;
    public GameObject shieldHealthBarBorderGO;
    public Image shieldBlueHealthBarFill;
    public GameObject shieldBlueHealthBarFillGO;
    public Image shieldHealthBarShadingFill;
    public GameObject shieldHealthBarShadingFillGO;

    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }
    public bool isStaggered { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public float currentHealth { get; set; }

    public IEnemy.EnemyType enemyType { get; set; }
    public bool inCombat { get; set; }

    public int shieldMaxHealth;
    public float currentShieldHealth;
    public static bool shield = true;
    public static bool blocking = false;
    public static bool shieldBreak = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 15;
        enemyType = IEnemy.EnemyType.normal;
        inCombat = false;
    }

    void Start()
    {
        spearmanAI = GetComponent<SpearmanAI>();

        currentHealth = maxHealth;
        healthBar.SetMaximumHealth(maxHealth);
        currentShieldHealth = shieldMaxHealth;
        shieldHealthBar.SetMaximumHealth(shieldMaxHealth);

        healthBarFill.canvasRenderer.SetAlpha(0f);
        healthBarBorder.canvasRenderer.SetAlpha(0f);
        yellowHealthBarFill.canvasRenderer.SetAlpha(0f);
        healthBarShadingFill.canvasRenderer.SetAlpha(0f);

        shieldHealthBarFill.canvasRenderer.SetAlpha(0f);
        shieldHealthBarBorder.canvasRenderer.SetAlpha(0f);
        shieldBlueHealthBarFill.canvasRenderer.SetAlpha(0f);
        shieldHealthBarShadingFill.canvasRenderer.SetAlpha(0f);
    }

    public void TakeDamage(float damage, bool? specialInteraction)
    {
        if (isDead)
        {
            return;
        }
        if (shield && !spearmanAI.staggered)
        {
            if (specialInteraction == true)
            {
                currentShieldHealth -= damage;
                shieldHealthBar.SetHealth(currentShieldHealth);
                ShowHealthBar();
                if (currentShieldHealth <= 0)
                {
                    shieldBreak = true;
                    shield = false;
                }
                return;
            }
            if (!spearmanAI.currentlyAttacking)
            {
                blocking = true;
            }
            return;
        }
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            StartCoroutine(Death());
            return;
        }
        ShowHealthBar();
    }

    public void FadeOutHealthBars()
    {
        healthBarFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        healthBarBorderGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        yellowHealthBarFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        healthBarShadingFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);

        shieldHealthBarFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        shieldHealthBarBorderGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        shieldBlueHealthBarFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        shieldHealthBarShadingFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
    }

    void ShowHealthBar()
    {
        healthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarBorder.canvasRenderer.SetAlpha(1f);
        yellowHealthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarShadingFill.canvasRenderer.SetAlpha(1f);

        shieldHealthBarFill.canvasRenderer.SetAlpha(1f);
        shieldHealthBarBorder.canvasRenderer.SetAlpha(1f);
        shieldBlueHealthBarFill.canvasRenderer.SetAlpha(1f);
        shieldHealthBarShadingFill.canvasRenderer.SetAlpha(1f);
    }

    public IEnumerator Death()
    {
        isDead = true;
        FadeOutHealthBars();
        yield return new WaitForSeconds(1f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(spearmanAI.spawn);
    }
}
