using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeavyEnemy : MonoBehaviour, IEnemy
{
    HeavyEnemyAI heavyEnemyAI;
    public HealthBar healthBar;
    public Image healthBarFill;
    public Image healthBarBorder;
    public GameObject healthBarFillGO;
    public GameObject healthBarBorderGO;

    public GameObject heavyAxeRight;
    public GameObject heavyAxeLeft;
    Vector3 axeSpawnLocation;
    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public float currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }

    private void Awake()
    {
        heavyEnemyAI = GetComponent<HeavyEnemyAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 8;
        enemyType = IEnemy.EnemyType.elite;
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
        else
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
            StartCoroutine(HeavyEnemyStaggered());
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

    IEnumerator HeavyEnemyStaggered()
    {
        isTakingDamage = true;
        //play stagger animation
        yield return new WaitForSeconds(0.35f);
        isTakingDamage = false;
    }

    void SpawnPickableAxe()
    {
        if (heavyEnemyAI.facingLeft)
        {
            axeSpawnLocation = new Vector3(transform.position.x + 0.4f, transform.position.y - 0.52f, transform.position.z);
            Instantiate(heavyAxeRight, axeSpawnLocation, transform.rotation);
        }
        else
        {
            axeSpawnLocation = new Vector3(transform.position.x - 0.4f, transform.position.y - 0.52f, transform.position.z);
            Instantiate(heavyAxeLeft, axeSpawnLocation, transform.rotation);
        }
    }

    public IEnumerator Death()
    {
        isDead = true;
        animator.Play("deathAnimation");

        yield return new WaitForSeconds(1.1f);

        FadeOutHealthBars();
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(heavyEnemyAI.spawn);
    }
}
