using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().StopScript();
            GameObject.Find("GameManager").GetComponent<GameManager>().NextLevel();
        }
    }
}
