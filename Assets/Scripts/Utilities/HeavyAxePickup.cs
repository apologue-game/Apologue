using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeavyAxePickup : Interactable
{
    public GameObject axeBar;
    public SpriteRenderer spriteRenderer;

    public override void Interact()
    {
        PlayerControl.axePickedUp = true;
        axeBar.SetActive(true);
        active = false;
        spriteRenderer.enabled = false;
        //GameMaster.DestroyGameObject(gameObject);
    }
}
