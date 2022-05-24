using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeStance : MonoBehaviour
{
    static ChangeStance changeStance;

    public Image katanaBar;
    public Image axeBar;

    Vector3 mainPosition;
    Vector3 sidePosition;

    bool activeState = true;

    Color transparency;

    private void Start()
    {
        changeStance = this;
        mainPosition = katanaBar.rectTransform.position;
        sidePosition = axeBar.rectTransform.position;

        transparency = new Color(1f, 1f, 1f, 0.45f);
        axeBar.color = transparency;
    }

    public static void ChangeCurrentStance()
    {
        if (changeStance.activeState)
        {
            changeStance.activeState = false;
            changeStance.katanaBar.rectTransform.position = changeStance.sidePosition;
            changeStance.katanaBar.color = changeStance.transparency;
            changeStance.axeBar.color = Color.white;
            changeStance.axeBar.rectTransform.position = changeStance.mainPosition;
            changeStance.katanaBar.rectTransform.SetAsFirstSibling();
            return;
        }

        changeStance.activeState = true;
        changeStance.axeBar.rectTransform.position = changeStance.sidePosition;
        changeStance.axeBar.color = changeStance.transparency;
        changeStance.katanaBar.color = Color.white;
        changeStance.katanaBar.rectTransform.position = changeStance.mainPosition;
        changeStance.katanaBar.rectTransform.SetAsLastSibling();
    }
}
