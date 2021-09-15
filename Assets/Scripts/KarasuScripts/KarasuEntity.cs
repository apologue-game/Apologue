using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarasuEntity : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;

    private Color takeDamageColor = new Color(1f, 0.45f, 0.55f, 0.6f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);
    private float takeDamageTimer = 3;
    int maxHealth = 10;
    int currentHealth;

    // Update is called once per frame
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
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
        Debug.Log("Karasu died");
        spriteRenderer.color = normalColor;
        animator.SetTrigger("animDeath");
    }
}
