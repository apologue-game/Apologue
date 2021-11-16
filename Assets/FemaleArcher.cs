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
        femaleArcherAI = GetComponent<FemaleArcherAI>();
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 1;
        enemyType = IEnemy.EnemyType.ranged;
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
            StartCoroutine(ShowHealthBar());
        }
    }
    public void FadeOutHealthBars()
    {
        healthBarFillGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        healthBarBorderGO.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
    }

    IEnumerator ShowHealthBar()
    {
        healthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarBorder.canvasRenderer.SetAlpha(1f);
        yield return new WaitForSeconds(1.5f);
        FadeOutHealthBars();
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
        Debug.Log("Archer died");
        animator.Play("deathAnimation");
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(femaleArcherAI.spawn);
    }

    public IEnumerator DeathByArrow()
    {
        isDead = true;
        Debug.Log("Archer died by deflection");
        animator.Play("deathByArrowAnimation");
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
        GameMaster.DestroyGameObject(femaleArcherAI.spawn);
    }
}
