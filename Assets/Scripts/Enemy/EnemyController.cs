using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool isInvincible;

    public int currentHealth;
    public int maxHealth = 1;
    public int contactDamage = 1;
    public int bulletDamage = 1;
    public float bulletSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Incincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
            {
                Defeat();
            }
        }
    }

    void Defeat()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get the player's health system component
        HeartSystem playerHealth = other.gameObject.GetComponent<HeartSystem>();
        // If the player has a health system component, apply damage
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.HitSide(transform.position.x > player.transform.position.x);
            player.TakeDamage(this.contactDamage);
            Debug.Log("Ouch!");
        }
    }
}
