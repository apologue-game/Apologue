using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToDusk : MonoBehaviour
{
    public GameObject daySky;
    public GameObject dayMountain;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            daySky.SetActive(false);
            dayMountain.SetActive(false);
        }
    }
}
