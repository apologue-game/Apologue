using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    public static bool blockedOrParried = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "SwordCollider" || collision.name == "SwordUppercutCollider" 
            || collision.name == "AxeCollider" || collision.name == "SpearCollider")
        {
            blockedOrParried = true;
        }
    }
}
