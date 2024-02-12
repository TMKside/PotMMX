using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameOver();
    }

    private void GameOver()
    {
        // Load the game over scene
        SceneManager.LoadScene("GameOverScene");
    }
}
