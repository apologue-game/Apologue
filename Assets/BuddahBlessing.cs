using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuddahBlessing : MonoBehaviour
{
    public static bool blessingGiven = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !blessingGiven)
        {
            PlayerControl.ShowInteractionIcon();
            //Tooltip: Press E to receive a blessing
            //After pressing E, start the praying animation and receive + 1hp
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControl.HideInteractionIcon();
        }    
    }
}
