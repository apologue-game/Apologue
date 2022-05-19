using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SicklemanWeaponCollider : MonoBehaviour
{
    Throwable throwable;
    GameObject parryCollider;

    private void Awake()
    {
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
    }
}
