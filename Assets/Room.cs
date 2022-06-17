using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<IEnemy> enemyList;
    public bool enemiesDefeated;
    public int enemyCount;

    public Animator exit;

    const string OPENGATEANIMATION = "openGate";
    const string CLOSEGATEANIMATION = "closeGate";
    const string IDLEENTRANCECLOSEDANIMATION = "idleEntranceClosed";
    const string IDLEENTRANCEOPENANIMATION = "idleEntrance";
    const string IDLEEXITANIMATION = "idleExit";
    const string OPENGATEEXITANIMATION = "openGateExit";
    const string IDLEEXITOPENANIMATION = "idleExitOpen";

    private void Start()
    {
        enemyList = new List<IEnemy>();
    }

    //TODO: Optimization: disable a room if it's finished
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StaminaBar.inCombat = true;
            //entrance.Play(CLOSEGATEANIMATION);
            //StartCoroutine(IdleAfterClosing());

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
                //entrance.Play(OPENGATEANIMATION);
                //StartCoroutine(IdleAfterOpeningEntrance());
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
                //entrance.Play(OPENGATEANIMATION);
                //StartCoroutine(IdleAfterOpeningEntrance());
                exit.Play(OPENGATEEXITANIMATION);
                StartCoroutine(IdleAfterOpeningExit());
            }
        }
    }

    IEnumerator IdleAfterClosing()
    {
        yield return new WaitForSeconds(0.517f);
        //entrance.Play(IDLEENTRANCECLOSEDANIMATION);
    }

    IEnumerator IdleAfterOpeningEntrance()
    {
        yield return new WaitForSeconds(0.517f);
        //entrance.Play(IDLEENTRANCEOPENANIMATION);
    }
    IEnumerator IdleAfterOpeningExit()
    {
        yield return new WaitForSeconds(0.517f);
        exit.Play(IDLEEXITOPENANIMATION);
        this.enabled = false;
    }

}
