using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : Interactable
{
    public Sprite inactiveButton;
    public Sprite activeButton;
    public bool activated = false;
    public SpriteRenderer spriteRenderer;
    bool isPressed = false;

    public override void Interact()
    {
        if (!isPressed)
        {
            isPressed = true;
            if (!activated)
            {
                activated = true;
            }
            spriteRenderer.sprite = activeButton;
            StartCoroutine(PressedButton());
        }
    }
    
    IEnumerator PressedButton()
    {
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = inactiveButton;
        isPressed = false;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        spriteRenderer.sprite = activeButton;
    //        activated = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        spriteRenderer.sprite = inactiveButton;
    //        activated = false;
    //    }
    //}
}
