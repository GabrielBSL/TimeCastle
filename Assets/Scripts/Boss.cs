using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private Animator anim;

    private float difficultyFactor;
    public int health = 6;

    private bool right;
    private bool isMoving;
    private bool gotHit;
    private bool playerAlive;
    private bool seekSawSpawned;

    [HideInInspector]
    public bool isActived;

    public GameObject saw;

    public float speed;
    public float secondsToStop;
    private float timeToStop;

    public float secondsToMove;
    private float timeToMove;

    public float secondsToSpawnSaw;
    private float timeToSpawnSaw;

    public float seekPlayerSaw;
    private int sawSpawned;

    public GameObject deathEffect;
    public GameObject explosionEffect;

    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();

        difficultyFactor = 1f;
        right = false;
        isActived = false;
        gotHit = false;
        seekSawSpawned = false;
        playerAlive = true;

        timeToMove = secondsToMove;
        timeToStop = secondsToStop;
        timeToSpawnSaw = 0.5f;

        seekPlayerSaw = 5;
        sawSpawned = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActived && !gotHit && playerAlive)
        {
            if (player == null)
                playerAlive = false;

            CheckPlayerPosition();

            if (isMoving)
                Move();

            else
                Attack();
        }
    }

    private void CheckPlayerPosition()
    {
        if (player.transform.position.x - transform.position.x < 0 && right)
        {
            right = false;
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        else if (player.transform.position.x - transform.position.x > 0 && !right)
        {
            right = true;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    private void Attack()
    {
        timeToSpawnSaw -= Time.deltaTime * difficultyFactor;
        timeToMove -= Time.deltaTime;

        if(timeToSpawnSaw <= 0)
        {
            if(!seekSawSpawned)
                sawSpawned++;

            GameObject newSaw = Instantiate(saw, transform.position, Quaternion.identity);
            newSaw.AddComponent<DestroyBoxHit>();

            newSaw.GetComponent<Saw>().seekPlayer = true;

            if (sawSpawned >= seekPlayerSaw)
            {
                sawSpawned = 0;
                seekSawSpawned = true;

                newSaw.GetComponent<Saw>().speed = 3f;
                newSaw.GetComponent<Saw>().targetPlayer = false;
                newSaw.GetComponent<Saw>().SetPurpleColor();
                newSaw.GetComponent<Saw>().SecondsToBeAffectedByTime(3);
            }

            else
            {
                newSaw.GetComponent<Saw>().speed = 5f * difficultyFactor;
                newSaw.GetComponent<Saw>().targetPlayer = true;
                newSaw.GetComponent<Saw>().SetRedColor();
            }

            timeToSpawnSaw = secondsToSpawnSaw;
        }

        if(timeToMove <= 0)
        {
            timeToMove = secondsToMove;
            isMoving = true;
        }
    }

    private void Move()
    {
        Vector2 direction = new Vector2(player.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime * difficultyFactor);
        anim.SetBool("walk", true);

        timeToStop -= Time.deltaTime;

        if(timeToStop <= 0)
        {
            isMoving = false;
            timeToStop = secondsToStop;
            anim.SetBool("walk", false);
        }
    }

    public void GetHit()
    {
        gotHit = true;
        anim.SetBool("hit", true);
        seekSawSpawned = false;

        FindObjectOfType<AudioManager>().PlaySound("BossHit");
        
        health--;

        if(health <= 2)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
            difficultyFactor = 1.7f;
        }
        else if (health <= 4)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 1f);
            difficultyFactor = 1.2f;
        }

        secondsToMove *= difficultyFactor;
        secondsToStop *= difficultyFactor;
        seekPlayerSaw *= difficultyFactor;

        Invoke("ExitStun", 2f);
    }

    private void ExitStun()
    {
        if (health <= 0)
            Defeated();
        //GameObject.Find("GameManager").GetComponent<GameManager>().NextLevel();

        else
        {
            gotHit = false;
            anim.SetBool("hit", false);
        }
    }

    private void Defeated()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        deathEffect.SetActive(true);
        Invoke("Explosion", 4f);
    }

    private void Explosion()
    {
        FindObjectOfType<AudioManager>().Stop("BossMusic");
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        explosionEffect.SetActive(true);
        Invoke("Win", 5f);
    }

    private void Win()
    {
        FindObjectOfType<GameManager>().NextLevel();
    }

    public void ActivateBoss()
    {
        isActived = true;
        isMoving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().setDeath();
        }
    }
}
