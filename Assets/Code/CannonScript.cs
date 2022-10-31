using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bullet;
    float timeBetween;
    public float startTimeBetween;

    public bool canSeePlayer;
    // Start is called before the first frame update
    void Start()
    {
        timeBetween = startTimeBetween;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (canSeePlayer)
        {
            if (timeBetween <= 0)
            {
                Instantiate(bullet, firepoint.position, firepoint.rotation);
                timeBetween = startTimeBetween;
            }
            else
            {
                timeBetween -= Time.deltaTime;
            }
        }
    }
}
