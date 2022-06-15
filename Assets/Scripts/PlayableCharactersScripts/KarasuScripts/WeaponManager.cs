using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    const float swordLight1Damage =  30;
    const float swordLight2Damage = 3;
    const float swordLight3Damage = 6;
    const float swordMedium2Damage = 4;
    const float swordMedium1Damage = 40;
    const float swordHeavy1Damage = 20;
    //const float swordHeavy2Damage = 40;
    const float axeLight1Damage = 45;
    const float axeLight2Damage = 4;
    const float axeLight3Damage = 70;
    const float axeMedium1Damage = 20;
    const float axeMedium2Damage = 100;
    const float axeHeavy1Damage = 20;
    const float axeHeavy2Damage = 60;

    public PlayerControl playerControl;

    public float heavyAttack1KnockupForce;

    public float comboTimer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IEnemy>() != null)
        {
            if (playerControl.attackState == PlayerControl.AttackState.lightAttackSword1)
            {
                collision.GetComponent<IEnemy>().TakeDamage(swordLight1Damage, false);
            }            
            else if (playerControl.attackState == PlayerControl.AttackState.lightAttackSword2)
            {
                collision.GetComponent<IEnemy>().TakeDamage(swordLight2Damage, false);
            }
            else if (playerControl.attackState == PlayerControl.AttackState.lightAttackSword3)
            {
                collision.GetComponent<IEnemy>().TakeDamage(swordLight3Damage, false);
            }
            else if (playerControl.attackState == PlayerControl.AttackState.heavyAttackSword1)
            {
                collision.GetComponent<IEnemy>().TakeDamage(swordHeavy1Damage, false);
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * heavyAttack1KnockupForce);
                playerControl.heavyAttackSword2_Available = true;
                StartCoroutine(ComboCountdown());
            }
            else if (playerControl.attackState == PlayerControl.AttackState.mediumAttackSword1)
            {
                collision.GetComponent<IEnemy>().TakeDamage(swordMedium1Damage, false);
                playerControl.mediumAttackSword2_Available = true;
                StartCoroutine(ComboCountdown());
            }
            else if (playerControl.attackState == PlayerControl.AttackState.mediumAttackSword2)
            {
                collision.GetComponent<IEnemy>().TakeDamage(swordMedium2Damage, false);
            }
            else if (playerControl.attackState == PlayerControl.AttackState.lightAttackAxe1)
            {
                collision.GetComponent<IEnemy>().TakeDamage(axeLight1Damage, true);
            }
            else if (playerControl.attackState == PlayerControl.AttackState.lightAttackAxe2)
            {
                collision.GetComponent<IEnemy>().TakeDamage(axeLight2Damage, false);
            }
            else if (playerControl.attackState == PlayerControl.AttackState.lightAttackAxe3)
            {
                collision.GetComponent<IEnemy>().TakeDamage(axeLight3Damage, false);
            }
            else if (playerControl.attackState == PlayerControl.AttackState.heavyAttackAxe1)
            {
                collision.GetComponent<IEnemy>().TakeDamage(axeHeavy1Damage, true);
                playerControl.heavyAttackAxe2_Available = true;
                StartCoroutine(ComboCountdown());
            }
            else if (playerControl.attackState == PlayerControl.AttackState.heavyAttackAxe2)
            {
                collision.GetComponent<IEnemy>().TakeDamage(axeHeavy2Damage, true);
            }
            else if (playerControl.attackState == PlayerControl.AttackState.mediumAttackAxe1)
            {
                collision.GetComponent<IEnemy>().TakeDamage(axeMedium1Damage, true);
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                playerControl.mediumAttackAxe2_Available = true;
                StartCoroutine(ComboCountdown());
            }
            else if (playerControl.attackState == PlayerControl.AttackState.mediumAttackAxe2)
            {
                collision.GetComponent<IEnemy>().TakeDamage(axeMedium2Damage, true);
            }
        }
    }

    IEnumerator ComboCountdown()
    {
        yield return new WaitForSeconds(comboTimer);
        playerControl.mediumAttackSword2_Available = false;
        playerControl.heavyAttackSword2_Available = false;
        playerControl.mediumAttackAxe2_Available = false;
        playerControl.heavyAttackAxe2_Available = false;
    }
}
