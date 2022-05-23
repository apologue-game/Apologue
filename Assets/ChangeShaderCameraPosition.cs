using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShaderCameraPosition : MonoBehaviour
{
    public Camera camera;
    public Transform newPosition;
    public Transform oldPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            if (camera.transform.position == newPosition.position)
            {
                camera.transform.position = oldPosition.position;
                return;
            }
            camera.transform.position = newPosition.position;
        }
    }
}
