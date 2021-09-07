using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInteractions : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("ApologueDemo");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Application closed.");
    }
}
