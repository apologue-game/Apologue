using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    Rigidbody2D parentPlatformRigidbody;

    Vector3 positionBeforeFalling;

    private void Awake()
    {
        parentPlatformRigidbody = GetComponentInParent<Rigidbody2D>();

        positionBeforeFalling = transform.parent.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parentPlatformRigidbody.isKinematic = false;
            StartCoroutine(RespawnFallingPlatform());
        }
    }

    IEnumerator RespawnFallingPlatform()
    {
        yield return new WaitForSeconds(2.5f);
        transform.parent.position = positionBeforeFalling;
        parentPlatformRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        parentPlatformRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        parentPlatformRigidbody.isKinematic = true;
    }
}
