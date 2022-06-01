using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFadeOut : MonoBehaviour
{
    public Tilemap tilemap;
    public bool fadeOut;
    public float currentAlpha = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fadeOut = true;
            StartCoroutine(TilemapFadeOutDelay());
        }
    }

    IEnumerator TilemapFadeOutDelay()   
    {
        for (int i = 100 - (int)(currentAlpha * 100); i < 100; i++)
        {
            if (!fadeOut)
            {
                yield break;
            }
            currentAlpha -= 0.01f;
            tilemap.color = new Color(1, 1, 1, currentAlpha);
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    IEnumerator TilemapFadeInDelay()
    {
        for (int i = 100 - (int)(currentAlpha * 100); i > 0; i--)
        {
            if (fadeOut)
            {
                yield break;
            }
            currentAlpha += 0.01f;
            tilemap.color = new Color(1, 1, 1, currentAlpha);
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fadeOut = false;
            StartCoroutine(TilemapFadeInDelay());
        }
    }
}
