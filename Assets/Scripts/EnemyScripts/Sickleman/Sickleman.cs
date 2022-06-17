using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sickleman : MonoBehaviour, IEnemy
{
    SicklemanAI sicklemanAI;
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
    public bool isStaggered { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public float currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }
    public bool inCombat { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 180;
        enemyType = IEnemy.EnemyType.elite;
        inCombat = false;
    }

    void Start()
    {
        sicklemanAI = GetComponent<SicklemanAI>();

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
    }

    void ShowHealthBar()
    {
        healthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarBorder.canvasRenderer.SetAlpha(1f);
        yellowHealthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarShadingFill.canvasRenderer.SetAlpha(1f);
    }

    public IEnumerator Death()
    {
        isDead = true;
        FadeOutHealthBars();
        yield return new WaitForSeconds(1f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(sicklemanAI.spawn);
    }
}
