using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slopes : MonoBehaviour
{
    PlayerControl playerControl;
    Transform karasuTransform;
    Rigidbody2D rigidBody2D;

    bool exitInterruption;
    Vector3 rotation;

    private void Awake()
    {
        rotation = new Vector3(0, 0, -35f);
    }

    private void Start()
    {
        karasuTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        rigidBody2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            exitInterruption = true;
            playerControl.slopeXPosition = collision.transform.position.x;
            playerControl.onASlope = true;
            karasuTransform.Rotate(rotation);
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
            exitInterruption = false;
            karasuTransform.Rotate(-rotation);
            StartCoroutine(ExitDelay());
        }
    }

    IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(0.08f);
        if (!exitInterruption)
        {
            playerControl.onASlope = false;
        }
    }
}