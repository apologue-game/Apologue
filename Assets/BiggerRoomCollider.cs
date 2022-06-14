using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerRoomCollider : MonoBehaviour
{
    public Room room;

    const string OPENGATEANIMATION = "openGate";
    const string OPENGATEEXITANIMATION = "openGateExit";
    const string IDLEENTRANCEOPENANIMATION = "idleEntrance";
    const string IDLEEXITOPENANIMATION = "idleExitOpen";

    private void Start()
    {
        room = GetComponentInParent<Room>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!room.enemiesDefeated)
            {
                foreach (IEnemy enemy in room.enemyList)
                {
                    enemy.inCombat = false;
                }
                room.entrance.Play(OPENGATEANIMATION);
                StartCoroutine(IdleAfterOpeningEntrance());
            }
            StaminaBar.inCombat = false;

        }
        else if (collision.GetComponent<IEnemy>() != null)
        {
            room.enemyList.Remove(collision.GetComponent<IEnemy>());
            room.enemyCount--;
            if (room.enemyCount == 0)
            {
                StaminaBar.inCombat = false;
                room.enemiesDefeated = true;
                room.entrance.Play(OPENGATEANIMATION);
                StartCoroutine(IdleAfterOpeningEntrance());
                room.exit.Play(OPENGATEEXITANIMATION);
                StartCoroutine(IdleAfterOpeningExit());
            }
        }
    }

    IEnumerator IdleAfterOpeningEntrance()
    {
        yield return new WaitForSeconds(0.517f);
        room.entrance.Play(IDLEENTRANCEOPENANIMATION);
    }

    IEnumerator IdleAfterOpeningExit()
    {
        yield return new WaitForSeconds(0.517f);
        room.exit.Play(IDLEEXITOPENANIMATION);
        this.enabled = false;
    }
}
