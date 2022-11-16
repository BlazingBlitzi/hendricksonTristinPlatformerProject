using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    public float walkSpeed, range, timeBetweenShots, shootSpeed;
    private float distToPlayer;

    [HideInInspector]
    public bool mustPatrol;
    private bool mustTurn;
    private bool canShoot;

    public Rigidbody2D rb2d;
    public Transform groundCheckPos;
    public LayerMask groundLayer;
    public Collider2D bodyCollider;
    public Transform player, shootPos;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        mustPatrol = true;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }

        distToPlayer = Vector2.Distance(transform.position, player.position);

        if(distToPlayer <= range)
        {
            if(player.position.x > transform.position.x && transform.localScale.x < 0
                || player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }

            mustPatrol = false;
            rb2d.velocity = Vector2.zero;

            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
            
        }
    }

    private void FixedUpdate()
    {
        if(mustPatrol)
        {
            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
        }
    }

    void Patrol()
    {
        if (mustTurn || bodyCollider.IsTouchingLayers(groundLayer))
        {
            Flip();
        }
        rb2d.velocity = new Vector2(walkSpeed * Vector2.right.x * Time.fixedDeltaTime, rb2d.velocity.y);
    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustPatrol = true;
    }

    IEnumerator Shoot()
    {
        canShoot = false;

        yield return new WaitForSeconds(timeBetweenShots);
        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);

        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * walkSpeed * Time.fixedDeltaTime, 0f);
        canShoot = true;
    }
}
