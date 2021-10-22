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
    float oldDistance = 0;
    float newDistance;
    int archerIndex = 0;

    private void Awake()
    {
        GameObject[] archerList = GameObject.FindGameObjectsWithTag("Archer");
        if (archerList.Length > 0)
        {
            for (int i = 0; i < archerList.Length; i++)
            {
                newDistance = Mathf.Abs(transform.position.x - archerList[i].transform.position.x);
                if (oldDistance == 0)
                {
                    oldDistance = newDistance;
                }
                else if (oldDistance > newDistance)
                {
                    oldDistance = newDistance;
                    archerIndex = i;
                }
            }
        }
        femaleArcherAI = archerList[archerIndex].GetComponent<FemaleArcherAI>();
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
            rigidBody2D.velocity = Vector2.zero;
            Vector2 direction = ((Vector2)femaleArcherAI.transform.position - rigidBody2D.position).normalized;
            Vector2 force = direction * deflectForce * Time.deltaTime;
            rigidBody2D.AddForce(force);
            //particle effects
            return;
        }
        if (collision.name == "PlayerKarasu" && parried == false)
        {
            karasuEntity.TakeDamage(1);
            //particle effects
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<IEnemy>().TakeDamage(1, null);
            GameMaster.DestroyGameObject(gameObject);
        }
        if (collision.tag == "Archer")
        {
            collision.GetComponent<IEnemy>().TakeDamage(1, true);
            GameMaster.DestroyGameObject(gameObject);
        }
        if (collision.name == "GroundTilemap" || collision.name == "PlatformsTilemap" || collision.name == "WallTilemap")
        {
            rigidBody2D.simulated = false;
            //particle effects
        }
    }

    IEnumerator DestroyItself()
    {
        yield return new WaitForSeconds(5f);
        GameMaster.DestroyGameObject(gameObject);
    }
}
