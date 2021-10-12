using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSight : MonoBehaviour
{
    SoldierAI soldierAI;
    public LayerMask platformsLayers;
    public LayerMask ignoreCollisionsForJumping;
    public Transform platformJumping;
    float platformJumpingRange =  0.79f;

    public bool platformInJumpRange = false;
    public bool inCombat = false;
    public int jumpCounterSoldier = 0;
    public int iDontWantToFightAnymoreCounter = 0;
    bool counterStarted = false;

    private void Start()
    {
        soldierAI = GetComponentInParent<SoldierAI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(ignoreCollisionsForJumping))
        {
            return;
        }
        if (collision.name == "PlayerKarasu" || collision.name == "ParryCollider" || collision.name == "BlockCollider")
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
                    soldierAI.Jump();
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(ignoreCollisionsForJumping))
        {
            return;
        }
        if (!inCombat && jumpCounterSoldier == 0)
        {
            soldierAI.Jump();
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
