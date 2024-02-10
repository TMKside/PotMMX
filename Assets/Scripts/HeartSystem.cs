using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HeartSystem : MonoBehaviour
{
    public GameObject[] hearts;
    private int life;
    private bool dead;
    public int maxLife;
    public AudioClip healthPickupSound; // AudioClip for health pickup sound
    public AudioClip damageSound; // AudioClip for damage sound
    public bool invincible; // Flag to track player's invincibility state
    private AudioSource audioSource; // Reference to AudioSource component
    
    private void Start()
    {
        life = hearts.Length;
        maxLife = life;

        // Get the AudioSource component or add it dynamically if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        
    }

    void Update()
    {
        if (dead)
        {
            GameOver();
        }
    }


    public void TakeDamage(int d)
    {
        if (!invincible && life >= 1) // Check if the player is not invincible
        {
            life -= d; 
            hearts[life].gameObject.SetActive(false);

            if (damageSound != null) // Check if damage sound is assigned
            {
                // Play the damage sound
                audioSource.PlayOneShot(damageSound);
            }

            if (life < 1)
            {
                dead = true;
            }
        }
    }

    public void AddLife()
    {
        if (life < maxLife && dead == false)
        {
            hearts[life].gameObject.SetActive(true);
            life += 1;

            if (healthPickupSound != null) // Check if health pickup sound is assigned
            {
                // Play the health pickup sound
                audioSource.PlayOneShot(healthPickupSound);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Potion") && life < maxLife && dead == false)
        {
            AddLife();
            Destroy(coll.gameObject);
        }
    }

    // Method to toggle player invincibility
    public void ToggleInvincibility()
    {
        invincible = !invincible;
    }

    private void GameOver()
    {
        // Load the game over scene
        SceneManager.LoadScene("GameOverScene");
    }
}
