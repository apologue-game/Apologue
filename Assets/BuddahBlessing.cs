using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddahBlessing : MonoBehaviour
{
    KarasuEntity karasuEntity;

    private void Start()
    {
        karasuEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<KarasuEntity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Tooltip: Press E to receive a blessing
            //After pressing E, start the praying animation and receive + 1hp
            karasuEntity.currentHealth += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Remove the tooltip from the screen
        }    
    }
}
