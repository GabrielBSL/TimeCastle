using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderAnim : MonoBehaviour
{
    public GameObject stanAnimation;

    public float transitionTime = 1f;

    private void Start()
    {
        FindObjectOfType<GameManager>().isInTransition = true;

        Invoke("EnablePause", 1f);
    }

    private void EnablePause()
    {
        FindObjectOfType<GameManager>().isInTransition = false;
    }

    // Update is called once per frame
    public void StartTransition(int levelIndex)
    {
        FindObjectOfType<GameManager>().isInTransition = true;

        if (stanAnimation.activeSelf)
        {
            StartCoroutine(LoadLevel(stanAnimation.GetComponent<Animator>(), levelIndex));
        }
        else
        {
            stanAnimation.SetActive(true);
            StartCoroutine(LoadLevel(stanAnimation.GetComponent<Animator>(), levelIndex));
        }
    }

    IEnumerator LoadLevel(Animator anim, int levelIndex)
    {
        anim.SetTrigger("fadeout");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
