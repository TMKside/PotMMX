using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossWinScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        WinGame();
    }

    private void WinGame()
    {
        // Load the game over scene
        SceneManager.LoadScene("WinScene");
    }
}