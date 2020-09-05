using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private Animator anim;
    private GameObject gm;
    private GameManager timeManager;

    public GameObject clockParticleEffect;

    private bool antiClip = true;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
        timeManager = gm.GetComponent<GameManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", timeManager.timeVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && antiClip)
        {
            antiClip = false;
            FindObjectOfType<GameManager>().AddScore();
            clockParticleEffect.SetActive(true);
            GetComponent<SpriteRenderer>().enabled = false;

            FindObjectOfType<AudioManager>().PlaySound("ClockHit");

            Invoke("DestroyClock", 0.2f);
        }
    }

    private void DestroyClock()
    {
        Destroy(gameObject);
    }
}
