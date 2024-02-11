using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossWinScript : MonoBehaviour
{
    public GameObject boss; // Reference to the boss GameObject
    private bool hasWon = false; // Flag to track whether the player has won

    public string WinScene; // Name of the win scene to load

    void Update()
    {
        // Check if the boss is defeated and the player hasn't won yet
        if (!hasWon && boss == null)
        {
            WinGame(); // If the boss is defeated, trigger the win state
        }
    }

    // Function to handle win state
    void WinGame()
    {
        hasWon = true; // Set the flag to true to prevent further checks

        // Load the win scene
        SceneManager.LoadScene(WinScene);
    }
}