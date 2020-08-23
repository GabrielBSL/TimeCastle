using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    public float stageTime;
    public Text timeText;

    private GameObject gm;
    private GameManager timeManager;
    private bool reachedZero;
    private float initialTime;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
        timeManager = gm.GetComponent<GameManager>();
        reachedZero = false;
        stageTime += 0.5f;
        initialTime = stageTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (stageTime >= initialTime && timeManager.timeVelocity <= 0)
        {

        }
        else
            if (!reachedZero)
            {
                stageTime -= Time.deltaTime * timeManager.timeVelocity;
                timeText.text = stageTime.ToString("F0");

                if (stageTime < 0)
                {
                    setPlayerDeath();
                    reachedZero = true;
                }

                if (stageTime <= 5.5)
                    timeText.color = new Color(1f, 0f, 0f, 1f);

                else if (stageTime <= 10.5)
                    timeText.color = new Color(1f, 1f, 0f, 1f);

                else
                    timeText.color = new Color(1f, 1f, 1f, 1f);
            }
    }

    void setPlayerDeath()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().setDeath();
    }
}
