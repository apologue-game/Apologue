using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KarasuEntity;

public class MovingPlatforms : MonoBehaviour
{
    public float speed;

    public Transform[] positions;

    public int movingTowardsIndex = 0;

    private void Start()
    {
        transform.position = positions[0].position;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, positions[movingTowardsIndex].position) < 0.05f)
        {
            movingTowardsIndex++;
            if (movingTowardsIndex == positions.Length)
            {
                movingTowardsIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, positions[movingTowardsIndex].position, speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "PlayerKarasu")
        {
            if (collision.transform.position.y > transform.position.y)
            {
                if (!dead)
                {
                    collision.transform.parent = transform;
                } 
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
