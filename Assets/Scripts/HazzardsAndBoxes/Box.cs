using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    Transform playerLocation;
    Rigidbody2D rigidBody2D;

    public enum BoxType
    {
        wooden,
        reinforcedWooden,
        metal
    }
    public BoxType boxType;

    public float pushForce;

    void Awake()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void MoveOrDestroy(bool lightOrHeavy)
    {
        if (boxType == BoxType.wooden)
        {
            if (lightOrHeavy)
            {
                if (playerLocation.position.x > transform.position.x)
                {
                    rigidBody2D.AddForce(Vector2.left * pushForce);
                }
                else if (playerLocation.position.x < transform.position.x)
                {
                    rigidBody2D.AddForce(Vector2.right * pushForce);
                }
            }
            if (!lightOrHeavy)
            {
                GameMaster.DestroyGameObject(gameObject);
            }
        }
        else if (boxType == BoxType.metal)
        {
            if (!lightOrHeavy)
            {
                if (playerLocation.position.x > transform.position.x)
                {
                    rigidBody2D.AddForce(Vector2.left * pushForce);
                }
                else if (playerLocation.position.x < transform.position.x)
                {
                    rigidBody2D.AddForce(Vector2.right * pushForce);
                }
            }
        }
    }
}
