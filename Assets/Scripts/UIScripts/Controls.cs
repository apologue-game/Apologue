using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public PlayerControl playerControl;

    public TMP_Text currentText;

    const string tSwordKB = "Light: \n" +
                            "1. Left mouse \n" +
                            "2. Left mouse (Hold) \n" +
                            "3. Right mouse (Hold) \n" +
                            "Medium: \n" +
                            "1. Number 2\n" +
                            "2. Left mouse (after first medium)\n" +
                            "Heavy: \n" +
                            "1. Right mouse\n" +
                            "2. Right mouse (after first heavy)\n";

    const string tSwordGP = "Light: \n" +
                            "1. X\n" +
                            "2. X (Hold)\n" +
                            "3. RB \n" +
                            "Medium: \n" +
                            "1. B + Y\n" +
                            "2. Y (after first medium)\n" +
                            "Heavy: \n" +
                            "1. Y\n" +
                            "2. Y (after first heavy)\n";


    const string tAxeKB = "Light: \n" +
                            "1. Left mouse \n" +
                            "2. Left mouse (Hold) \n" +
                            "3. Right mouse (Hold) \n" +
                            "Medium: \n" +
                            "1. Number 2\n" +
                            "2. Right mouse (after first medium)\n" +
                            "Heavy: \n" +
                            "1. Right mouse\n" +
                            "2. Right mouse (after first heavy)\n";

    const string tAxeGP = "Light: \n" +
                            "1. X \n" +
                            "2. X (Hold)\n" +
                            "3. Y (Hold)\n" +
                            "Medium: \n" +
                            "1. B + Y\n" +
                            "2. Y (after first medium)\n" +
                            "Heavy: \n" +
                            "1. Y\n" +
                            "2. Y (after first heavy)\n";

    private void Update()
    {
        if (playerControl.swordOrAxeStance)
        {
            if (PlayerControl.playerInput.currentControlScheme == "Gamepad")
            {
                currentText.text = tSwordGP;
            }
            else
            {
                currentText.text = tSwordKB;
            }
        }
        else
        {
            if (PlayerControl.playerInput.currentControlScheme == "Gamepad")
            {
                currentText.text = tAxeGP;
            }
            else
            {
                currentText.text = tAxeKB;
            }
        }
    }
}
