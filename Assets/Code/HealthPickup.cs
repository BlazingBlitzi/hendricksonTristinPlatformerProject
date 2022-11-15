using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    PlayerMovement playerHealth;

    public int healthBonus = 15;


    // Start is called before the first frame update
    void Awake()
    {
        playerHealth = FindObjectOfType<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (playerHealth.currentHealth < playerHealth.maxHealth)
            {
                Destroy(gameObject);
                playerHealth.currentHealth = playerHealth.currentHealth + healthBonus;
            }
        }
        
    }
}
