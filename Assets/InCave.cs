using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InCave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CaveParallax.inCave = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CaveParallax.inCave = false;
        }
    }
}
