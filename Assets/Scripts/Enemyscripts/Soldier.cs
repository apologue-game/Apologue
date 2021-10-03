using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Animator animator;
    SpriteRenderer spriteRenderer;

    private Color takeDamageColor = new Color(1f, 0.45f, 0.55f, 0.6f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);

    private float takeDamageTimer;
    readonly int maxHealth = 50;
    int currentHealth;

    public static bool soldierDead = false;
    public static bool takingDamage = false;

    // Update is called once per frame
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Time.time >= takeDamageTimer)
        {
            spriteRenderer.color = normalColor;
        }
    }

    public void TakeDamage(int damage)
    {
        if (BlockCollider.blockedOrParried)
        {
            Debug.Log("Soldier blocked an attack!!");
        }
        else
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                StartCoroutine(Death());
                return;
            }
            StartCoroutine(SoldierStaggered());
            spriteRenderer.color = takeDamageColor;
            takeDamageTimer = Time.time + 0.15f;
        }

    }

    IEnumerator SoldierStaggered()
    {
        takingDamage = true;
        animator.SetTrigger("animSoldierTakingDamage");
        yield return new WaitForSeconds(0.333f);
        takingDamage = false;
    }

    IEnumerator Death()
    {
        soldierDead = true;
        Debug.Log("Soldier died");
        animator.SetTrigger("animSoldierDeath");
        yield return new WaitForSeconds(3.5f);
        GameMaster.DestroyGameObject(gameObject);
    }
}
