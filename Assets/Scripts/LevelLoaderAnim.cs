using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderAnim : MonoBehaviour
{
    public GameObject stanAnimation;

    public float transitionTime = 1f;

    // Update is called once per frame
    public void StartTransition(int levelIndex)
    {
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

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
