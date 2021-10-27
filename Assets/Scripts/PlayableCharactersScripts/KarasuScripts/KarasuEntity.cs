using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AttackSystem;

public class KarasuEntity : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;

    private Color takeDamageColor = new Color(1f, 0.45f, 0.55f, 0.6f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);
    private float takeDamageTimer = 3;
    int maxHealth = 500;
    public int currentHealth;

    //dying
    float respawnDelay = 3f;
    public bool dead = false;

    //taking damage
    float invincibilityWindow = 0.15f;
    float nextTimeVulnerable;
    bool invulnerable = false;

    // Update is called once per frame
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Time.time >= takeDamageTimer)
        {
            spriteRenderer.color = normalColor;
        }
    }

    public void TakeDamage(int damage, AttackType? attackType)
    {
        if (Time.time > nextTimeVulnerable && !invulnerable)
        {
            invulnerable = true;
            currentHealth -= damage;
            spriteRenderer.color = takeDamageColor;
            takeDamageTimer = Time.time + invincibilityWindow;
            nextTimeVulnerable = Time.time + invincibilityWindow;
        }
        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(Death());
        }
        invulnerable = false;
    }

    IEnumerator Death()
    {
        PlayerControl.TurnOffControlsOnDeath();
        Debug.Log("Karasu died");
        spriteRenderer.color = normalColor;
        animator.SetTrigger("animDeath");
        yield return new WaitForSeconds(respawnDelay);
        KillPlayer();
    }

    void KillPlayer()
    {
        Debug.Log("Kill karasu");
        GameMaster.KillPlayer(this);
        Respawn();
    }

    void Respawn()
    {
        PlayerControl.TurnOnControlsOnRespawn();
        animator.Play("karasuIdleAnimation");
        currentHealth = maxHealth;
        dead = false;
    }
}
