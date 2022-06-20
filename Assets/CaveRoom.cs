using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveRoom : MonoBehaviour
{
    public Tilemap caveRoomTilemap;
    public SicklemanAI sicklemanAI;

    public List<IEnemy> enemyList;
    public bool enemiesDefeated;
    public int enemyCount;
    public float currentAlpha = 1f;
    public bool fadingOut = false;

    private void Start()
    {
        enemyList = new List<IEnemy>();
    }

    private void Update()
    {
        foreach (IEnemy enemy in enemyList)
        {
            if (enemy.isDead)
            {
                StaminaBar.inCombat = false;
                enemiesDefeated = true;
                caveRoomTilemap.GetComponent<TilemapCollider2D>().isTrigger = true;
                if (!fadingOut)
                {
                    StartCoroutine(TilemapFadeOut());
                }
            }
        }
    }

    //TODO: Optimization: disable a room if it's finished
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemiesDefeated)
        {
            return;
        }
        if (collision.CompareTag("Player"))
        {
            StaminaBar.inCombat = true;
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
                    //sicklemanAI.firstStrike = true;
                }
            }
            StaminaBar.inCombat = false;
        }
        //else if (collision.CompareTag("Enemy"))
        //{
        //    enemyList.Remove(collision.GetComponent<IEnemy>());
        //    enemyCount--;
        //    if (enemyCount == 0)
        //    {
        //        StaminaBar.inCombat = false;
        //        enemiesDefeated = true;
        //        StartCoroutine(TilemapFadeOut());
        //        caveRoomTilemap.GetComponent<TilemapCollider2D>().isTrigger = true;
        //    }
        //}
    }

    IEnumerator TilemapFadeOut()
    {
        fadingOut = true;
        yield return new WaitForSeconds(0.5f);
        for (int i = 100; i >= 0; i--)
        {
            currentAlpha -= 0.01f;
            caveRoomTilemap.color = new Color(1, 1, 1, currentAlpha);
            yield return new WaitForSeconds(0.01f);
        }
        //this.enabled = false;
    }

}
