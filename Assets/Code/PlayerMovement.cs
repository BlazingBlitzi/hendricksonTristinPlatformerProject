using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    Animator animator;


    //Movement
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 10f;
    private bool isFacingRight = true;
    BoxCollider2D box2d;


    //Dashing
    [SerializeField] private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;


    //Shooting
    bool keyShoot;
    bool isShooting;
    bool keyShootRelease;
    float shootTime;

    //Audio
    AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip jumpSound;
    public AudioClip dashSound;

    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] Transform bulletShootPos;
    [SerializeField] GameObject bulletPrefab;

    //Health
    public int currentHealth;
    public int maxHealth = 28;
    public Text lifeText;
    public int lives;
    public Vector3 respawnPoint;
    public HealthBar healthBar;

    //Damage
    bool isTakingDamage;
    bool isInvincible;
    bool hitSideRight;

    void Start()
    {
        box2d = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        respawnPoint = transform.position;

        //Character Faces right be default
        isFacingRight = true;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (isTakingDamage)
        {
            animator.Play("Player_Hit");
            return;
        }


        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position);
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.C) && canDash)
        {
            AudioSource.PlayClipAtPoint(dashSound, Camera.main.transform.position);
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (lives == 0)
        {
            SceneManager.LoadScene(1);
        }




        Flip();

        PlayerShootInput();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "RespawnPoint")
        {
            respawnPoint = transform.position;
        }
    }

    void PlayerShootInput()
    {
        float shootTimeLength;
        float keyShootReleaseTimeLength = 0;

        //Shoot Key
        keyShoot = Input.GetKey(KeyCode.X);


        if (keyShoot && keyShootRelease)
        {
            isShooting = true;
            keyShootRelease = false;
            shootTime = Time.time;
            //Shoot Bullet
            Invoke("ShootBullet", 0.1f);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position);
            Debug.Log("Bang!");
        }
        if (!keyShoot && !keyShootRelease)
        {
            keyShootReleaseTimeLength = Time.time - shootTime;
            keyShootRelease = true;

        }
        if (isShooting)
        {
            shootTimeLength = Time.time - shootTime;
            if (shootTimeLength >= 0.25f || keyShootReleaseTimeLength >= 0.15f)
            {
                isShooting = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);


    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        isInvincible = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (transform.localScale.x < 0)
        {
            rb.velocity = new Vector2(-1 * dashingPower, 0f);
        }
        else
        {
            rb.velocity = new Vector2(1 * dashingPower, 0f);
        }
        
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        isInvincible = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletShootPos.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDamageValue(bulletDamage);
        bullet.GetComponent<Bullet>().SetBulletSpeed(bulletSpeed);
        bullet.GetComponent<Bullet>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        bullet.GetComponent<Bullet>().Shoot();
    }

    public void HitSide(bool rightSide)
    {
        hitSideRight = rightSide;
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        

        if (!isInvincible)
        {
            
           
            if (currentHealth <= 0)
            {
                transform.position = respawnPoint;
                
                currentHealth = maxHealth;
                
                LoseALife();
                
            }
            else
            {
                currentHealth -= damage;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                StartDamageAnimation();
            }

            healthBar.SetHealth(currentHealth);
        }
    }

    void StartDamageAnimation()
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            isInvincible = true;
            float hitForceX = 0.50f;
            float hitForceY = 1.5f;
            if (hitSideRight) hitForceX = -hitForceX;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
            animator.SetBool("isHit", true);
        }
    }

    void StopDamageAnimation()
    {
        isTakingDamage = false;
        isInvincible = false;
        animator.SetBool("isHit", false);
    }

    public void Defeat()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Elevator")
        {
            transform.parent = coll.gameObject.transform;
        }
    }

    public void LoseALife()
    {
        lives -= 1;
        lifeText.text = ": 0" + lives.ToString();
        
    }
}