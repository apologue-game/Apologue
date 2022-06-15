using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCollidersRespawn : MonoBehaviour
{
    public Transform respawnLocation;
    public Transform playerKarasu;
    public bool spriteRendererDisable = false;
    public ParticleSystem waterSplash;

    private void Start()
    {
        playerKarasu = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            if (spriteRendererDisable)
            {
                waterSplash.transform.position = collision.transform.position;
                collision.GetComponent<SpriteRenderer>().enabled = false;
                waterSplash.Play();
            }
            StartCoroutine(RespawnDelay());
        }
    }

    IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(1f);
        playerKarasu.position = respawnLocation.position;
        playerKarasu.GetComponent<SpriteRenderer>().enabled = true;
        playerKarasu.GetComponent<PlayerControl>().isCrouching = false;
    }
}
