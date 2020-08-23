using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform frontUp;
    public Transform frontDown;
    public Transform BackUp;
    public Transform BackDown;

    public LayerMask layer;

    public float speed;
    public bool affectedByTime;

    private GameObject gm;
    private GameManager timeManager;
    private Animator anim;

    private bool gotHit;

    public GameObject deathParticleEffect;

    private bool colliding;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
        timeManager = gm.GetComponent<GameManager>();
        anim = GetComponent<Animator>();

        if (!affectedByTime)
            SetRedColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gotHit)
        {
            if (affectedByTime)
            {
                transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime * timeManager.timeVelocity), transform.position.y, transform.position.z);
                anim.SetFloat("speed", timeManager.timeVelocity);
            }
            else
                transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), transform.position.y, transform.position.z);

            if (timeManager.timeVelocity >= 0 || !affectedByTime)
                colliding = Physics2D.Linecast(frontUp.position, frontDown.position, layer);

            else
                colliding = Physics2D.Linecast(BackUp.position, BackDown.position, layer);

            if (colliding)
                TurnAround();
        }
    }

    public void TurnAround()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
        speed *= -1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            GameObject playerObject = collision.gameObject;

            if (playerObject.transform.position.y >= gameObject.transform.position.y)
            {
                playerObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerObject.GetComponent<PlayerMovement>().jumpForce, ForceMode2D.Impulse);
                playerObject.GetComponent<PlayerMovement>().isBouncing();
                gotHit = true;

                FindObjectOfType<AudioManager>().PlaySound("EnemyHit");

                anim.SetBool("hit", true);
                Invoke("Death", 0.3f);
            }
            else
                playerObject.GetComponent<PlayerMovement>().setDeath();
        }
    }

    private void SetRedColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
    }

    private void Death()
    {
        deathParticleEffect.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0f;

        Destroy(gameObject, 0.3f);
    }
}
