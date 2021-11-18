using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAxePickup : Interactable
{
    public override void Interact()
    {
        PlayerControl.axePickedUp = true;
        GameMaster.DestroyGameObject(gameObject);
    }
}
