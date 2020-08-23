using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoxHit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "DestroyBox")
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DestroyBox")
            Destroy(gameObject);
    }
}
