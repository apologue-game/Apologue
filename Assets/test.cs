using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform respawnLocation;
    public Transform playerKarasu;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            playerKarasu.position = respawnLocation.position;
        }
    }
}
