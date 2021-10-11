using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommunication : MonoBehaviour
{
    EnemyCluster enemyCluster;
    int counter = 0;
    List<IEnemy> enemies = new List<IEnemy>();

    // Start is called before the first frame update
    void Start()
    {
        GetInstantiatedEnemies();
        InvokeRepeating("GetInstantiatedEnemies", 2f, 3f);
    }

    private void GetInstantiatedEnemies()
    {
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyArray)
        {
            IEnemy enemyType = (IEnemy)enemy.GetComponent(typeof(IEnemy));
            enemies.Add(enemyType);
        }
        if (enemyArray.Length > 1)
        {
            enemyCluster = new EnemyCluster(enemyArray);   
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
