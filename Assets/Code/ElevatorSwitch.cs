using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSwitch : MonoBehaviour
{
    public ElevatorController Controller;
    [SerializeField] bool Playerisinfront = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Playerisinfront == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Controller.Switchhit == false)
                {
                    Controller.Switchhit = true;
                }
                else
                {
                    Controller.Switchhit = false;
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Playerisinfront = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Playerisinfront = false;
        }
    }
}
