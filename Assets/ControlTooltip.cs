using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlTooltip : MonoBehaviour
{
    public GameObject turnTooltipOnOrOff;
    public Image image1;
    public Image image2;
    public Sprite keyboardControl1;
    public Sprite gamepadControl1;
    public Sprite keyboardControl2;
    public Sprite gamepadControl2;
    public Text tooltipText1;
    public Text tooltipText2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.name == "SlideCollider")
        {
            tooltipText1.color = new Color(0.05f, 0.05f, 0.05f, 1f);
            if (tooltipText2 != null)
            {
                tooltipText2.color = new Color(0.05f, 0.05f, 0.05f, 1f);
            }
            if (PlayerControl.playerInput.currentControlScheme != "Gamepad")
            {
                if (image1 != null)
                {
                    image1.color = new Color(1f, 1f, 1f, 1f);
                    image1.sprite = keyboardControl1;
                }
                if (image2 != null)
                {
                    image2.color = new Color(1f, 1f, 1f, 1f);
                    image2.sprite = keyboardControl2;
                }
            }
            else
            {
                if (image1 != null)
                {
                    image1.color = new Color(1f, 1f, 1f, 1f);
                    image1.sprite = gamepadControl1;
                }
                if (image2 != null)
                {
                    image2.color = new Color(0f, 0f, 0f, 0f);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.name == "SlideCollider")
        {
            if (PlayerControl.playerInput.currentControlScheme != "Gamepad")
            {
                if (image1 != null)
                {
                    image1.color = new Color(1f, 1f, 1f, 1f);
                    image1.sprite = keyboardControl1;
                }
                if (image2 != null)
                {
                    image2.color = new Color(1f, 1f, 1f, 1f);
                    image2.sprite = keyboardControl2;
                }

            }
            else
            {
                if (image1 != null)
                {
                    image1.color = new Color(1f, 1f, 1f, 1f);
                    image1.sprite = gamepadControl1;
                }
                if (image2 != null)
                {
                    image2.color = new Color(0f, 0f, 0f, 0f);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.name == "SlideCollider")
        {
            if (image1 != null)
            {
                image1.color = new Color(0f, 0f, 0f, 0f);
            }
            if (image2 != null)
            {
                image2.color = new Color(0f, 0f, 0f, 0f);
            }
            tooltipText1.color = new Color(0f, 0f, 0f, 0f);
            if (tooltipText2 != null)
            {
                tooltipText2.color = new Color(0f, 0f, 0f, 0f);
            }
        }
    }
}
