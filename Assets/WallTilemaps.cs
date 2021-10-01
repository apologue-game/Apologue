using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallTilemaps : MonoBehaviour
{
    GameObject playerKarasu;
    public int newPosition;
    public int oldPosition;

    private void Awake()
    {
        playerKarasu = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            newPosition = (int)playerKarasu.transform.position.x;
            if (newPosition == oldPosition /*|| Enumerable.Range(oldPosition - 1, oldPosition + 1).Contains(newPosition)*/)
            {
                return;
            }
            PlayerControl.hangingOnTheWall = true;
            PlayerControl.wallJump = true;
            playerKarasu.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(HangingOnTheWall());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            oldPosition = newPosition;
            PlayerControl.hangingOnTheWall = false;
            PlayerControl.wallJump = false;
            playerKarasu.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            StopAllCoroutines();
        }
    }

    IEnumerator HangingOnTheWall()
    {
        yield return new WaitForSeconds(PlayerControl.hangingOnTheWallTimer);
        if (PlayerControl.hangingOnTheWall)
        {
            playerKarasu.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            PlayerControl.wallJump = false;
        }
    }
}
