using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hwacha : MonoBehaviour, IEnemy
{
    public HealthBar healthBar;
    public Image healthBarFill;
    public Image healthBarBorder;
    public GameObject healthBarFillGO;
    public GameObject healthBarBorderGO;
    BoxCollider2D hwachaCollider;
    BoxCollider2D playerCollider;


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
        hwachaCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 5;
        enemyType = IEnemy.EnemyType.ranged;
    }

    void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();

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
        else
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
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

    public IEnumerator Death()
    {
        isDead = true;
        animator.Play("death");
        yield return new WaitForSeconds(3.5f);
        Physics2D.IgnoreCollision(hwachaCollider, playerCollider);
        //GameMaster.DestroyGameObject(gameObject);
    }
}
