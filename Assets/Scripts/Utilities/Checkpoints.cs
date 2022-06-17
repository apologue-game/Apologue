using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PauseMenu.currentPosition = transform.position;
            KarasuEntity.currentCheckpoint = transform.position;
        }
    }
}
