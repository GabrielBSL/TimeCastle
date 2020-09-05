using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float speed;
    public bool affectedByTime;
    public Transform[] pathPoints;
    public bool seekPlayer;
    public bool targetPlayer;
    
    private int currentPathPoint;
    private bool isInverted;
    private bool isStationary;
    private bool hitPlayer;
    private bool seekBoss;

    private GameObject gm;
    private GameObject player;
    private GameObject boss;
    private GameManager timeManager;
    private Animator anim;

    [HideInInspector]
    public Vector3 initialPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        gm = GameObject.Find("GameManager");
        timeManager = gm.GetComponent<GameManager>();
        anim = GetComponent<Animator>();

        if (seekPlayer)
        {
            player = GameObject.Find("Player");
            initialPosition = transform.position;
            hitPlayer = false;

            if (targetPlayer)
                targetPosition = (player.transform.position - transform.position).normalized * speed;

        }
        else
        {
            currentPathPoint = 0;
            isInverted = false;

            if (pathPoints.Length == 0)
                isStationary = true;

            else
                isStationary = false;

            if (!affectedByTime)
                SetRedColor();
        }

        seekBoss = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (seekPlayer)
            SeekPlayerPath();

        else
            FollowPathPoints();  
    }

    private void SeekPlayerPath()
    {
        if (affectedByTime)
            anim.SetFloat("speed", timeManager.timeVelocity);

        if (!isStationary && !hitPlayer)
        {
            if (timeManager.timeVelocity >= 0)
            {
                if (affectedByTime)
                {
                    if (targetPlayer)
                        transform.position += targetPosition * Time.deltaTime * timeManager.timeVelocity;

                    else
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime * timeManager.timeVelocity);
                }

                else
                {
                       
                    if (targetPlayer)
                        transform.position += targetPosition * Time.deltaTime;

                    else
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                }
            }
            else
            {
                if (affectedByTime)
                {
                    if (seekBoss)
                        transform.position = Vector2.MoveTowards(transform.position, boss.transform.position, speed * Time.deltaTime * (timeManager.timeVelocity * -1f));
                    
                    else
                        transform.position = Vector2.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime * (timeManager.timeVelocity * -1f));
                }
                else
                {
                    if(targetPlayer)
                        transform.position += targetPosition * Time.deltaTime;

                    else
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                }
            }
        }
    }

    private void FollowPathPoints()
    {
        if (affectedByTime)
            anim.SetFloat("speed", timeManager.timeVelocity);

        if (!isStationary)
        {
            if (timeManager.timeVelocity >= 0 || !affectedByTime)
            {
                if (isInverted)
                {
                    isInverted = false;
                    currentPathPoint++;

                    if (currentPathPoint >= pathPoints.Length)
                        currentPathPoint = 0;
                }

                if ((Vector2)transform.position == (Vector2)pathPoints[currentPathPoint].position)
                {
                    currentPathPoint++;

                    if (currentPathPoint >= pathPoints.Length)
                        currentPathPoint = 0;
                }

                else
                    if (affectedByTime)
                    transform.position = Vector2.MoveTowards(transform.position, pathPoints[currentPathPoint].position, speed * Time.deltaTime * timeManager.timeVelocity);

                else
                    transform.position = Vector2.MoveTowards(transform.position, pathPoints[currentPathPoint].position, speed * Time.deltaTime);
            }
            else
            {
                if (!isInverted)
                {
                    isInverted = true;
                    currentPathPoint--;

                    if (currentPathPoint < 0)
                        currentPathPoint = pathPoints.Length - 1;
                }

                if ((Vector2)transform.position == (Vector2)pathPoints[currentPathPoint].position)
                {
                    currentPathPoint--;

                    if (currentPathPoint < 0)
                        currentPathPoint = pathPoints.Length - 1;
                }

                else
                    transform.position = Vector2.MoveTowards(transform.position, pathPoints[currentPathPoint].position, speed * Time.deltaTime * (timeManager.timeVelocity * -1f));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().setDeath();

            if(!targetPlayer)
                hitPlayer = true;
        }

        if (collision.tag == "Boss")
        {
            if (seekBoss && affectedByTime)
            {
                collision.gameObject.GetComponent<Boss>().GetHit();
                Destroy(gameObject);
            }
        }
    }

    public void SetRedColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
        affectedByTime = false;
    }

    public void SetPurpleColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 1f, 1f);
        affectedByTime = false;
    }

    public void SecondsToBeAffectedByTime(float seconds)
    {
        Invoke("BeAffectedByTime", seconds);
    }

    private void BeAffectedByTime()
    {
        affectedByTime = true;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        boss = GameObject.Find("Boss");
        seekBoss = true;
    }
}
