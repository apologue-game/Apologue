using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackSystem;

public class SpearmanWeaponManager : MonoBehaviour
{
    SpearmanAI spearmanAI;
    GameObject playerKarasu;
    GameObject parryCollider;

    //Attacks
    //Spearman basic attack
    public AttackSystem basicAttack;
    public AttackType basicAttackType = AttackType.normal;
    int basicAttackDamage = 3;
    //Spearman dash attack
    public AttackSystem dashAttack;
    public AttackType dashAttackType = AttackType.normal;
    int dashStrikeAttackDamage = 3;
    //Spearman shield bash attack
    public AttackSystem shieldBashAttack;
    public AttackType shieldBashAttackType = AttackType.normal;
    int jumpForwardAttackDamage = 3;    
    //Spearman flurry attack
    public AttackSystem flurryAttack;
    public AttackType flurryAttackType = AttackType.normal;
    int flurryAttackDamage = 3;

    bool parried = false;
    float parryTimer = 0f;

    private void Start()
    {
        spearmanAI = GetComponentInParent<SpearmanAI>();

        playerKarasu = GameObject.FindGameObjectWithTag("Player");
        parryCollider = playerKarasu.transform.Find("ParryCollider").gameObject;

        //Attack types
        basicAttack = new AttackSystem(basicAttackDamage, basicAttackType);
        dashAttack = new AttackSystem(dashStrikeAttackDamage, dashAttackType);
        shieldBashAttack = new AttackSystem(jumpForwardAttackDamage, shieldBashAttackType);
        flurryAttack = new AttackSystem(flurryAttackDamage, flurryAttackType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Spearman hit: " + collision.name);
        if (collision.GetComponent<KarasuEntity>() != null/* && !parried*/)
        {
            if (parryCollider.activeInHierarchy)
            {
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
