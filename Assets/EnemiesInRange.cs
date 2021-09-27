using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemiesInRange : MonoBehaviour
{
    KarasuEntity karasuEntity;
    bool inRange = false;
    SoldierAI soldierAI;
    private CircleCollider2D enemiesInRangeCollider2D;
    LayerMask enemiesLayers;

    private void Awake()
    {
        karasuEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<KarasuEntity>();
        soldierAI = GetComponentInParent<SoldierAI>();
        enemiesInRangeCollider2D = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            inRange = true;
            StartCoroutine("EnemiesInRangeCoroutine", collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            inRange = false;
        }
    }

    private IEnumerator EnemiesInRangeCoroutine(Collider2D collision)
    {
        if (karasuEntity.dead)
        {
            yield break;
            //celebrate
        }
        if (inRange && collision.name == "PlayerKarasu" && !Soldier.soldierDead)
        {
            soldierAI.SoldierAttackConditions();
            yield return new WaitForSeconds(0.1f);
            StartCoroutine("EnemiesInRangeCoroutine", collision);         
        }
        else if (collision.name == "ParryCollider" || collision.name == "BlockCollider")
        {
            Collider2D[] colliderHelper = Physics2D.OverlapCircleAll(enemiesInRangeCollider2D.transform.position, enemiesInRangeCollider2D.radius, enemiesLayers);
            foreach (Collider2D enemy in colliderHelper)
            {
                if (enemy.name == "PlayerKarasu")
                {
                    StartCoroutine("EnemiesInRangeCoroutine", enemy);
                }
            }
            yield break;
        }
        else
        {
            yield break;
        }
    }

    IEnumerator help()
    {
        if (inRange)
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("In range");
            StartCoroutine(help());
        }
        else
        {
            Debug.Log("Out of range");
            yield break;
        }
        
    }
}
