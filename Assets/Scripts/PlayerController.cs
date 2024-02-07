using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    public Animator anim;

    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float jumpSpeed = 3f;

    float keyHorizontal;
    bool keyJump;
    public GameObject projectilePrefab;

    public int maxHealth = 10;
    public int currentHealth;

    AudioSource audioSource;
    public AudioClip backgroundMusicClip;
    public AudioClip damageSoundClip;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusicClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        keyJump = Input.GetKeyDown(KeyCode.X);
        if (keyJump)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }
    }

    // Update but for physics because consistency
    void FixedUpdate()
    {
        keyHorizontal = Input.GetAxisRaw("Horizontal");
        rb2d.velocity = new Vector2(keyHorizontal * moveSpeed, rb2d.velocity.y);
    }

    public void PlaySound(AudioClip sound)
    {
        // Play the provided sound
        AudioSource.PlayClipAtPoint(sound, transform.position);
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