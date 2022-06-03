using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeavyAxePickup : Interactable
{
    public GameObject axeBar;

    public override void Interact()
    {
        PlayerControl.axePickedUp = true;
        axeBar.SetActive(true);
        GameMaster.DestroyGameObject(gameObject);
    }
}
