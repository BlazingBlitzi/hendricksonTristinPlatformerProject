using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideController : MonoBehaviour
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
            PreformSlide();
        }
    }
    private void PreformSlide()
    {
        isSliding = true;

        anim.SetBool("isSlide", true);

        regularColl.enabled = false;
        slideColl.enabled = true;

        if (PL.isFacingRight)
        {
            rb2d.AddForce(Vector2.right * slideSpeed);
        }
        else
        {
            rb2d.AddForce(Vector2.left * slideSpeed);
        }

        StartCoroutine("StopSlide");
    }

    IEnumerator StopSlide()
    {
        yield return new WaitForSeconds(0.0f);
        anim.Play("PlayerIdle");
        anim.SetBool("isSlide", false);
        regularColl.enabled = true;
        slideColl.enabled = false;
        isSliding = false;
    }
}
