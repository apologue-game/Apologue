using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        if (playerControl.swordOrAxeStance)
        {
            PlayerControl.playerInput.SwitchCurrentActionMap("PlayerSword");
        }
        else if (!playerControl.swordOrAxeStance)
        {
            PlayerControl.playerInput.SwitchCurrentActionMap("PlayerAxe");
        }
        Time.timeScale = 1f;
        PlayerControl.isGamePaused = false;
    }

    public void ReloadScene()
    {
        playerControl.pauseMenuPanel.SetActive(false);
        if (playerControl.swordOrAxeStance)
        {
            PlayerControl.playerInput.SwitchCurrentActionMap("PlayerSword");
        }
        else if (!playerControl.swordOrAxeStance)
        {
            PlayerControl.playerInput.SwitchCurrentActionMap("PlayerAxe");
        }
        Time.timeScale = 1f;
        PlayerControl.isGamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
