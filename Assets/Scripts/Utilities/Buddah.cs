using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buddah : Interactable
{
    GameObject canvas;
    public KarasuEntity karasuEntity;
    public GameObject healthBar;
    public Image healthBarFill;
    public Color healthBarColorInvulnerable;

    private void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        karasuEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<KarasuEntity>();

        healthBar = canvas.transform.Find("HealthBar").gameObject;
        healthBarFill = healthBar.transform.Find("Fill").GetComponent<Image>();
    }

    public override void Interact()
    {
        if (active)
        {
            active = false;
            if (karasuEntity.currentHealth == karasuEntity.maxHealth)
            {
                KarasuEntity.invulnerableToNextAttack = true;
                healthBarFill.color = healthBarColorInvulnerable;
            }
            else
            {
                karasuEntity.currentHealth = karasuEntity.maxHealth;
                healthBar.GetComponent<HealthBar>().SetHealth(karasuEntity.currentHealth);
            }
        }
    }
}