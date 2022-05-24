using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShaderCameraPosition : MonoBehaviour
{
    public Camera shaderCamera;
    public Transform newPosition;
    public Transform oldPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerKarasu")
        {
            if (GetComponent<Camera>().transform.position == newPosition.position)
            {
                GetComponent<Camera>().transform.position = oldPosition.position;
                return;
            }
            GetComponent<Camera>().transform.position = newPosition.position;
        }
    }
}
