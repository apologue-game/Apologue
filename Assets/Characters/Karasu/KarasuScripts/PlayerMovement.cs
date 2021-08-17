using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool facingRight = true;
    private Transform groundCheck;
    const float groundedRadius = .01f; //.2f
    public bool grounded;
    private Transform ceilingCheck;
    const float ceilingRadius = .01f;
    private Rigidbody2D rigidbody2D;
    public float maxSpeed = 30f;

    private void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(Input.GetKey(KeyCode.LeftArrow) && facingRight)
            {
                Flip();
            }
            if (Input.GetKey(KeyCode.RightArrow) && !facingRight)
            {
                Flip();
            }

            Move();
        }
    }

    private void FixedUpdate()
    {

    }

    private void Move()
    {
        rigidbody2D.velocity = new Vector2(maxSpeed, rigidbody2D.velocity.y);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
