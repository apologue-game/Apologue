using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AttackSystem;

public class KarasuEntity : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public HealthBar healthBar;
    public Image healthBarFill;
    public static bool invulnerableToNextAttack = false;
    public Color healthBarColor;

    private Color takeDamageColor = new Color(1f, 0.45f, 0.55f, 0.6f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);
    private float takeDamageTimer = 3;
    public int maxHealth = 15;
    public int currentHealth;

    //Dying
    float respawnDelay = 3f;
    public static bool dead = false;
    public static bool spikesDeath = false;

    //Taking damage
    float invincibilityWindow = 0.2f;
    public float nextTimeVulnerable;
    bool invulnerable = false;

    //Animations
    string oldState;

    const string KARASUIDLEANIMATION = "karasuIdleAnimation";
    const string KARASUSTAGGERANIMATION = "karasuStaggerAnimation";

    void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetMaximumHealth(maxHealth);
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
        if (Time.time >= nextTimeVulnerable)
        {
            invulnerable = false;
        }
    }

    public void TakeDamage(int damage, AttackType? attackType)
    {
        if (damage == 500)
        {
            dead = true;
            spikesDeath = true;
            StartCoroutine(SpikesDeath());
            return;
        }
        if (invulnerableToNextAttack)
        {
            healthBarFill.color = healthBarColor;
            invulnerableToNextAttack = false;
            return;
        }
        if (Time.time > nextTimeVulnerable && !invulnerable)
        {
            if (attackType == AttackType.onlyParryable || attackType == AttackType.special)
            {
                StartCoroutine(Stagger());
            }
            invulnerable = true;
            currentHealth -= damage;
            Debug.Log(currentHealth);
            healthBar.SetHealth(currentHealth);
            spriteRenderer.color = takeDamageColor;
            takeDamageTimer = Time.time + invincibilityWindow;
            nextTimeVulnerable = Time.time + invincibilityWindow;
        }
        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(Death());
        }
    }

    IEnumerator SpikesDeath()
    {
        PlayerControl.TurnOffControlsOnDeath();
        AnimatorSwitchState("spikeDeathAnimation");
        spriteRenderer.color = normalColor;
        yield return new WaitForSeconds(respawnDelay);
        KillPlayer();
    }

    IEnumerator Death()
    {
        PlayerControl.TurnOffControlsOnDeath();
        spriteRenderer.color = normalColor;
        yield return new WaitForSeconds(respawnDelay);
        KillPlayer();
    }

    IEnumerator Stagger()
    {
        PlayerControl.TurnOffControlsOnDeath();
        spriteRenderer.color = normalColor;
        AnimatorSwitchState(KARASUSTAGGERANIMATION);
        yield return new WaitForSeconds(0.2f);
        PlayerControl.TurnOnControlsOnRespawn();
    }

    void KillPlayer()
    {
        GameMaster.KillPlayer(this);
        Respawn();
    }

    void Respawn()
    {
        PlayerControl.TurnOnControlsOnRespawn();
        AnimatorSwitchState(KARASUIDLEANIMATION);
        currentHealth = maxHealth;
        healthBar.SetHealth(maxHealth);
        dead = false;
        spikesDeath = false;
    }

    public void AnimatorSwitchState(string newState)
    {
        if (oldState == newState)
        {
            return;
        }

        animator.Play(newState);

        oldState = newState;
    }
}
