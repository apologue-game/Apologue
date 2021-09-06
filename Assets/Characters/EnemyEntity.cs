using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    private Color takeDamageColor = new Color(1f, 0.45f, 0.55f, 0.6f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);
    private float takeDamageTimer = 3;

    private SpriteRenderer spriteRenderer;
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
        currentHealth -= damage;
        spriteRenderer.color = takeDamageColor;
        takeDamageTimer = Time.time + 0.3f;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log("Enemy died");
        spriteRenderer.color = normalColor;

        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
    }
}
