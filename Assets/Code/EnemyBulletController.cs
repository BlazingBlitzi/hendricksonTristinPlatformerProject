using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    bool isInvincible;
    public int currentHealth;
    public int maxHealth = 1;
    public int contactDamage = 1;
    SpriteRenderer sprite;
    Rigidbody2D rb2d;

    float destroyTime;

    [SerializeField] float destroyDelay;
    [SerializeField] Vector2 bulletDirection;
    [SerializeField] float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

    }


    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetDestoryDelay(float delay)
    {
        this.destroyDelay = delay;
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

    public void Shoot()
    {
        sprite.flipX = (bulletDirection.x < 0);
        rb2d.velocity = bulletDirection * bulletSpeed;
        destroyTime = destroyDelay;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
