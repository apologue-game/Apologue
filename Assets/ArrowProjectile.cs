using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    FemaleArcherAI femaleArcherAI;
    KarasuEntity karasuEntity;
    Rigidbody2D rigidBody2D;
    int projectileType;
    public float arrowForce = 15;

    private void Awake()
    {
        femaleArcherAI = GetComponentInParent<FemaleArcherAI>();
        karasuEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<KarasuEntity>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (femaleArcherAI.facingLeft)
        {
            rigidBody2D.velocity = -transform.right * arrowForce;
        }
        else
        {
            rigidBody2D.velocity = transform.right * arrowForce;
        }
    }

    private void Update()
    {
        float angle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            Debug.Log("Karasu was hit by an arrow");
            karasuEntity.TakeDamage(1);
            //particle effects
            GameMaster.DestroyGameObject(gameObject);
        }
        if (collision.name == "GroundTilemap" || collision.name == "PlatformsTilemap" || collision.name == "WallTilemap")
        {
            Debug.Log("The terrain was hit by an arrow");
            //particle effects
            GameMaster.DestroyGameObject(gameObject);
        }
    }
}
