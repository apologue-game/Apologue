using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableStopNearWalls : MonoBehaviour
{
    Throwable throwable;
    public Rigidbody2D rigidBody2D;
    public BoxCollider2D stopNearWallsCollider;

    private void Start()
    {
        throwable = GetComponentInParent<Throwable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grid"))
        {
            throwable.stop = true;
            rigidBody2D.velocity = Vector3.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Grid"))
        {
            throwable.stop = false;
        }
    }
}
