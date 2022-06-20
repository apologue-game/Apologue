using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AttackSystem;

public class KarasuEntity : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public HealthBar healthBar;
    public Image healthBarFill;
    public static bool invulnerableToNextAttack = false;
    public Color healthBarColor;
    PlayerControl playerControl;
    public FixedJoint2D joint2D;

    private Color takeDamageColor = new Color(1f, 0.45f, 0.55f, 0.6f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);
    private float takeDamageTimer = 3;
    public bool takingDamage = false;
    public int maxHealth = 125;
    public float currentHealth;

    //Dying
    float respawnDelay = 3f;
    public static bool dead = false;
    public static bool spikesDeath = false;
    public static bool staggered = false;

    //Taking damage
    float invincibilityWindow = 0.2f;
    public float nextTimeVulnerable;
    bool invulnerable = false;

    public static Vector3 currentCheckpoint;

    //Animations
    string oldState;

    void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetMaximumHealth(maxHealth);

        currentCheckpoint = PauseMenu.startingPosition;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControl = GetComponentInParent<PlayerControl>();
    }

    private void Update()
    {
        if (Time.time >= takeDamageTimer && takingDamage) 
        {
            takingDamage = false;
            spriteRenderer.color = normalColor;
        }
        if (Time.time >= nextTimeVulnerable)
        {
            invulnerable = false;
        }
    }

    public void TakeDamage(float damage, AttackType? attackType)
    {
        if (damage == 500)
        {
            dead = true;
            spikesDeath = true;
            StartCoroutine(SpikesDeath());
            return;
        }
        if (playerControl.isRolling)
        {
            return;
        }
        if (invulnerableToNextAttack && damage != 501)
        {
            healthBarFill.color = healthBarColor;
            invulnerableToNextAttack = false;
            return;
        }
        if (Time.time > nextTimeVulnerable && !invulnerable)
        {
            if (attackType == AttackType.special)
            {
                StartCoroutine(Stagger());
            }
            invulnerable = true;
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            spriteRenderer.color = takeDamageColor;
            takingDamage = true;
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
        playerControl.animationState = PlayerControl.AnimationState.spikeDeath;
        spriteRenderer.color = normalColor;
        yield return new WaitForSeconds(respawnDelay);
        KillPlayer();
    }

    IEnumerator Death()
    {
        PlayerControl.TurnOffControlsOnDeath();
        spriteRenderer.color = normalColor;
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }

    IEnumerator Stagger()
    {
        PlayerControl.TurnOffControlsOnDeath();
        staggered = true;
        yield return new WaitForSeconds(0.3f);
        staggered = false;
        PlayerControl.TurnOnControlsOnRespawn();
    }

    void KillPlayer()
    {
        //GameMaster.KillPlayer(this);
        Respawn();
    }

    void Respawn()
    {
        PlayerControl.TurnOnControlsOnRespawn();
        playerControl.animationState = PlayerControl.AnimationState.idle;
        if (playerControl.isCrouching)
        {
            playerControl.isCrouching = false;
        }
        transform.position = currentCheckpoint;
        playerControl.attackState = PlayerControl.AttackState.notAttacking;
        currentHealth = maxHealth;
        healthBar.SetHealth(maxHealth);
        dead = false;
        spikesDeath = false;
        joint2D.enabled = false;
    }
}
