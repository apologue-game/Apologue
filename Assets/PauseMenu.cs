using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        transform.Find("PauseMenu").gameObject.SetActive(false);
        Time.timeScale = 1f;
        PlayerControl.isGamePaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
