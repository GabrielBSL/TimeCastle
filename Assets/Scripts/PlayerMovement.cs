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

    public GameObject deathEffect;

    private bool wallColliding;
    private bool right;

    [HideInInspector]
    public bool isAlive;

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
        if (rig.velocity.y < -0.1)
        {
            anim.SetBool("fall", true);
            anim.SetBool("jump", false);
            rig.velocity = Vector3.ClampMagnitude(rig.velocity, jumpForce * 2f);
        }

        else if (rig.velocity.y > 0.1)
        {
            anim.SetBool("jump", true);
            anim.SetBool("fall", false);
            rig.velocity = Vector3.ClampMagnitude(rig.velocity, jumpForce);
        }

        else
        {
            anim.SetBool("jump", false);
            anim.SetBool("fall", false);
        }
    }

    void Jump()
    {
        jumping = true;
        rig.velocity = new Vector2(0f, jumpForce);
    }

    public void setDeath()
    {
        isAlive = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        rig.velocity = Vector3.zero;
        rig.bodyType = RigidbodyType2D.Static;

        deathEffect.SetActive(true);

        FindObjectOfType<AudioManager>().PlaySound("PlayerHit");
        FindObjectOfType<AudioManager>().Pause(FindObjectOfType<GameManager>().songName);

        if(!FindObjectOfType<GameManager>().isInTransition)
            FindObjectOfType<Slider>().enabled = false;

        Invoke("RestartLevel", 0.5f);
    }

    public void StopScript()
    {
        anim.SetBool("walk", false);
        anim.SetBool("fall", false);
        gameObject.GetComponent<PlayerMovement>().enabled = false;
    }

    public void RestartLevel()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().RestartLevel();
    }
}
