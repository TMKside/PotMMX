using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public AudioClip collectable;
    public int healthAmount = 2;
   
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.PickupHealth(healthAmount);
            
            Destroy(gameObject);
            
            controller.PlaySound(collectable);
        }
    }
}
