using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    bool isInvincible;


    public float range;
    public Transform target;

    

    public int currentHealth;
    public int maxHealth = 99999;
    public int contactDamage = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        
    }


    void Update()
    {
        
        
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
