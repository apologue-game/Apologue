using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAxeDrop : MonoBehaviour
{
    Transform karasuTransform;
    public Transform groundCheck;
    public float groundCheckRange = 0.3f;
    public LayerMask whatIsGround;
    Rigidbody2D rigidBody2D;
    BoxCollider2D boxCollider2D;

    float distance;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        //groundCheck = GetComponentInChildren<Transform>();
    }

    private void Start()
    {
        karasuTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Show tooltip");
            //Show tooltip
            //If picked up, destroy prefab, PlayerControl.axePickedUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Remove tooltip
            Debug.Log("Remove tooltip");
        }
    }

    private void Update()
    {
        distance = Mathf.Abs(transform.position.x - karasuTransform.position.x);
        if (distance > 10)
        {
            //Karasu needs to go and pick up the axe
        }
        Collider2D[] collidersGround = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRange, whatIsGround);
        for (int i = 0; i < collidersGround.Length; i++)
        {
            if (collidersGround[i].name == "PlatformsTilemap" || collidersGround[i].name == "GroundTilemap" || collidersGround[i].name == "NothingTilemap" || collidersGround[i].CompareTag("Box") || collidersGround[i].CompareTag("FallingPlatforms"))
            {
                rigidBody2D.isKinematic = true;
                boxCollider2D.isTrigger = true;
                boxCollider2D.size = new Vector2(5, 5);
                rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRange);
    }
}
