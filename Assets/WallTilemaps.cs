using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallTilemaps : MonoBehaviour
{
    GameObject playerKarasu;
    Transform wallHangingCollider;
    public LayerMask walls;
    public int newPosition;
    public int oldPosition;

    private void Awake()
    {
        playerKarasu = GameObject.FindGameObjectWithTag("Player");
        wallHangingCollider = playerKarasu.transform.Find("WallHangingCollider").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            Collider2D[] inRangeToHang = Physics2D.OverlapCircleAll(wallHangingCollider.position, 0.38f, walls);
            if (inRangeToHang.Length > 0)
            {
                newPosition = (int)playerKarasu.transform.position.x;
                if (newPosition == oldPosition)
                {
                    return;
                }
                PlayerControl.hangingOnTheWall = true;
                PlayerControl.wallJump = true;
                playerKarasu.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(HangingOnTheWall());
            }
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
