using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    // Update is called once per frame
    void Start()
    {
        currentHealth = maxHealth;    
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log("Enemy died");

        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
    }
}
