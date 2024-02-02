using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

  Animator anim;
    
  private Rigidbody2D rd2d;
   
  public float speed;

  public int maxHealth = 10;
  public int currentHealth;

  AudioSource audioSource;
  public AudioClip backgroundMusicClip;
  public AudioClip damageSoundClip;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component dynamically
        audioSource.clip = backgroundMusicClip; // Assign background music clip
        audioSource.loop = true; // Set background music to loop
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
      // animation
       if (Input.GetKeyDown(KeyCode.D))

        {
          anim.SetInteger("State", 1);
        } 

        if (Input.GetKeyUp(KeyCode.D))

        {
          anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.A))

        {
          anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))

        {
          anim.SetInteger("State", 0);
        } 
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
    }


    public void PlaySound(AudioClip sound)
    {
        // Play the provided sound
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Reduce current health by the damage amount
        
        if (currentHealth <= 0)
        {
            Die(); // If health drops to or below 0, call the Die function
        }
      
       
    }

    public void PickupHealth(int healthAmount)
    {
        
        currentHealth += healthAmount; // Increase current health by the health amount
        // Ensure current health doesn't exceed max health
        
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    void Die()
    {
        
        // Perform actions for player death, such as playing death animation, showing game over screen, etc.
        // For example:
        SceneManager.LoadScene("Menu");

    }

}
