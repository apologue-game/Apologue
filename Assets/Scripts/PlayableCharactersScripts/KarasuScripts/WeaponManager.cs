using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    const float swordLight1Damage =  4;
    const float swordLight2Damage = 2;
    const float swordLight3Damage = 2;
    const float swordHeavy1Damage = 2;
    const float swordHeavy2Damage = 3.5f;
    const float swordMedium1Damage = 1.3f;
    const float swordMedium2Damage = 2.4f;
    const float axeLight1Damage = 2.3f;
    const float axeLight2Damage = 3.5f;
    const float axeLight3Damage = 4.5f;
    const float axeHeavy1Damage = 3.5f;
    const float axeHeavy2Damage = 5;
    const float axeMedium1Damage = 1.8f;
    const float axeMedium2Damage = 2.75f;

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
            else if (playerControl.attackState == PlayerControl.AttackState.heavyAttackSword2)
            {
                collision.GetComponent<IEnemy>().TakeDamage(swordHeavy2Damage, false);
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
                collision.GetComponent<IEnemy>().TakeDamage(axeLight1Damage, false);
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
                collision.GetComponent<IEnemy>().TakeDamage(axeHeavy1Damage, false);
                playerControl.heavyAttackAxe2_Available = true;
                StartCoroutine(ComboCountdown());
            }
            else if (playerControl.attackState == PlayerControl.AttackState.heavyAttackAxe2)
            {
                collision.GetComponent<IEnemy>().TakeDamage(axeHeavy2Damage, false);
            }
            else if (playerControl.attackState == PlayerControl.AttackState.mediumAttackAxe1)
            {
                collision.GetComponent<IEnemy>().TakeDamage(axeMedium1Damage, false);
                playerControl.mediumAttackAxe2_Available = true;
                StartCoroutine(ComboCountdown());
            }
            else if (playerControl.attackState == PlayerControl.AttackState.mediumAttackAxe2)
            {
                collision.GetComponent<IEnemy>().TakeDamage(axeMedium2Damage, false);
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
