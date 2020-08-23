using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintText : MonoBehaviour
{
    private float time;
    private float alpha;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time <= 1f)
        {
            if (alpha <= 0.5f)
                alpha += Time.deltaTime / 2;
            else
                alpha = 0.5f;

            GetComponent<CanvasGroup>().alpha = alpha;
        }

        else if (time > 2f && time <= 3)
        {
            if (alpha >= 0)
                alpha -= Time.deltaTime;

            else
                alpha = 0f;
            GetComponent<CanvasGroup>().alpha = alpha;
        }

        else if (time > 3f)
            Destroy(gameObject);
    }
}
