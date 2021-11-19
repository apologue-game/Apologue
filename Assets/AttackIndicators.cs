using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackIndicators : MonoBehaviour
{
    public GameObject turnTooltipOnOrOff;
    public Image image1;
    public Image image2;
    public Image image3;
    public Sprite keyboardIcon1;
    public Sprite keyboardIcon2;
    public Sprite keyboardIcon3;
    public Sprite gamepadIcon1;
    public Sprite gamepadIcon2;
    public Sprite gamepadIcon3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.name == "SlideCollider")
        {
            turnTooltipOnOrOff.SetActive(true);
            PlayerControl.playerInput.SwitchCurrentActionMap("UI");
            Time.timeScale = 0f;
            PlayerControl.isGamePaused = true;
            if (PlayerControl.playerInput.currentControlScheme == "Gamepad")
            {
                image1.sprite = gamepadIcon1;
                image2.sprite = gamepadIcon2;
                image3.sprite = gamepadIcon3;
            }
            else
            {
                image1.sprite = keyboardIcon1;
                image2.sprite = keyboardIcon2;
                image3.sprite = keyboardIcon3;
            }
        }
    }
}
