using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSwitch : MonoBehaviour
{
    public ElevatorButton buttonUP;
    public ElevatorButton buttonDOWN;

    public float distanceFromUp;
    public float distanceFromDown;

    public float speed;

    public Transform[] positions;

    public int movingTowardsIndex = 0;

    public bool activated = false;
    public Rigidbody2D rigidBody2D;
    public Vector2 move;
    public int direction;

    public float activationTimer = 0f;
    public bool currentPosition = true;

    private void Start()
    {
        transform.position = positions[0].position;
        movingTowardsIndex++;
    }


    private void FixedUpdate()
    {
        if (activated)
        {
            if (Vector2.Distance(transform.position, positions[movingTowardsIndex].position) < 0.05f)
            {
                movingTowardsIndex++;
                if (movingTowardsIndex == positions.Length)
                {
                    movingTowardsIndex = 0;
                }
                activated = false;
                buttonUP.activated = false;
                buttonDOWN.activated = false;
                activationTimer = Time.time + 0.1f;
            }
            if (transform.position.y < positions[movingTowardsIndex].position.y)
            {
                direction = 1;
            }
            else if (transform.position.y > positions[movingTowardsIndex].position.y)
            {
                direction = -1;
            }
            move = new Vector2(transform.position.x, transform.position.y + direction * speed);
            rigidBody2D.MovePosition(move);
            return;
        }

        if (transform.position.y - positions[0].position.y <= 0.15f)
        {
            currentPosition = true;
        }
        if (transform.position.y - positions[1].position.y <= 0.15f)
        {
            currentPosition = false;
        }
        if (buttonUP.activated && !currentPosition)
        {
            activated = true;
        }
        if (buttonDOWN.activated && currentPosition)
        {
            activated = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "PlayerKarasu")
        {
            if (Time.time > activationTimer)
            {
                activated = true;
            }
            
            if (collision.transform.position.y > transform.position.y)
            {
                collision.transform.parent = transform;
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
