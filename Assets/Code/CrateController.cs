using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    bool isInvincible;

    

    public int currentHealth;
    public int maxHealth = 1;
    public int contactDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }


    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        if(!isInvincible)
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if(currentHealth <= 0)
            {
                Defeat();
            }
        }
    }

    void Defeat()
    {
        
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            player.HitSide(transform.position.x > player.transform.position.x);
            player.TakeDamage(this.contactDamage);
            Debug.Log("Player Hit");
        }
    }
}