using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public List<IEnemy> enemyList;
    public bool enemiesDefeated;
    public int enemyCount;

    public BoxCollider2D lockIn;

    private void Start()
    {
        enemyList = new List<IEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StaminaBar.inCombat = true;

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
                lockIn.enabled = false;
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
            }
        }
    }
}
