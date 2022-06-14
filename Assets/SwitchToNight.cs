using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToNight : MonoBehaviour
{
    public GameObject duskSky;
    public GameObject duskMountain;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            duskSky.SetActive(false);
            duskMountain.SetActive(false);
        }
    }
}
