using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCollidersRespawn : MonoBehaviour
{
    public Transform respawnLocation;
    public Transform playerKarasu;

    private void Start()
    {
        playerKarasu = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            StartCoroutine(RespawnDelay());
        }
    }

    IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(1f);
        playerKarasu.position = respawnLocation.position;
    }
}
