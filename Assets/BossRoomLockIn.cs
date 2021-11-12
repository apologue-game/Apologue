using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomLockIn : MonoBehaviour
{
    public BoxCollider2D bossRoomBoxCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bossRoomBoxCollider.enabled = true;
        }
    }
}
