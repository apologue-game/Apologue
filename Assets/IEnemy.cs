using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    Animator animator { get; set; }
    
    int maxHealth { get;  set; }
    int currentHealth { get; set; }

    bool isPartOfCluster { get; set; }

    bool isReadyToAttack { get; set; }

    bool isTakingDamage { get; set; }
    
    bool isDead { get; set; }

    enum EnemyType
    {
        weak,
        normal,
        ranged,
        flying,
        elite
    }
    
    EnemyType enemyType { get; set; }

    public void TakeDamage(int damage, bool? specialInteraction);
    IEnumerator Death();
}
