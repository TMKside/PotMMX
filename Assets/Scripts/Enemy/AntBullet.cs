using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntBullet : MonoBehaviour
{
    Rigidbody2D rb2d;

    float destroyTime;

    public int damage = 1;

    [SerializeField] float bulletSpeed;
    [SerializeField] Vector2 bulletDirection;
    [SerializeField] float destroyDelay;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetBulletSpeed(float speed)
    {
        this.bulletSpeed = speed;
    }

    public void SetBulletDirection(Vector2 direction)
    {
        this.bulletDirection = direction;
    }

    public void SetDamageValue(int damage)
    {
        this.damage = damage;
    }

    public void SetDestroyDelay(float delay)
    {
        this.destroyDelay = delay;
    }

    public void Shoot()
    {
        rb2d.velocity = bulletDirection * bulletSpeed;
        destroyTime = destroyDelay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get the player's health system component
        HeartSystem playerHealth = other.gameObject.GetComponent<HeartSystem>();
        // If the player has a health system component, apply damage
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.HitSide(transform.position.x > player.transform.position.x);
            player.TakeDamage(this.damage);
            Debug.Log("Ouch!");
        }
    }
}
