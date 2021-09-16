using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KarasuEntity : MonoBehaviour
{
    PlayerControl playerControl;
    Animator animator;
    SpriteRenderer spriteRenderer;

    private Color takeDamageColor = new Color(1f, 0.45f, 0.55f, 0.6f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);
    private float takeDamageTimer = 3;
    int maxHealth = 5;
    public int currentHealth;

    //dying
    float respawnDelay = 1.5f;
    public bool dead = false;

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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        spriteRenderer.color = takeDamageColor;
        takeDamageTimer = Time.time + 0.3f;
        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            StartCoroutine("Death");
        }
    }

    IEnumerator Death()
    {
        PlayerControl.TurnOffControlsOnDeath();
        Debug.Log("Karasu died");
        spriteRenderer.color = normalColor;
        animator.SetTrigger("animDeath");
        yield return new WaitForSeconds(3);
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
