using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackSystem;

public class SicklemanWeaponManager : MonoBehaviour
{
    SicklemanAI sicklemanAI;
    GameObject playerKarasu;
    GameObject parryCollider;

    //Attacks
    //Sickleman basic attack
    public AttackSystem basicAttack;
    public AttackType basicAttackType = AttackType.normal;
    int basicAttackDamage = 3;
    //Sickleman scream attack
    public AttackSystem screamAttack;
    public AttackType screamAttackType = AttackType.special;
    int screamAttackDamage = 3;
    //Sickleman stomp attack
    public AttackSystem stompAttack;
    public AttackType stompAttackType = AttackType.onlyParryable;
    int stompAttackDamage = 3;  
    //Sickleman stomp attack
    public AttackSystem teleportStrikeAttack;
    public AttackType teleportStrikeAttackType = AttackType.onlyParryable;
    int teleportStrikeAttackDamage = 3;

    private void Start()
    {
        sicklemanAI = GetComponentInParent<SicklemanAI>();

        playerKarasu = GameObject.FindGameObjectWithTag("Player");
        parryCollider = playerKarasu.transform.Find("ParryCollider").gameObject;

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
                return;
            }
            //if (sicklemanAI.attackDecision == SicklemanAI.AttackDecision.basic)
            //{
            //    collision.GetComponent<KarasuEntity>().TakeDamage(basicAttack.AttackDamage, basicAttack.AttackMake);
            //}
            //else if (sicklemanAI.attackDecision == SicklemanAI.AttackDecision.scream)
            //{
            //    collision.GetComponent<KarasuEntity>().TakeDamage(screamAttack.AttackDamage, screamAttack.AttackMake);
            //}
            //else if (sicklemanAI.attackDecision == SicklemanAI.AttackDecision.stomp)
            //{
            //    collision.GetComponent<KarasuEntity>().TakeDamage(stompAttack.AttackDamage, stompAttack.AttackMake);
            //}            
            //else if (sicklemanAI.attackDecision == SicklemanAI.AttackDecision.teleportStrike)
            //{
            //    collision.GetComponent<KarasuEntity>().TakeDamage(teleportStrikeAttack.AttackDamage, teleportStrikeAttack.AttackMake);
            //}
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
