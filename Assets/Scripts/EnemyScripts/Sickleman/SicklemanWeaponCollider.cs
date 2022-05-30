using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SicklemanWeaponCollider : MonoBehaviour
{
    Throwable throwable;
    GameObject parryCollider;
    Rigidbody2D rigidBody2D;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        throwable = GetComponent<Throwable>();
        parryCollider = GameObject.FindGameObjectWithTag("Player").transform.Find("ParryCollider").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (parryCollider.activeSelf)
            {
                throwable.hasDamaged = true;
                return;
            }
            if (!throwable.hasDamaged)
            {
                throwable.hasDamaged = true;
                collision.GetComponent<KarasuEntity>().TakeDamage(3, null);
            }
        }
        if (collision.CompareTag("Grid"))
        {
            rigidBody2D.velocity = Vector3.zero;
        }
    }
}
