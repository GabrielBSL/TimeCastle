using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public bool isHorizontal;
    [HideInInspector]
    public bool isInverted;
    [HideInInspector]
    public bool affectedByTime;

    public bool isStationary;

    private GameObject gm;
    private GameObject player;
    private GameManager timeManager;

    private float velocity;
    private float timeSpeed;
    private float direction;

    private bool withPlayer;

    private void Start()
    {
        gm = GameObject.Find("GameManager");
        timeManager = gm.GetComponent<GameManager>();
        player = GameObject.Find("Player");
        withPlayer = false;

        if (isInverted)
            direction = -1f;
        else
            direction = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStationary)
        {
            if (affectedByTime)
                timeSpeed = timeManager.timeVelocity;

            else
                timeSpeed = 1f;

            velocity = speed * Time.deltaTime * timeSpeed * direction;

            if (isHorizontal)
            {
                transform.position = new Vector3(transform.position.x + velocity, transform.position.y, transform.position.z);

                if (withPlayer && player.GetComponent<PlayerMovement>().isAlive)
                    player.transform.position = new Vector3(player.transform.position.x + velocity, player.transform.position.y, player.transform.position.z);
            }

            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - velocity, transform.position.z);

                if (withPlayer && player.GetComponent<PlayerMovement>().isAlive)
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - velocity, player.transform.position.z);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DestroyBox" && !isStationary)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            withPlayer = true;
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
            withPlayer = true;
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            withPlayer = false;
        
    }

    public void SetRedColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
    }
}
