using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KarasuEntity;

public class SecondPlatformingSectionMovingPlatform : MonoBehaviour
{
    public Transform karasuTransform;
    public FixedJoint2D fixedJoint2D;
    public Rigidbody2D rigidBody2D;
    Vector2 move;
    public bool restarting = false;

    public int direction;
    public bool horizontalOrVerticalDirection;
    public bool courseSet = false;

    public bool activated = false;
    public float speed;

    public Transform[] positions;

    public int movingTowardsIndex = 0;

    private void Start()
    {
        transform.position = positions[0].position;
        //movingTowardsIndex++;
    }

    private void FixedUpdate()
    {
        if (dead)
        {
            if (!restarting)
            {
                restarting = true;
                StartCoroutine(Restart());
            }
            
            return;
        }
        if (movingTowardsIndex == 4)
        {
            speed = 0.03f;
        }
        else
        {
            speed = 0.06f;
        }

        if (movingTowardsIndex == 5)
        {
            if (karasuTransform.position.x < 278.02f)
            {
                //Too slow
                //ReloadSceenFromLastCheckpoint
            }
        }

        if (movingTowardsIndex == 6)
        {
            speed = 0.08f;
        }

        if (activated)
        {
            if (Vector2.Distance(transform.position, positions[movingTowardsIndex].position) < 0.05f)
            {
                movingTowardsIndex++;
                courseSet = false;
                if (movingTowardsIndex == positions.Length)
                {
                    activated = false;
                    return;
                }
            }

            if (horizontalOrVerticalDirection)
            {
                move = new Vector2(transform.position.x + direction * speed, transform.position.y);
            }
            if (!horizontalOrVerticalDirection)
            {
                move = new Vector2(transform.position.x, transform.position.y + direction * speed);
            }

            rigidBody2D.MovePosition(move);

        }
        if (!courseSet)
        {
            if (GameMaster.Utilities.IsFloatInRange(positions[movingTowardsIndex].position.y - 0.05f, positions[movingTowardsIndex].position.y + 0.05f, transform.position.y))
            {
                horizontalOrVerticalDirection = true;
            }
            if (GameMaster.Utilities.IsFloatInRange(positions[movingTowardsIndex].position.x - 0.05f, positions[movingTowardsIndex].position.x + 0.05f, transform.position.x))
            {
                horizontalOrVerticalDirection = false;
            }
            if (horizontalOrVerticalDirection)
            {
                if (transform.position.x < positions[movingTowardsIndex].position.x)
                {
                    direction = 1;
                }
                if (transform.position.x > positions[movingTowardsIndex].position.x)
                {
                    direction = -1;
                }
            }
            if (!horizontalOrVerticalDirection)
            {
                if (transform.position.y < positions[movingTowardsIndex].position.y)
                {
                    direction = 1;
                }
                if (transform.position.y > positions[movingTowardsIndex].position.y)
                {
                    direction = -1;
                }
            }
            courseSet = true;
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3.0f);
        horizontalOrVerticalDirection = false;
        direction = 0;
        courseSet = false;
        activated = false;
        transform.position = positions[0].position;
        movingTowardsIndex = 1;
        fixedJoint2D.enabled = false;
        //speed = starting speed
        restarting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "PlayerKarasu")
        {
            if (collision.transform.position.y > transform.position.y)
            {
                collision.transform.GetComponent<FixedJoint2D>().enabled = true;
                collision.transform.parent = transform;
                activated = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.name == "PlayerKarasu")
        {
            collision.transform.parent = null;
        }
    }
}

