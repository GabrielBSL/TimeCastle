using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuIntro : MonoBehaviour
{
    public float velocity;
    public float timeLimit;

    public GameObject titlePanel;
    public GameObject optionsPanel;
    public GameObject introFadeIn;
    public GameObject normalFadeIn;

    [HideInInspector]
    public bool noIntro;
    public bool credits;

    // Start is called before the first frame update
    void Start()
    {
        noIntro = FindObjectOfType<AudioManager>().noMenuIntro;

        if (!credits)
        {
            if (!noIntro)
                introFadeIn.SetActive(true);

            else
                normalFadeIn.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!credits)
        {
            if (!noIntro)
            {
                if (timeLimit > 0)
                {
                    timeLimit -= Time.deltaTime;

                    transform.position = new Vector3(transform.position.x + (Time.deltaTime * velocity), transform.position.y, transform.position.z);
                }
                else
                {
                    if (!titlePanel.activeSelf)
                    {
                        titlePanel.SetActive(true);
                        titlePanel.GetComponent<CanvasGroup>().alpha = 0f;
                    }

                    else if (titlePanel.activeSelf && titlePanel.GetComponent<CanvasGroup>().alpha < 1f)
                    {
                        titlePanel.GetComponent<CanvasGroup>().alpha += Time.deltaTime / 2f;
                    }

                    else if (!optionsPanel.activeSelf)
                    {
                        optionsPanel.SetActive(true);
                        optionsPanel.GetComponent<CanvasGroup>().alpha = 0f;
                    }

                    else if (optionsPanel.activeSelf && optionsPanel.GetComponent<CanvasGroup>().alpha < 1f)
                    {
                        optionsPanel.GetComponent<CanvasGroup>().alpha += Time.deltaTime / 2f;
                    }

                    else
                    {
                        gameObject.GetComponent<MainMenuIntro>().enabled = false;
                    }
                }
            }
            else
            {
                transform.position = new Vector3(transform.position.x + (timeLimit * velocity), transform.position.y, transform.position.z);

                titlePanel.SetActive(true);
                titlePanel.GetComponent<CanvasGroup>().alpha = 1f;
                
                
                optionsPanel.SetActive(true);
                optionsPanel.GetComponent<CanvasGroup>().alpha = 1f;

                gameObject.GetComponent<MainMenuIntro>().enabled = false;
            }
        }
        else
            transform.position = new Vector3(transform.position.x + (Time.deltaTime * velocity), transform.position.y, transform.position.z);
        
    }
}
