using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    System.Random rnd = new System.Random();

    FemaleArcherAI femaleArcherAI;
    KarasuEntity karasuEntity;
    Rigidbody2D rigidBody2D;
    float arrowForceDynamic = 0;
    float modifier;
    bool parried = false;
    public float deflectForce = 0;

    private void Awake()
    {
        femaleArcherAI = GetComponentInParent<FemaleArcherAI>();
        karasuEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<KarasuEntity>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        CalculateArrowForce();
        if (femaleArcherAI.facingLeft)
        {
            rigidBody2D.velocity = -transform.right * arrowForceDynamic;
        }
        else
        {
            rigidBody2D.velocity = transform.right * arrowForceDynamic;
        }
        StartCoroutine(DestroyItself());
    }

    private void CalculateArrowForce()
    {
        for (int i = 0; i < femaleArcherAI.hDistance; i++)
        {
            arrowForceDynamic += 0.7f;
        }
        for (int i = 0; i < femaleArcherAI.vDistance; i++)
        {
            arrowForceDynamic += 0.38f;
        }
        modifier = (float)rnd.Next(-10, 30);
        if (modifier > 0)
        {
            arrowForceDynamic += modifier / 10;
        }
        else if (modifier < 0)
        {
            arrowForceDynamic -= modifier / 10;
        }
        if (femaleArcherAI.hDistance > 11)
        {
            arrowForceDynamic += 1;
        }
        else if (femaleArcherAI.hDistance < 9)
        {
            arrowForceDynamic -= 1.5f;
        }
        if (femaleArcherAI.vDistance < 4)
        {
            arrowForceDynamic += arrowForceDynamic / 4;
        }
        else if (femaleArcherAI.hDistance > 8)
        {
            arrowForceDynamic -= 2f;
        }
        if (!femaleArcherAI.targetBeneathArcher)
        {
            arrowForceDynamic *= 4.3f;
        }
        else if (femaleArcherAI.targetInLine )
        {
            arrowForceDynamic *= 4.3f;
        }
    }

    private void Update()
    {
        float angle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "ParryCollider")
        {
            parried = true;
            Debug.Log("Parried an arrow");
            rigidBody2D.velocity = Vector2.zero;
            Vector2 direction = ((Vector2)femaleArcherAI.transform.position - rigidBody2D.position).normalized;
            Vector2 force = direction * deflectForce * Time.deltaTime;
            rigidBody2D.AddForce(force);
            return;
        }
        if (collision.name == "PlayerKarasu" && parried == false)
        {
            Debug.Log("Karasu was hit by an arrow");
            karasuEntity.TakeDamage(1);
            //particle effects
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<IEnemy>().TakeDamage(1);
            GameMaster.DestroyGameObject(gameObject);
        }
        if (collision.name == "GroundTilemap" || collision.name == "PlatformsTilemap" || collision.name == "WallTilemap")
        {
            //Debug.Log("The terrain was hit by an arrow");
            rigidBody2D.simulated = false;
            //particle effects
        }
    }

    IEnumerator DestroyItself()
    {
        yield return new WaitForSeconds(5f);
        GameMaster.DestroyGameObject(gameObject);
    }

    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
