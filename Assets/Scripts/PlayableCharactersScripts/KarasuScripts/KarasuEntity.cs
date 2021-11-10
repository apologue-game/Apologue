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

    //Dying
    float respawnDelay = 3f;
    public static bool dead = false;
    public static bool spikesDeath = false;

    //Taking damage
    float invincibilityWindow = 0.15f;
    float nextTimeVulnerable;
    bool invulnerable = false;

    //Animations
    string oldState;

    const string KARASUIDLEANIMATION = "karasuIdleAnimation";
    const string KARASUSTAGGERANIMATION = "karasuStaggerAnimation";

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
        if (damage == 500)
        {
            dead = true;
            spikesDeath = true;
            StartCoroutine(SpikesDeath());
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

    IEnumerator SpikesDeath()
    {
        PlayerControl.TurnOffControlsOnDeath();
        Debug.Log("SpikeDeath");
        AnimatorSwitchState("spikeDeathAnimation");
        Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        spriteRenderer.color = normalColor;
        yield return new WaitForSeconds(respawnDelay);
        KillPlayer();
    }

    IEnumerator Death()
    {
        PlayerControl.TurnOffControlsOnDeath();
        Debug.Log("Karasu died");
        spriteRenderer.color = normalColor;
        yield return new WaitForSeconds(respawnDelay);
        KillPlayer();
    }

    IEnumerator Stagger()
    {
        PlayerControl.TurnOffControlsOnDeath();
        Debug.Log("Karasu staggered");
        spriteRenderer.color = normalColor;
        AnimatorSwitchState(KARASUSTAGGERANIMATION);
        yield return new WaitForSeconds(0.2f);
        PlayerControl.TurnOnControlsOnRespawn();
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
        AnimatorSwitchState(KARASUIDLEANIMATION);
        currentHealth = maxHealth;
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
