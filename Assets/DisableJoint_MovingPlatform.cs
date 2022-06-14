using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableJoint_MovingPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "PlayerKarasu")
        {
            if (collision.transform.parent is not null)
            {
                collision.gameObject.GetComponent<FixedJoint2D>().enabled = false;
                PlayerControl.dontEnableJoint = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.name == "PlayerKarasu")
        {
            if (collision.transform.parent is null)
            {
                PlayerControl.dontEnableJoint = false;
            }
        }
    }
}
