using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    Rigidbody2D parentPlatformRigidbody;
    Transform parentPlatformTransform;
    public GameObject fallingPlatformPrefab;

    Vector3 positionBeforeFalling;

    private void Awake()
    {
        parentPlatformRigidbody = GetComponentInParent<Rigidbody2D>();
        parentPlatformTransform = transform.parent;

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
        yield return new WaitForSeconds(1f);
        transform.parent.position = positionBeforeFalling;
        parentPlatformRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        parentPlatformRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        parentPlatformRigidbody.isKinematic = true;
        //Instantiate(fallingPlatformPrefab, positionBeforeFalling, parentPlatformTransform.rotation);
        //GameMaster.DestroyGameObject(transform.parent.gameObject);
    }
}
