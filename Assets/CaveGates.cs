using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGates : MonoBehaviour
{
    public List<IEnemy> enemyList;
    public bool enemiesDefeated;
    public int enemyCount;

    public Animator entrance;
    public Animator exit;
    public bool exitOpened = false;
    const string OPENGATEANIMATION = "openGate";
    const string CLOSEGATEANIMATION = "closeGate";
    const string IDLEENTRANCECLOSEDANIMATION = "idleEntranceClosed";
    const string IDLEENTRANCEOPENANIMATION = "idleEntrance";

    const string IDLEANIMATION = "idle";
    const string OPENANIMATION = "open";
    const string IDLEAFTEROPENINGGATESANIMATION = "idleAfterOpeningGates";

    private void Start()
    {
        enemyList = new List<IEnemy>();
    }

    private void Update()
    {
        if (PlayerControl.axePickedUp && !exitOpened)
        {
            exitOpened = true;
            exit.Play(OPENANIMATION);
            StartCoroutine(IdleAfterOpeningExit());
        }
    }

    //TODO: Optimization: disable a room if it's finished
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            if (enemyCount > 0)
            {
                StaminaBar.inCombat = true;
            }

            foreach (IEnemy enemy in enemyList)
            {
                enemy.inCombat = true;
            }
        }
        else if (collision.GetComponent<IEnemy>() != null)
        {
            enemyList.Add(collision.GetComponent<IEnemy>());
            enemyCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!enemiesDefeated)
            {
                foreach (IEnemy enemy in enemyList)
                {
                    enemy.inCombat = false;
                }
                StartCoroutine(IdleAfterOpeningEntrance());
            }
            StaminaBar.inCombat = false;

        }
        else if (collision.GetComponent<IEnemy>() != null)
        {
            enemyList.Remove(collision.GetComponent<IEnemy>());
            enemyCount--;
            if (enemyCount == 0)
            {
                StaminaBar.inCombat = false;
                enemiesDefeated = true;
                entrance.Play(OPENANIMATION);
                StartCoroutine(IdleAfterOpeningEntrance());
            }
        }
    }

    IEnumerator IdleAfterClosing()
    {
        yield return new WaitForSeconds(0.517f);
        //firstEntrance.Play(IDLEENTRANCECLOSEDANIMATION);
    }

    IEnumerator IdleAfterOpeningFirstEntrance()
    {
        yield return new WaitForSeconds(0.517f);
        //firstEntrance.Play(IDLEENTRANCEOPENANIMATION);
    }

    IEnumerator IdleAfterOpeningEntrance()
    {
        yield return new WaitForSeconds(0.8f);
        exit.Play(IDLEAFTEROPENINGGATESANIMATION);
    }

    IEnumerator IdleAfterOpeningExit()
    {
        yield return new WaitForSeconds(0.8f);
        exit.Play(IDLEAFTEROPENINGGATESANIMATION);
    }

}
