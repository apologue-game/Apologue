using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackSystem;

public class SpearmanWeaponManager : MonoBehaviour
{
    SpearmanAI spearmanAI;
    GameObject playerKarasu;
    GameObject parryCollider;
    PlayerControl playerControl;

    //Attacks
    //Spearman basic attack
    public AttackSystem basicAttack;
    public AttackType basicAttackType = AttackType.normal;
    int basicAttackDamage = 15;
    //Spearman dash attack
    public AttackSystem dashAttack;
    public AttackType dashAttackType = AttackType.normal;
    int dashStrikeAttackDamage = 10;
    //Spearman shield bash attack
    public AttackSystem shieldBashAttack;
    public AttackType shieldBashAttackType = AttackType.special;
    int jumpForwardAttackDamage = 0;    
    //Spearman flurry attack
    public AttackSystem flurryAttack;
    public AttackType flurryAttackType = AttackType.normal;
    int flurryAttackDamage = 15;

    private void Start()
    {
        spearmanAI = GetComponentInParent<SpearmanAI>();

        playerKarasu = GameObject.FindGameObjectWithTag("Player");
        parryCollider = playerKarasu.transform.Find("ParryCollider").gameObject;
        playerControl = playerKarasu.GetComponent<PlayerControl>();

        //Attack types
        basicAttack = new AttackSystem(basicAttackDamage, basicAttackType);
        dashAttack = new AttackSystem(dashStrikeAttackDamage, dashAttackType);
        shieldBashAttack = new AttackSystem(jumpForwardAttackDamage, shieldBashAttackType);
        flurryAttack = new AttackSystem(flurryAttackDamage, flurryAttackType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<KarasuEntity>() != null)
        {
            if (parryCollider.activeInHierarchy)
            {
                playerControl.staminaBar.currentStamina += 25;
                spearmanAI.SpearmanParryStagger();
                return;
            }
            if (spearmanAI.attackDecision == SpearmanAI.AttackDecision.basic)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(basicAttack.AttackDamage, basicAttack.AttackMake);
            }
            else if (spearmanAI.attackDecision == SpearmanAI.AttackDecision.dashAttack)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(dashAttack.AttackDamage, dashAttack.AttackMake);
            }
            else if (spearmanAI.attackDecision == SpearmanAI.AttackDecision.shieldBash)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(shieldBashAttack.AttackDamage, shieldBashAttack.AttackMake);
            } 
            else if (spearmanAI.attackDecision == SpearmanAI.AttackDecision.attackFlurry)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(flurryAttack.AttackDamage, flurryAttack.AttackMake);
            }
        }
    }
}
