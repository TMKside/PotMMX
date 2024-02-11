using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody; // Get player gameobject
    private PlayerInput input; // Get input system
    public Transform floorCheck; // Get floorcheck child object (requires editor)
    public Transform wallCheck; // Get wallcheck child object (requires editor)
    public LayerMask floorLayer; // Get floor layer (requires editor)

    private float moveDir; // Direction of movement
    public float speed = 0.075f; // Player default speed; can be adjusted in editor
    public float jumpHeight = 12f; // Player default jump height; can be adjusted in editor
    public float wallJumpPushForce; // Horizontal force when walljumping
    private bool isFacingRight = true; // Checks direction player is facing

    public int maxHealth = 10;
    public int currentHealth;

    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5;
    [SerializeField] Transform bulletShootPos;
    [SerializeField] GameObject bulletPrefab;

    Animator anim;

    AudioSource audioSource;
    public AudioClip backgroundMusicClip;
    public AudioClip damageSoundClip;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>(); // Auto get player gameobject
        input = GetComponent<PlayerInput>(); // Auto get input system
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component dynamically
        audioSource.clip = backgroundMusicClip; // Assign background music clip
        audioSource.loop = true; // Set background music to loop
        audioSource.Play();

        // Unused code:
        //PlayerInputActions playerInputActions = new PlayerInputActions();
        //playerInputActions.Player.Enable();
        //playerInputActions.Player.Jump.performed += Jump;
        //playerInputActions.Player.WalkRun.performed += WalkRun;
    }

    private void FixedUpdate()
    {
        playerRigidbody.transform.position = new Vector2(playerRigidbody.transform.position.x + (moveDir * speed), playerRigidbody.transform.position.y); // Moves player left or right when input is performed (will probably be replaced by an AddForce function)
        if (moveDir == 0) // Unused
        {
            //playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
        }

        // Flip player:
        if (!isFacingRight && moveDir > 0f) // If player is facing left and moves right,
        {
            Flip(); // Call Flip function
        }
        else if (isFacingRight && moveDir < 0f) // Else if player is facing right and moves left//
        {
            Flip(); // Call Flip function
        }
    }

    public void OnMovement(InputAction.CallbackContext context) // Input System move function
    {
        moveDir = context.ReadValue<float>(); // Movement direction equals value of 1D Axis
    }

    public void Jump(InputAction.CallbackContext context) // Input system jump function
    {
        if (context.performed && IsGrounded()) // If jump is performed and ground check is successful,
        {
            playerRigidbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse); // Add upward momentum based on jumpHeight variable
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootBullet();
            //Debug.Log("SHOOT!");
        }
    }

    private bool IsGrounded() // Checks if player is touching ground
    {
        return Physics2D.OverlapCircle(floorCheck.position, 0.2f, floorLayer); // Returns true if floorcheck child object overlaps floor
    }

    private void Flip() // Function flips player object horizontally
    {
        isFacingRight = !isFacingRight;
        Vector2 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletShootPos.position, Quaternion.identity);
        bullet.name = bulletPrefab.name;
        bullet.GetComponent<BulletScript>().SetDamageValue(bulletDamage);
        bullet.GetComponent<BulletScript>().SetBulletSpeed(bulletSpeed);
        bullet.GetComponent<BulletScript>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        bullet.GetComponent<BulletScript>().Shoot();
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

    void Die()
    {

        // Perform actions for player death, such as playing death animation, showing game over screen, etc.
        // For example:
        SceneManager.LoadScene("Menu");

    }
}