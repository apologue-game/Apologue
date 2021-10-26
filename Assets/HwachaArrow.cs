using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HwachaArrow : MonoBehaviour
{
    HwachaAI hwachaAI;
    KarasuEntity karasuEntity;
    Rigidbody2D rigidBody2D;
    float arrowForce = 0;
    bool parried = false;
    public float deflectForce = 0;
    float oldDistance = 0;
    float newDistance;
    int hwachaIndex = 0;

    private void Awake()
    {
        GameObject[] hwachaList = GameObject.FindGameObjectsWithTag("Hwacha");
        if (hwachaList.Length > 0)
        {
            for (int i = 0; i < hwachaList.Length; i++)
            {
                newDistance = Mathf.Abs(transform.position.x - hwachaList[i].transform.position.x);
                if (oldDistance == 0)
                {
                    oldDistance = newDistance;
                }
                else if (oldDistance > newDistance)
                {
                    oldDistance = newDistance;
                    hwachaIndex = i;
                }
            }
        }
        hwachaAI = hwachaList[hwachaIndex].GetComponent<HwachaAI>();
        karasuEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<KarasuEntity>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidBody2D.velocity = -transform.right * 50;
        StartCoroutine(DestroyItself());
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
            Vector2 direction = ((Vector2)hwachaAI.transform.position - rigidBody2D.position).normalized;
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
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IEnemy>().TakeDamage(1, null);
            GameMaster.DestroyGameObject(gameObject);
        }
        if (collision.CompareTag("Archer"))
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
