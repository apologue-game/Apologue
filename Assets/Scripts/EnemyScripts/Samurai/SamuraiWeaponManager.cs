using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackSystem;

public class SamuraiWeaponManager : MonoBehaviour
{
    SamuraiAI samuraiAI;
    GameObject playerKarasu;
    GameObject parryCollider;

    //Attacks
    //Samurai basic attack
    public AttackSystem basicAttack;
    public AttackType basicAttackType = AttackType.normal;
    int basicAttackDamage = 15;
    //Samurai dash strike attack
    public AttackSystem dashStrikeAttack;
    public AttackType dashStrikeAttackType = AttackType.normal;
    int dashStrikeAttackDamage = 12;

    //Samurai jump forward attack
    public AttackSystem jumpForwardAttack;
    public AttackType jumpForwardAttackType = AttackType.onlyParryable;
    public float jumpForceJumpForward = 0f;
    public float moveForceJumpForward = 0f;
    public float moveForceJumpForwardBaseValue = 0f;
    public float moveForceJumpForwardMultiplier = 0f;
    public bool currentlyJumpingForward = false;
    int jumpForwardAttackDamage = 25;

    private void Start()
    {
        samuraiAI = GetComponentInParent<SamuraiAI>();

        playerKarasu = GameObject.FindGameObjectWithTag("Player");
        parryCollider = playerKarasu.transform.Find("ParryCollider").gameObject;

        //Attack types
        basicAttack = new AttackSystem(basicAttackDamage, basicAttackType);
        dashStrikeAttack = new AttackSystem(dashStrikeAttackDamage, dashStrikeAttackType);
        jumpForwardAttack = new AttackSystem(jumpForwardAttackDamage, jumpForwardAttackType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<KarasuEntity>() != null)
        {
            if (parryCollider.activeInHierarchy)
            {
                samuraiAI.SamuraiParryStagger();
                return;
            }
            if (samuraiAI.attackDecision == SamuraiAI.AttackDecision.basic)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(basicAttack.AttackDamage, basicAttack.AttackMake);
            } 
            else if (samuraiAI.attackDecision == SamuraiAI.AttackDecision.dashStrike)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(dashStrikeAttack.AttackDamage, dashStrikeAttack.AttackMake);
            }
            else if (samuraiAI.attackDecision == SamuraiAI.AttackDecision.jumpForward)
            {
                collision.GetComponent<KarasuEntity>().TakeDamage(jumpForwardAttack.AttackDamage, jumpForwardAttack.AttackMake);
            }
        }
    }
}
