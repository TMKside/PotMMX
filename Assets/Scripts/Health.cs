using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public AudioClip collectable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the pickup is collected by the player
        if (other.CompareTag("Player"))
        {
            // Get the player's controller component
            PlayerController playerController = other.GetComponent<PlayerController>();


        }
    }
}