using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{

    public bool isSliding = false;

    public PlayerMovement PL;

    public Rigidbody2D rb2d;

    public Animator anim;

    public BoxCollider2D regularColl;
    public BoxCollider2D slideColl;

    public float slideSpeed = 5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            preformSlide();
        }
    }
    private void preformSlide()
    {
        isSliding = true;

            anim.SetBool("PlayerSlide", true);

        regularColl.enabled = false;
        slideColl.enabled = true;

        if (!PL.isFacingRight)
        {
            rb2d.AddForce(Vector2.right * slideSpeed);
        }
        else
        {
            rb2d.AddForce(Vector2.left * slideSpeed);
        }

        StartCoroutine("stopSlide");
    }

    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(0.0f);
        anim.Play("Idle");
        anim.SetBool("IsSlide", false);
        regularColl.enabled = true;
        slideColl.enabled = false;
        isSliding = false;
    }
}
