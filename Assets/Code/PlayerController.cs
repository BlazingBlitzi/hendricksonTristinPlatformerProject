using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //SerializeField is an automatic process of transofmring data structures or object states
    //into a format that Unity can store and rebuild later.
    [SerializeField] private LayerMask platformslayerMask;
    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = transform.GetComponent<Rigidbody2D>();
        bc2d = transform.GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(IsGrounded() && Input.GetKeyDown(KeyCode.Z))
        {
            float jumpVelocity = 10f;
            rb2d.velocity = Vector2.up * jumpVelocity;
        }

        MovementHandler();
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.down, .1f, platformslayerMask);
        Debug.Log(raycastHit2D.collider);
        return (raycastHit2D.collider != null);
    }

    private void MovementHandler()
    {
        float moveSpeed = 5f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb2d.velocity = new Vector2(+moveSpeed, rb2d.velocity.y);
            }
            else
            {
                //No Keys pressed
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }
    }
}
