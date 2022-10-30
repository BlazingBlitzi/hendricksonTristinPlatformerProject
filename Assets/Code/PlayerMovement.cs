using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    BoxCollider2D box2d;
    

    float keyHorizontal;
    bool keyJump;
    bool keyShoot;

    bool isGrounded;
    bool isShooting;
    bool isTakingDamage;
    bool isInvincible;
    public bool isFacingRight;

    bool hitSideRight;

    public AudioClip shootSound;

    public bool isMoving = false;

    float shootTime;
    bool keyShootRelease;

    public int currentHealth;
    public int maxHealth = 28;
    //public TextAlignment lifeText;
    public int lives;

    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float jumpSpeed = 3.7f;

    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;


    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] Transform bulletShootPos;
    [SerializeField] GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //Character Faces right be default
        isFacingRight = true;

        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        isGrounded = false;
        Color raycastColor;
        RaycastHit2D raycastHit;
        float raycastDistance = 0.5f;
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        //Ground Check
        Vector3 box_origin = box2d.bounds.center;
        box_origin.y = box2d.bounds.min.y + (box2d.bounds.extents.y / 4f);
        Vector3 box_size = box2d.bounds.size;
        box_size.y = box2d.bounds.size.y / 4f;
        raycastHit = Physics2D.BoxCast(box_origin, box_size, 0f, Vector2.down, raycastDistance, layerMask);

        // Player Box touching the Ground
        if (raycastHit.collider != null)
        {
            isGrounded = true;
        }
        // Debug Lines
        raycastColor = (isGrounded) ? Color.green : Color.red;
        Debug.DrawRay(box_origin + new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
        Debug.DrawRay(box_origin - new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
        Debug.DrawRay(box_origin - new Vector3(box2d.bounds.extents.x, box2d.bounds.extents.y / 4f + raycastDistance), Vector2.right * (box2d.bounds.extents.x * 2), raycastColor);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (isTakingDamage)
        {
            animator.Play("Player_Hit");
            return;
        }

        PlayerDirectionInput();
        PlayerJumpInput();
        PlayerShootInput();
        PlayerController();
        PlayerDashInput();

        if (isDashing)
        {
            return;
        }
        
        /*if (lives == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }*/

    }

    

    void PlayerController()
    {
        if (keyHorizontal < 0)
        {
            if (isFacingRight)
            {
                Flip();
            }
            if (isGrounded)
            {
                if (isShooting)
                {
                    //Player_RunShoot Animation
                }
                else
                {
                    //Player Run Animation
                    animator.SetBool("isMove", true);
                }
            }
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
        }
        else if (keyHorizontal > 0)
        {
            if (!isFacingRight)
            {
                Flip();
            }
            if (isGrounded)
            {
                if (isShooting)
                {
                    //Player_RunShoot Animation
                }
                else
                {
                    //Player Run Animation
                    animator.SetBool("isMove", true);

                }
            }
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        }
        else
        {
            if (isGrounded)
            {
                if (isShooting)
                {
                    animator.Play("PlayerShoot");
                }
                else
                {
                    animator.Play("PlayerIdle");
                }

            }
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }


        if (keyJump && isGrounded)
        {
            if (isShooting)
            {
                //animator.Play(Player Jump_Shoot)
            }
            else
            {
                //animator.Play(Player.Jump)
            }
            
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        if (!isGrounded)
        {
            //animator.Play(Jump Animation)
        }

        if (!Input.anyKey)
        {
            isMoving = false;
            animator.SetBool("isMove", false);
        }
    }

    void PlayerJumpInput()
    {
        //Jump Key
        keyJump = Input.GetKeyDown(KeyCode.Z);
    }

    void PlayerDirectionInput()
    {
        //Directional Keys
        keyHorizontal = Input.GetAxisRaw("Horizontal");
    }

    void PlayerDashInput()
    {
        if (Input.GetKeyDown(KeyCode.C) && canDash)
        {
            StartCoroutine(Dash());
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

    /*void Flip()
    {
        
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }*/

    private void Flip()
    {
        if (isFacingRight && keyHorizontal < 0f || !isFacingRight && keyHorizontal > 0f)
        {
            {
                Vector3 localScale = transform.localScale;
                isFacingRight = !isFacingRight;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

        }
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletShootPos.position, Quaternion.identity);
        bullet.GetComponent<PlayerBullet>().SetDamageValue(bulletDamage);
        bullet.GetComponent<PlayerBullet>().SetBulletSpeed(bulletSpeed);
        bullet.GetComponent<PlayerBullet>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        bullet.GetComponent<PlayerBullet>().Shoot();
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
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
            if (currentHealth <= 0)
            {
                Defeat();
            }
            else
            {
                StartDamageAnimation();
            }
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
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
        }
    }

    void StopDamageAnimation()
    {
        isTakingDamage = false;
        isInvincible = false;
        animator.Play("Player_Hit", -1, 0f);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0f;
        rb2d.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb2d.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void Defeat()
    {
        Destroy(gameObject);
    }

    
    
}
