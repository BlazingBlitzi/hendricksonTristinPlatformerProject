using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBoss : MonoBehaviour
{
    public Transform target;

    public float speed = 5f;

    public float rotateSpeed = 200f;

    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb2d.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb2d.angularVelocity = -rotateAmount * rotateSpeed;

        rb2d.velocity = transform.right * speed;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
