using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingBoss : MonoBehaviour
{
    public float speed;
    private GameObject player;
    public bool chase = false;
    public Transform startingPoint;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }
        if (chase == true)
        {
            Chase();
        }
        else

        Flip();
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    /*private void ReturnStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
    }*/

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
            
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
            
    }
}
