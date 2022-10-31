using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBehaviour : MonoBehaviour
{
    bool isInvincible;


    public AudioClip defeatSound;

    public int currentHealth;
    public int maxHealth = 1;
    

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
        if (!isInvincible)
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
            {
                Defeat();
            }
        }
    }

    void Defeat()
    {
        AudioSource.PlayClipAtPoint(defeatSound, Camera.main.transform.position);
        Destroy(gameObject);
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Defeat();
        }
    }
}
