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
    public static int iDontWantToFightAnymoreCounter = 0;
    bool counterStarted = false;
    public int counterr;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            iDontWantToFightAnymoreCounter = 0;
            StopCoroutine(IDontWantToFightAnymore());
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

    private void Update()
    {
        counterr = iDontWantToFightAnymoreCounter;
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
            if (!counterStarted)
            {
                StartCoroutine(IDontWantToFightAnymore());
                counterStarted = true;
            }
        }
        if (collision.name == "PlatformsTilemap")
        {
            platformInJumpRange = false;
        }
    }

    IEnumerator IDontWantToFightAnymore()
    {
        yield return new WaitForSeconds(1f);
        iDontWantToFightAnymoreCounter++;
        StartCoroutine(IDontWantToFightAnymore());
    }
}
