using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PineappleController : MonoBehaviour
{
    // Get Unity objects
    private GameObject pappleObject;
    private Rigidbody2D pappleRigidbody;
    private Collider2D pappleCollider;
    public GameObject pappleVine; // Papple's vine structure
    public LayerMask playerLayer; // Player Unity layer
    public LayerMask groundLayer; // Ground layer

    // Initialize variables
    public int pappleHealth = 4; // Health of pineapple
    private int currentHealth; // Current health of pineapple
    public float aggroRadius = 5f; // Player detection radius
    public float fallDelay = 0.2f; // Delay between aggro and fall
    public float blastRadius = 5f; // Explosion damage radius
    public int blastDamage = 1; // Explosion damage amount

    private bool isAwake; // Checks if pineapple is awake
    private float fallCounter; // Internal fall delay timer

    public float gravityValue = 4f; // Force of gravity on aggro

    AudioSource audioSource;
    public AudioClip boom;


    // Start is called before the first frame update
    void Start()
    {
        pappleObject = GetComponent<GameObject>();
        pappleRigidbody = GetComponent<Rigidbody2D>();
        pappleCollider = GetComponent<Collider2D>();
        currentHealth = pappleHealth;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(BlastRadius());
        Debug.Log(fallCounter);
        Debug.Log(isAwake);

        AggRadius();
        Aggro();
        OnAggro();
    }

    private bool AggRadius()
    {
        return Physics2D.OverlapCircle(pappleVine.transform.position, aggroRadius, playerLayer);
    }

    private bool Aggro() // Methods wakes papple if conditions are met
    {
        if ((AggRadius() || currentHealth < pappleHealth) && !isAwake)
        {
            isAwake = true;
            fallCounter = fallDelay;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnAggro() // Actions to take when 
    {
        if (isAwake)
        {
            fallCounter -= Time.deltaTime;
        }

        if (isAwake && fallCounter <= 0f)
        {
            pappleRigidbody.gravityScale = gravityValue;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        audioSource.PlayOneShot(boom);
    }

    private void OnDestroy()
    {
        Collider2D other = BlastRadius();
        HeartSystem playerHealth = other.GetComponent<HeartSystem>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(blastDamage);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.HitSide(transform.position.x > player.transform.position.x);
            player.TakeDamage(this.blastDamage);
            Debug.Log("Ouch!");
        }
    }

    private Collider2D BlastRadius()
    {
        return Physics2D.OverlapCircle(pappleRigidbody.position, blastRadius, playerLayer);
    }
}
