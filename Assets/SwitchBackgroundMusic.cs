using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBackgroundMusic : MonoBehaviour
{
    public AudioManager audioManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.name == "SlideCollider")
        {
            audioManager.PlaySound("bossTheme");
            audioManager.StopSound("darkVillageTheme");
        }
    }
}
