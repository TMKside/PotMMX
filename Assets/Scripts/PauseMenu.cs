using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Controls;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool PlayerIsInvincible = false;

    public GameObject pauseMenuUI;
    public GameObject invincibilityButton;


    /* Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)  
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }*/

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    
    public void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");

    }

    public void QuitGame()
    {
        Debug.Log("quit...");
        Application.Quit();
    }

     public void ToggleInvincibility()
    {
        PlayerIsInvincible = !PlayerIsInvincible; 
        Debug.Log("invincible");
        
    }

    
}
