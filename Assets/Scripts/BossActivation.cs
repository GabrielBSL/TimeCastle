using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivation : MonoBehaviour
{
    public GameObject hintText;

    private AudioManager audio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            transform.parent.gameObject.GetComponent<Boss>().ActivateBoss();
            hintText.SetActive(true);

            Destroy(gameObject);
        }
    }
}
