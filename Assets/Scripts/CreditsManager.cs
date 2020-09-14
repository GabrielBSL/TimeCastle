using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public GameObject congratulationsCanvas;
    public GameObject creditsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        congratulationsCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }

    public void ActiveCredits()
    {
        congratulationsCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void DeactiveCredits()
    {
        congratulationsCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }
}
