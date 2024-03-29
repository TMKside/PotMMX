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

    public float moveDir; // Direction of movement
    public float speed = 0.075f; // Player default speed; can be adjusted in editor
    public float jumpHeight = 12f; // Player default jump height; can be adjusted in editor
    public float wallJumpPushForce; // Horizontal force when walljumping
    private bool isFacingRight = true; // Checks direction player is facing

    bool hitSideRight;
    bool isInvincible;
    bool isTakingDamage;


    public int maxHealth = 10;
    public int currentHealth;

    [Header("Bullet Settings")]
    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5;
    [SerializeField] Transform bulletShootPos;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject halfBulletPrefab;
    [SerializeField] GameObject fullBulletPrefab;
    [SerializeField] float chargeSpeed;
    [SerializeField] float chargeTime;
    bool isCharging;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 0.2f;
    [SerializeField] float dashDuration = 1f;
    bool isDashing;

    Animator anim;

    AudioSource audioSource;
    public AudioClip backgroundMusicClip;
    public AudioClip damageSoundClip;
    public AudioClip shoot1Clip;
    public AudioClip shoot2Clip;

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

    private void Update()
    {
        if (Input.GetKey(KeyCode.C) && chargeTime < 5)
        {
            isCharging = true;
            if (isCharging == true)
            {
                chargeTime += Time.deltaTime * chargeSpeed;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            return;
        }

        else if (Input.GetKeyUp(KeyCode.C))
        {
            if (chargeTime >= 2 && chargeTime < 3)
                ShootHalfBullet();
            else if (chargeTime >= 3)
                ShootFullBullet();
        }
    }

    private void FixedUpdate()
    {
        if (isTakingDamage)
        {
            anim.Play("Hit");
            return;
        }

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
        if (moveDir == 0f)
        {
            anim.Play("Idle");
        }
        if (moveDir != 0f)
        {
            anim.Play("Run");
        }
    }

    public void Jump(InputAction.CallbackContext context) // Input system jump function
    {
        if (context.performed && IsGrounded()) // If jump is performed and ground check is successful,
        {
            playerRigidbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse); // Add upward momentum based on jumpHeight variable
            anim.Play("Jump");
        }
        if (IsGrounded())
        {
            if (moveDir != 0)
            {
                anim.Play("Run");
            }
            else
            {
                anim.Play("Idle");
            }
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

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            StartCoroutine(Dash());
            //Debug.Log("Yeah?");
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        playerRigidbody.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        anim.Play("Dash");
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        anim.Play("Idle");
       // Debug.Log("YEAH!");
    }

    private bool IsGrounded() // Checks if player is touching ground
    {
        return Physics2D.OverlapCircle(floorCheck.position, 0.2f, floorLayer); // Returns true if floorcheck child object overlaps floor
        anim.Play("Idle");
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
        bullet.GetComponent<Bullet1>().SetDamageValue(bulletDamage);
        bullet.GetComponent<Bullet1>().SetBulletSpeed(bulletSpeed);
        bullet.GetComponent<Bullet1>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        bullet.GetComponent<Bullet1>().Shoot();
    }

    void ShootHalfBullet()
    {
        GameObject halfBullet = Instantiate(halfBulletPrefab, bulletShootPos.position, Quaternion.identity);
        halfBullet.name = halfBulletPrefab.name;
        halfBullet.GetComponent<Bullet2>().SetDamageValue(bulletDamage);
        halfBullet.GetComponent<Bullet2>().SetBulletSpeed(bulletSpeed);
        halfBullet.GetComponent<Bullet2>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        halfBullet.GetComponent<Bullet2>().Shoot();
        isCharging = false;
        chargeTime = 0;
    }

    void ShootFullBullet()
    {
        GameObject fullBullet = Instantiate(fullBulletPrefab, bulletShootPos.position, Quaternion.identity);
        fullBullet.name = fullBulletPrefab.name;
        fullBullet.GetComponent<Bullet3>().SetDamageValue(bulletDamage);
        fullBullet.GetComponent<Bullet3>().SetBulletSpeed(bulletSpeed);
        fullBullet.GetComponent<Bullet3>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        fullBullet.GetComponent<Bullet3>().Shoot();
        isCharging = false;
        chargeTime = 0;
    }

    public void PlaySound(AudioClip sound)
    {
        // Play the provided sound
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isInvincible && currentHealth >= 1)
        {
            currentHealth -= damageAmount; // Reduce current health by the damage amount

            if (currentHealth <= 0)
            {
                Die(); // If health drops to or below 0, call the Die function
            }
            else
            {
                StartDamageAnimation();
            }
        }
    }

    void StartDamageAnimation()
    {
        if (!isInvincible)
        {
            if (!isTakingDamage)
            {
                isTakingDamage = true;
                isInvincible = true;
                float hitForceX = 3f;
                float hitForceY = 10f;
                if (hitSideRight) hitForceX = -hitForceX;
                playerRigidbody.velocity = Vector2.zero;
                playerRigidbody.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
            }
        else
            return;
        }
    }

    void StopDamageAnimation()
    {
        isTakingDamage = false;
        isInvincible = false;
        anim.Play("Idle", -1, 0f);
    }

    public void HitSide(bool rightSide)
    {
        hitSideRight = rightSide;
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    void Die()
    {

        // Perform actions for player death, such as playing death animation, showing game over screen, etc.
        // For example:
        SceneManager.LoadScene("Menu");

    }
}