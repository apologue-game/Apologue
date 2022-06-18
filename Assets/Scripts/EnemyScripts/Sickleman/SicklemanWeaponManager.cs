using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackSystem;

public class SicklemanWeaponManager : MonoBehaviour
{
    SicklemanAI sicklemanAI;
    GameObject playerKarasu;
    GameObject parryCollider;
    PlayerControl playerControl;

    //Attacks
    //Sickleman basic attack
    public AttackSystem basicAttack;
    public AttackType basicAttackType = AttackType.normal;
    int basicAttackDamage = 32;
    //Sickleman scream attack
    public AttackSystem screamAttack;
    public AttackType screamAttackType = AttackType.special;
    int screamAttackDamage = 20;
    //Sickleman stomp attack
    public AttackSystem stompAttack;
    public AttackType stompAttackType = AttackType.onlyParryable;
    int stompAttackDamage = 43;  
    //Sickleman teleport attack
    public AttackSystem teleportStrikeAttack;
    public AttackType teleportStrikeAttackType = AttackType.onlyParryable;
    int teleportStrikeAttackDamage = 15;

    private void Start()
    {
        sicklemanAI = GetComponentInParent<SicklemanAI>();

        playerKarasu = GameObject.FindGameObjectWithTag("Player");
        parryCollider = playerKarasu.transform.Find("ParryCollider").gameObject;
        playerControl = playerKarasu.GetComponent<PlayerControl>();

        //Attack types
        basicAttack = new AttackSystem(basicAttackDamage, basicAttackType);
        screamAttack = new AttackSystem(screamAttackDamage, screamAttackType);
        stompAttack = new AttackSystem(stompAttackDamage, stompAttackType);
        teleportStrikeAttack = new AttackSystem(teleportStrikeAttackDamage, teleportStrikeAttackType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<KarasuEntity>() != null)
        {
            if (parryCollider.activeInHierarchy)
            {
                playerControl.staminaBar.currentStamina += 25;
                return;
            }
            if (sicklemanAI.currentDecision.Id == 0)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(basicAttack.AttackDamage, basicAttack.AttackMake);
            }
            else if (sicklemanAI.currentDecision.Id == 1)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(screamAttack.AttackDamage, screamAttack.AttackMake);
            }
            else if (sicklemanAI.currentDecision.Id == 2)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(stompAttack.AttackDamage, stompAttack.AttackMake);
            }
            else if (sicklemanAI.currentDecision.Id == 3)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(teleportStrikeAttack.AttackDamage, teleportStrikeAttack.AttackMake);
            }
        }
    }
}
