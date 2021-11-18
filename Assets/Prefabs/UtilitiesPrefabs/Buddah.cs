using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buddah : Interactable
{
    private bool blessingGiven = false;
    public KarasuEntity karasuEntity;
    public HealthBar healthBar;
    public Image healthBarFill;
    public Color healthBarColorInvulnerable;

    public override void Interact()
    {
        if (!blessingGiven)
        {
            blessingGiven = true;
            if (karasuEntity.currentHealth == karasuEntity.maxHealth)
            {
                KarasuEntity.invulnerableToNextAttack = true;
                healthBarFill.color = healthBarColorInvulnerable;
            }
            else
            {
                karasuEntity.currentHealth = karasuEntity.maxHealth;
                healthBar.SetHealth(karasuEntity.currentHealth);
            }
        }
    }
}
