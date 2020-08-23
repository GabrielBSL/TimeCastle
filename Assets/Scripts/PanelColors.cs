using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelColors : MonoBehaviour
{
    private GameObject gm;
    private GameManager timeManager;
    private SpriteRenderer sr;

    public float opacity;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
        timeManager = gm.GetComponent<GameManager>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.timeVelocity >= 0)
        {
            sr.color = new Color(1f, 1f, 0f, ((opacity - (opacity * timeManager.timeVelocity)) / 255f));
        }
        else
        {
            sr.color = new Color(1f, (1f + timeManager.timeVelocity), 0f, opacity / 255f);
        }
    }
}
