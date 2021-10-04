using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSight : MonoBehaviour
{
    public LayerMask platformsLayers;
    public Transform platformJumping;
    float platformJumpingRange =  0.79f;

    public static bool platformInJumpRange = false;
    public static bool inCombat = false;
    public static int jumpCounterSoldier = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlatformJumping") 
        {
            return;
        }
        if (collision.name == "PlayerKarasu")
        {
            inCombat = true;
        }
        if (collision.name == "GroundTilemap" || collision.name == "WallTilemap" || collision.name == "PlatformsTilemap")
        {
            if (!inCombat)
            {
                Collider2D[] platforms = Physics2D.OverlapCircleAll(platformJumping.position, platformJumpingRange, platformsLayers);
                if (platforms.Length > 0)
                {
                    platformInJumpRange = true;
                    SoldierAI.Jump();
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!inCombat && jumpCounterSoldier == 0)
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
        if (collision.name == "PlatformsTilemap")
        {
            platformInJumpRange = false;
        }
    }
}
