using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<IEnemy> enemyList;
    public bool enemiesDefeated;
    public int enemyCount;

    public Transform entrance;
    public Transform exit;

    private void Start()
    {
        enemyList = new List<IEnemy>();
    }

    private void Update()
    {
        if (enemyCount == 0)
        {
            StaminaBar.inCombat = false;
            enemiesDefeated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StaminaBar.inCombat = true;
            if (enemyCount > 0)
            {
                entrance.gameObject.SetActive(true);
                exit.gameObject.SetActive(true);
            }

            foreach (IEnemy enemy in enemyList)
            {
                enemy.inCombat = true;
            }
        }
        else if (collision.CompareTag("Enemy"))
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
            }
            StaminaBar.inCombat = false;
            entrance.gameObject.SetActive(false);
            exit.gameObject.SetActive(false);

        }
        else if (collision.CompareTag("Enemy"))
        {
            enemyList.Remove(collision.GetComponent<IEnemy>());
            enemyCount--;
        }
    }
}
