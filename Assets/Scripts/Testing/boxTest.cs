using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boxTest : MonoBehaviour, IEnemy
{
    public Animator animator { get; set; }

    public bool isDead { get; set; }
    public bool isTakingDamage { get; set; }

    public bool isPartOfCluster { get; set; }
    public bool isReadyToAttack { get; set; }

    public int maxHealth { get; set; }
    public float currentHealth { get; set; }
    public IEnemy.EnemyType enemyType { get; set; }
    public bool inCombat { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public HealthBar healthBar;
    public Image healthBarFill;
    public Image healthBarBorder;
    public GameObject healthBarFillGO;
    public GameObject healthBarBorderGO;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isDead = false;
        maxHealth = 50;
        enemyType = IEnemy.EnemyType.normal;
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaximumHealth(maxHealth);

        healthBarFill.canvasRenderer.SetAlpha(0f);
        healthBarBorder.canvasRenderer.SetAlpha(0f);
        ShowHealthBar();
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
            if (currentHealth <= 0)
            {
                currentHealth = maxHealth;
            }
            healthBar.SetHealth(currentHealth);
        }
    }

    void ShowHealthBar()
    {
        healthBarFill.canvasRenderer.SetAlpha(1f);
        healthBarBorder.canvasRenderer.SetAlpha(1f);
    }

    public IEnumerator Death()
    {
        //isDead = true;
        //Debug.Log("Soldier died");
        //animator.SetTrigger("animSoldierDeath");
        yield return new WaitForSeconds(3f);
        //GameMaster.DestroyGameObject(gameObject);
    }
}
