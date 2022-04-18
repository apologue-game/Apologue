using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCluster 
{
    int clusterCounter = 0;
    string ClusterName { get; set; }
    List<IEnemy> ListOfEnemiesInACluster { get; set; }

    public EnemyCluster(GameObject[] enemyArray)
    {
        ClusterName = "cluster" + clusterCounter;
        ListOfEnemiesInACluster = new List<IEnemy>();
        foreach (GameObject enemy in enemyArray)
        {
            IEnemy enemyType = (IEnemy)enemy.GetComponent(typeof(IEnemy));
            ListOfEnemiesInACluster.Add(enemyType);
        }
        clusterCounter++;
    }

    public static void CreateNewCluster()
    {

    }
}
