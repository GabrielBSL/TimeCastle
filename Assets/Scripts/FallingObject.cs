using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float gravityFactor;
    public float timeToActivate;
    public bool affectedByTime;

    private float speed;
    private float gravityVelocity;
    private bool activated;
    private bool withPlayer;

    private GameObject gm;
    private GameManager timeManager;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
        timeManager = gm.GetComponent<GameManager>();
        player = GameObject.Find("Player");

        speed = 0f;
        activated = false;

        if (!affectedByTime)
            SetRedColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (speed <= 0 && timeManager.timeVelocity < 0)
            {
                if (!affectedByTime)
                {
                    speed += Time.deltaTime;
                    gravityVelocity = (speed / gravityFactor);
                }
            }
            else
            {
                if (affectedByTime)
                {
                    speed += Time.deltaTime * timeManager.timeVelocity;
                    gravityVelocity = (speed / gravityFactor) * timeManager.timeVelocity;
                }

                else
                {
                    speed += Time.deltaTime;
                    gravityVelocity = (speed / gravityFactor);
                }
            }

            if (speed > 0)
            {
                if (withPlayer && 
                    player.GetComponent<PlayerMovement>().isAlive &&
                    gravityVelocity <= player.GetComponent<PlayerMovement>().jumpForce * Time.deltaTime * 2f)
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - gravityVelocity, player.transform.position.z);

                else if (gravityVelocity > player.GetComponent<PlayerMovement>().jumpForce * Time.deltaTime * 2f && withPlayer)
                {
                    player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, player.GetComponent<PlayerMovement>().jumpForce * -1.9f);
                    withPlayer = false;
                }

                transform.position = new Vector3(transform.position.x, transform.position.y - gravityVelocity, transform.position.z);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player" && !activated)
        {
            Invoke("SetActivationTrue", timeToActivate);
            withPlayer = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            withPlayer = true;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            withPlayer = false;

    }

    public void SetActivationTrue()
    {
        activated = true;
    }

    private void SetRedColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
    }
}
