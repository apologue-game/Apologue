using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dasher : MonoBehaviour, IEnemy
{
    DasherAI dasherAI;
    public HealthBar healthBar;
    public Image healthBarFill;
    public Image healthBarBorder;
    public GameObject healthBarFillGO;
    public GameObject healthBarBorderGO;
    BoxCollider2D dasherBoxCollider;
    BoxCollider2D playerBoxCollider;

    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }
    public bool isStaggered = false;

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }

    private void Awake()
    {
        dasherAI = GetComponent<DasherAI>();
        animator = GetComponent<Animator>();
        dasherBoxCollider = GetComponent<BoxCollider2D>();
        isDead = false;
        maxHealth = 5;
        enemyType = IEnemy.EnemyType.ranged;
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaximumHealth(maxHealth);

        healthBarFill.canvasRenderer.SetAlpha(0f);
        healthBarBorder.canvasRenderer.SetAlpha(0f);

        playerBoxCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
    }

    public void TakeDamage(int damage, bool? specialInteraction)
    {
        if (isDead)
        {
            return;
        }
        else
        {
            if (isStaggered)
            {
                currentHealth -= damage * 5;
            }
            else
            {
                currentHealth -= damage;
            }
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
            StartCoroutine(DasherIsTakingDamage());
            ShowHealthBar();
        }
    }

    public void Stagger()
    {
        StartCoroutine(DasherStaggered());
    }

    public IEnumerator DasherIsTakingDamage()
    {
        isTakingDamage = true;
        yield return new WaitForSeconds(1.5f);
        isTakingDamage = false;
    }

    public IEnumerator DasherStaggered()
    {
        isStaggered = true;
        animator.Play("stagger");
        yield return new WaitForSeconds(1.5f);
        isStaggered = false;
    }

    void ShowHealthBar()
    {
        healthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarBorder.canvasRenderer.SetAlpha(1f);
    }

    public void FadeOutHealthBars()
    {
        healthBarFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, true);
        healthBarBorderGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, true);
    }

    public IEnumerator Death()
    {
        isDead = true;
        animator.Play("death");
        Physics2D.IgnoreCollision(dasherBoxCollider, playerBoxCollider);
        yield return new WaitForSeconds(3f);
        FadeOutHealthBars();
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(dasherAI.spawn);
    }
}
