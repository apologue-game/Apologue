using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shieldman : MonoBehaviour, IEnemy
{
    ShieldmanAI shieldmanAI;
    public AudioManager audioManager;

    public HealthBar healthBar;
    public Image healthBarFill;
    public Image healthBarBorder;
    public GameObject healthBarFillGO;
    public GameObject healthBarBorderGO;

    //BoxCollider2D boxCollider2D;
    public PolygonCollider2D polygonCollider2D;
    BoxCollider2D boxCollider2DKarasu;
    BoxCollider2D slideBoxCollider2DKarasu;

    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }

    public bool shieldBroken = false;
    public bool isBlocking = false;

    private void Awake()
    {
        shieldmanAI = GetComponent<ShieldmanAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 5;
        enemyType = IEnemy.EnemyType.normal;

        //boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2DKarasu = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        slideBoxCollider2DKarasu = GameObject.FindGameObjectWithTag("Player").transform.Find("SlideCollider").GetComponent<BoxCollider2D>();
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
        if (!shieldBroken)
        {
            if (specialInteraction == true)
            {
                shieldBroken = true;
                Physics2D.IgnoreCollision(polygonCollider2D, boxCollider2DKarasu);
                Physics2D.IgnoreCollision(polygonCollider2D, slideBoxCollider2DKarasu);
                StartCoroutine(ShieldmanStaggered());
                animator.Play("shieldbreakAnimation");
            }
            else
            {
                audioManager.PlaySound("swordStrikingWood");
                StartCoroutine(ShieldmanBlock());
            }
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
            StartCoroutine(ShieldmanStaggered());
            ShowHealthBar();
        }
    }

    //staggered only if the shield is broken
    IEnumerator ShieldmanStaggered()
    {
        isTakingDamage = true;
        //TODO: shieldman stagger animation
        yield return new WaitForSeconds(0.35f);
        isTakingDamage = false;
    }

    IEnumerator ShieldmanBlock()
    {
        isBlocking = true;
        yield return new WaitForSeconds(0.35f);
        isBlocking = false;
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
        Debug.Log("Shieldman died");
        animator.Play("deathAnimation");
        yield return new WaitForSeconds(3.83f);
        FadeOutHealthBars();
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(shieldmanAI.spawn);
    }
}
