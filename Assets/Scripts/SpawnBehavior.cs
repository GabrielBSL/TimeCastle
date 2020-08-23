using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehavior : MonoBehaviour
{
    public GameObject spawnBox;
    public float spawnTime;

    public float speed;
    public float firstSpawnTime;
    public bool isHorizontal;
    public bool isInverted;
    public bool affectedByTime;

    private float timeToSpawn;
    private GameObject gm;
    private GameManager timeManager;
    private Transform spawnPoint;

    private float timeSpeed;
    private bool negativeSpeed;

    private void Start()
    {
        timeToSpawn = spawnTime - firstSpawnTime;
        gm = GameObject.Find("GameManager");
        timeManager = gm.GetComponent<GameManager>();
        negativeSpeed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (affectedByTime)
        {
            if (timeSpeed < 0 && !negativeSpeed)
            {
                timeToSpawn = spawnTime - (spawnTime - timeToSpawn);
                negativeSpeed = true;
            }

            if (timeSpeed >= 0 && negativeSpeed)
                negativeSpeed = false;

            timeSpeed = timeManager.timeVelocity;
            timeToSpawn += Time.deltaTime * timeSpeed;
        }
        else
            timeToSpawn += Time.deltaTime;

        if (timeToSpawn >= spawnTime || timeToSpawn <= 0)
        {
            if(timeSpeed >= 0 && !isInverted || timeSpeed < 0 & isInverted)
                spawnPoint = gameObject.transform.Find("SpawnPoint1").gameObject.transform;
            
            else 
                spawnPoint = gameObject.transform.Find("SpawnPoint2").gameObject.transform;

            GameObject box = Instantiate(spawnBox, spawnPoint.position, Quaternion.identity);

            box.GetComponent<BoxSpawner>().speed = speed;
            box.GetComponent<BoxSpawner>().isHorizontal = isHorizontal;
            box.GetComponent<BoxSpawner>().isInverted = isInverted;
            box.GetComponent<BoxSpawner>().affectedByTime = affectedByTime;
            box.GetComponent<BoxSpawner>().isStationary = false;

            if (!affectedByTime)
                box.GetComponent<BoxSpawner>().SetRedColor();

            if (timeToSpawn >= spawnTime && !negativeSpeed)
                timeToSpawn = 0f;

            else
                timeToSpawn = spawnTime;
            
        }
    }
}
