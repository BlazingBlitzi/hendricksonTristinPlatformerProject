using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentBlaster : MonoBehaviour
{
    public float Range;

    public Transform Target;

    bool Detected = false;

    Vector2 Direction;

    public GameObject AlarmLight;

    public GameObject Gun;

    public GameObject Bullet;

    public float FireRate;

    float nextTimetoFire = 0;

    public Transform ShootPoint;

    public float Force;

    public AudioClip ShootSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPos = Target.position;

        Direction = targetPos - (Vector2)transform.position;

        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range);

        if(rayInfo)
        {
            if(rayInfo.collider.gameObject.tag == "Player")
            {
                if(Detected == false)
                {
                    Detected = true;
                    AlarmLight.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else
            {
                if(Detected == true)
                {
                    Detected = true;
                    AlarmLight.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
        }

        if(Detected)
        {
            Gun.transform.up = Direction;
            if(Time.time > nextTimetoFire)
            {
                nextTimetoFire = Time.time + 1 / FireRate;
                AudioSource.PlayClipAtPoint(ShootSound, Camera.main.transform.position);
                shoot();
            }
        }
    }

    void shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction * Force);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);

    }
}
