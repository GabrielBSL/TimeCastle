using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Transform frontUp;
    public Transform frontDown;
    public LayerMask layer;

    [HideInInspector]
    public bool grounded;

    [HideInInspector]
    public bool jumping;

    private Rigidbody2D rig;
    private Animator anim;

    public float slopeRayLenght;

    public GameObject deathEffect;

    private bool wallColliding;
    private bool right;
    private bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        jumping = true;
        grounded = false;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
            Movement();
    }

    private void Movement()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump") && grounded)
            Jump();

        wallColliding = Physics2D.Linecast(frontUp.position, frontDown.position, layer);

        if (!wallColliding ||
            (CrossPlatformInputManager.GetAxis("Horizontal") > 0 && right == false) ||
            (CrossPlatformInputManager.GetAxis("Horizontal") < 0 && right == true))
        {
            Vector3 movement = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), 0f, 0f);
            transform.position += movement * Time.deltaTime * speed;
        }

        if (CrossPlatformInputManager.GetAxis("Horizontal") > 0)
        {
            right = true;
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        else if (CrossPlatformInputManager.GetAxis("Horizontal") < 0)
        {
            right = false;
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        else
            anim.SetBool("walk", false);

    }

    private void FixedUpdate()
    {
        rig.velocity = Vector3.ClampMagnitude(rig.velocity, jumpForce * 2f);

        if (rig.velocity.y < -0.1)
            anim.SetBool("fall", true);
         
        else
            anim.SetBool("fall", false);

    }

    void Jump()
    {
        jumping = true;
        anim.SetBool("jump", true);
        rig.velocity = new Vector2(0f, jumpForce);
    }

    public void setDeath()
    {
        isAlive = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        rig.gravityScale = 0f;
        rig.velocity = Vector3.zero;

        deathEffect.SetActive(true);
        FindObjectOfType<AudioManager>().PlaySound("PlayerHit");

        FindObjectOfType<AudioManager>().Pause(FindObjectOfType<GameManager>().songName);

        FindObjectOfType<Slider>().enabled = false;

        Invoke("RestartLevel", 1.5f);
    }

    public void RestartLevel()
    {
        FindObjectOfType<AudioManager>().Unpause(FindObjectOfType<GameManager>().songName);

        GameObject.Find("GameManager").GetComponent<GameManager>().RestartLevel();
    }

    public void isGrounded()
    {
        anim.SetBool("jump", false);
    }

    public void isBouncing()
    {
        anim.SetBool("jump", true);
    }
}
