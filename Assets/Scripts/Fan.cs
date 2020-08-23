using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    private GameObject gm;
    private GameManager timeManager;
    private Animator anim;
    private AreaEffector2D areaEffect;
    private ParticleSystem parSys;

    public GameObject windEffect;
    public float particleVelocity;
    public bool affectedByTime;

    private float forceMag;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
        timeManager = gm.GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        areaEffect = GetComponent<AreaEffector2D>();

        forceMag = areaEffect.forceMagnitude;
        parSys = windEffect.GetComponent<ParticleSystem>();

        if (!affectedByTime)
            SetRedColor();

    }

    // Update is called once per frame
    void Update()
    {
        if (affectedByTime)
        {
            anim.SetFloat("speed", timeManager.timeVelocity);
            areaEffect.forceMagnitude = forceMag * timeManager.timeVelocity;
        }
    }

    private void LateUpdate()
    {

        ParticleSystem.Particle[] p = new ParticleSystem.Particle[parSys.particleCount + 1];
        int l = parSys.GetParticles(p);

        if (affectedByTime)
            for (int i = 0; i < l; i++)
                p[i].velocity = new Vector3(1f, particleVelocity * timeManager.timeVelocity, 1f);
            
        else
            for (int i = 0; i < l; i++)
                p[i].velocity = new Vector3(1f, particleVelocity, 1f);

        parSys.SetParticles(p, l);
        
    }

    private void SetRedColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            collision.gameObject.GetComponent<PlayerMovement>().setDeath();
    }
}
