using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTilemaps : MonoBehaviour
{
    GameObject playerKarasu;
    Animator karasuAnimator;


    private void Awake()
    {
        playerKarasu = GameObject.FindGameObjectWithTag("Player");
        karasuAnimator = playerKarasu.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (PlayerControl.hangingOnTheWall)
        {
            //karasuAnimator.SetBool("animHangingOnTheWall", true);
        }
        else
        {
            //karasuAnimator.SetBool("animHangingOnTheWall", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            PlayerControl.hangingOnTheWall = true;
            PlayerControl.wallJump = true;
            playerKarasu.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
            StartCoroutine(HangingOnTheWall());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            PlayerControl.hangingOnTheWall = false;
            PlayerControl.wallJump = false;
            playerKarasu.GetComponent<Rigidbody2D>().gravityScale = 3;
        }
    }

    IEnumerator HangingOnTheWall()
    {
        yield return new WaitForSeconds(PlayerControl.hangingOnTheWallTimer);
        if (PlayerControl.hangingOnTheWall)
        {
            playerKarasu.GetComponent<Rigidbody2D>().gravityScale = 3;
            PlayerControl.hangingOnTheWall = false;
            PlayerControl.wallJump = false;
        }
    }
}
