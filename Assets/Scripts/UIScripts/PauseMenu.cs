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

    private void Start()
    {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        startingPosition = playerControl.transform.position;
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
        playerControl.transform.position = startingPosition;
    } 
    
    public void ReloadSceneFromLastCheckpoint()
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
        PlayerControl.lastCheckpoint = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
