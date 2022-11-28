using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Transform Player;
    public Transform Elevatorswitch;
    public Transform Downpos;
    public Transform Upperpos;

    public SpriteRenderer elevator;

    public float Speed;
    bool Iselevatordown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartElevator();
        DisplayColor();
    }
    void StartElevator()
    {
        if(Vector2.Distance(Player.position, Elevatorswitch.position)<0.5f && Input.GetKeyDown("f"))
        {
            if(transform.position.y <= Downpos.position.y)
            {
                Iselevatordown = true;
            }
            else if(transform.position.y >= Upperpos.position.y)
            {
                Iselevatordown = false;
            }
        }

        if(Iselevatordown)
        {
            transform.position = Vector2.MoveTowards(transform.position, Upperpos.position, Speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, Downpos.position, Speed * Time.deltaTime);
        }
    }    
    void DisplayColor()
    {
        if(transform.position.y <= Downpos.position.y || transform.position.y >= Upperpos.position.y)
        {
            elevator.color = Color.green;
        }
        else
        {
            elevator.color = Color.red;
        }
    }    
}
