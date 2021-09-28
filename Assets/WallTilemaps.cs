using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTilemaps : MonoBehaviour
{
    GameObject playerKarasu;

    private void Awake()
    {
        playerKarasu = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (PlayerControl.hangingOnTheWall)
        {

        }
        else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            PlayerControl.hangingOnTheWall = true;
            PlayerControl.wallJump = true;
            playerKarasu.GetComponent<Rigidbody2D>().gravityScale = 3;
            StartCoroutine(HangingOnTheWall());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            PlayerControl.hangingOnTheWall = false;
            PlayerControl.wallJump = false;
            playerKarasu.GetComponent<Rigidbody2D>().gravityScale = 5;
        }
    }

    IEnumerator HangingOnTheWall()
    {
        yield return new WaitForSeconds(PlayerControl.hangingOnTheWallTimer);
        if (PlayerControl.hangingOnTheWall)
        {
            playerKarasu.GetComponent<Rigidbody2D>().gravityScale = 5;
            PlayerControl.hangingOnTheWall = false;
            PlayerControl.wallJump = false;
        }
    }
}
