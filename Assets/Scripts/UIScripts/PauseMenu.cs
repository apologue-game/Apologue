using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameMaster gameMaster;

    PlayerControl playerControl;
    public static Vector3 currentPosition;
    public static Vector3 startingPosition;
    public GameObject axeBar;

    private void Start()
    {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        startingPosition = playerControl.transform.position;
        if (PlayerControl.axePickedUp)
        {
            axeBar.SetActive(true);
        }
    }

    public void Resume()
    {
        playerControl.pauseMenuPanel.SetActive(false);
        playerControl.wellDonePanel.SetActive(false);
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
        playerControl.wellDonePanel.SetActive(false);
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
    
    public void ReloadSceneFromLastCheckpoint()
    {
        playerControl.pauseMenuPanel.SetActive(false);
        playerControl.wellDonePanel.SetActive(false);
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
        PlayerControl.lastCheckpoint = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
