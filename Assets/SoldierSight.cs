using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSight : MonoBehaviour
{
    public static bool inCombat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.name == "PlayerKarasu")
        {
            inCombat = true;
        }
        if (collision.name == "GroundTilemap" || collision.name == "WallTilemap")
        {
            SoldierAI.Jump();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            inCombat = false;
        }
    }
}
