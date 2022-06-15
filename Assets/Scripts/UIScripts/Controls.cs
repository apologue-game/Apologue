using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public PlayerControl playerControl;

    public TMP_Text currentText;

    const string tSwordKB = "Light attacks: \n" +
                            "1. A \n" +
                            "2. S (Hold) \n" +
                            "3. D \n" +
                            "Dash attack: \n" +
                            "1. Q \n" +
                            "2. A (after hitting an enemy with 'Q')\n" +
                            "Special: \n" +
                            "1. W \n";

    const string tSwordGP = "Light attacks: \n" +
                            "1. X\n" +
                            "2. X (Hold)\n" +
                            "3. RB \n" +
                            "Dash attack: \n" +
                            "1. B + Y\n" +
                            "2. Y (after hitting an enemy with 'B + Y')\n" +
                            "Special: \n" +
                            "1. Y\n";


    const string tAxeKB =   "Light attacks: \n" +
                            "1. A \n" +
                            "2. S (hold) \n" +
                            "3. D (Hold) \n" +
                            "Dash attack: \n" +
                            "1. Q \n" +
                            "2. A (after hitting an enemy with 'Q')\n" +
                            "Heavy attack: \n" +
                            "1. W \n" +
                            "2. W (after hitting an enemy with 'W')\n";

    const string tAxeGP =   "Light attacks: \n" +
                            "1. X \n" +
                            "2. X (Hold)\n" +
                            "3. Y (Hold)\n" +
                            "Dash attack: \n" +
                            "1. B + Y\n" +
                            "2. Y (after hitting an enemy with 'B + Y')\n" +
                            "Heavy attack: \n" +
                            "1. Y\n" +
                            "2. Y (after hitting an enemy with 'Y')\n";

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
