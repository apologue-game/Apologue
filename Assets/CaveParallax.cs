using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveParallax : MonoBehaviour
{
    PlayerControl playerControl;
    public Transform[] images;
    public float parallaxEffect = 0f;

    Vector3 newPosition;

    public static bool inCave = false;

    private void Start()
    {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    private void Update()
    {
        if (inCave)
        {
            if (playerControl.inputX > 0)
            {
                foreach (Transform image in images)
                {
                    newPosition = new Vector3(image.position.x - parallaxEffect * image.position.z / 500, image.position.y, image.position.z);
                    image.transform.position = newPosition;
                }
            }
            if (playerControl.inputX < 0)
            {
                foreach (Transform image in images)
                {
                    newPosition = new Vector3(image.position.x + parallaxEffect * image.position.z / 500, image.position.y, image.position.z);
                    image.transform.position = newPosition;
                }
            }
        }
    }
}
