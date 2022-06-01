using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallTilemaps : MonoBehaviour
{
    GameObject playerKarasu;
    Transform wallHangingCollider;
    public LayerMask walls;
    public float wallHangingColliderRange = 0.45f;

    private void Start()
    {
        playerKarasu = GameObject.FindGameObjectWithTag("Player");
        wallHangingCollider = playerKarasu.transform.Find("WallHangingCollider").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            Collider2D[] inRangeToHang = Physics2D.OverlapCircleAll(wallHangingCollider.position, wallHangingColliderRange, walls);
            if (inRangeToHang.Length > 0)
            {
                PlayerControl.hangingOnTheWall = true;
                PlayerControl.wallJump = true;
                StartCoroutine(HangingOnTheWall());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            PlayerControl.hangingOnTheWall = false;
            PlayerControl.wallJump = false;
            StopAllCoroutines();
        }
    }

    IEnumerator HangingOnTheWall()
    {
        yield return new WaitForSeconds(PlayerControl.hangingOnTheWallTimer);
        if (PlayerControl.hangingOnTheWall)
        {
            PlayerControl.wallJump = false;
            PlayerControl.hangingOnTheWall = false;
        }
    }
}
