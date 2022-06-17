using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasicControls : MonoBehaviour
{
    public PlayerControl playerControl;

    public TMP_Text currentText;

    const string basicControlsKB = "Move: Arrow keys   Jump, Double jump: Space    Roll: LShift    Crouch toggle: LCtrl \n" +
                                    "Slide: LCtrl while moving    Parry: R    Interact: E    Pause menu: Esc\n" +
                                    "Switch attack stance: X    Toggle the visibility of controls: C\n";

    const string basicControlsGP = "Move: Left stick    Jump, Double jump: A    Roll: B    Crouch toggle: Left trigger \n" +
                                    "Slide: Left trigger while moving     Parry: Left shoulder     Interact: Right shoulder\n" +
                                    "Pause menu: Start button     Switch attack stance: D-pad up    Toggle the visibility of controls: D-Pad left\n";

    private void Update()
    {
        if (PlayerControl.playerInput.currentControlScheme == "Gamepad")
        {
            currentText.text = basicControlsGP;
        }
        else
        {
            currentText.text = basicControlsKB;
        }
    }
}
