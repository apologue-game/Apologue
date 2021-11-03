using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slopes : MonoBehaviour
{
    PlayerControl playerControl;
    Transform karasuTransform;

    Vector3 rotation;

    private void Awake()
    {
        karasuTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        rotation = new Vector3(0, 0, -35f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerControl.onASlope = true;
            karasuTransform.Rotate(rotation);
            if (!playerControl.facingRight)
            {
                playerControl.Flip();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerControl.onASlope = true;
            if (!playerControl.facingRight)
            {
                playerControl.Flip();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerControl.onASlope = false;
            karasuTransform.Rotate(-rotation);
        }
    }
}
