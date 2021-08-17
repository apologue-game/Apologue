using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private Keyboard keyboard = Keyboard.current;
    Keyboard kb = InputSystem.GetDevice<Keyboard>();

    private void Update()
    {
        if (keyboard.leftArrowKey.wasPressedThisFrame)
        {
            Debug.Log("I'm pressing really hard right now");


        }
    }
}
