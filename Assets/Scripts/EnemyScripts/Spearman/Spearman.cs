using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spearman : MonoBehaviour, IEnemy
{
    SpearmanAI spearmanAI;
    public EnemyHealthBar healthBar;
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
    public bool inCombat { get; set; }

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

        healthBarFill.canvasRenderer.SetAlpha(0f);
        healthBarBorder.canvasRenderer.SetAlpha(0f);
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
                shieldBreak = true;
                shield = false;
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
    }

    void ShowHealthBar()
    {
        healthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarBorder.canvasRenderer.SetAlpha(1f);
    }

    public IEnumerator Death()
    {
        isDead = true;
        yield return new WaitForSeconds(3f);
        FadeOutHealthBars();
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(spearmanAI.spawn);
    }
}
