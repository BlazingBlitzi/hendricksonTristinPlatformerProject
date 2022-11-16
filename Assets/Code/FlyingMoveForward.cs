using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMoveForward : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.right * speed;
    }
}
