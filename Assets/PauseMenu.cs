using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    PlayerControl playerControl;

    private void Start()
    {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    public void Resume()
    {
        playerControl.pauseMenuPanel.SetActive(false);
        PlayerControl.playerInput.SwitchCurrentActionMap("Player");
        Time.timeScale = 1f;
        PlayerControl.isGamePaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
