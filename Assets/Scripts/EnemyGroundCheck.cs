using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            enemy.GetComponent<EnemyBehavior>().TurnAround();
        }
    }
}
