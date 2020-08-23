using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public float timeVelocity;

    [HideInInspector]
    public int currentLevelIndex;

    private AudioManager audio;

    private string songName;

    private void Start()
    {
        timeVelocity = 1f;

        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneManager.GetActiveScene().name == "MainMenu")
            songName = "MenuMusic";

        else if (SceneManager.GetActiveScene().name == "Level_1" ||
            SceneManager.GetActiveScene().name == "Level_2" ||
            SceneManager.GetActiveScene().name == "Level_3" ||
            SceneManager.GetActiveScene().name == "Level_4" ||
            SceneManager.GetActiveScene().name == "Intermission_1")
            songName = "FirstSection";

        else if (SceneManager.GetActiveScene().name == "Level_5" ||
            SceneManager.GetActiveScene().name == "Level_6" ||
            SceneManager.GetActiveScene().name == "Level_7" ||
            SceneManager.GetActiveScene().name == "Level_8" ||
            SceneManager.GetActiveScene().name == "Intermission_2")
            songName = "SecondSection";

        else if (SceneManager.GetActiveScene().name == "Level_9" ||
            SceneManager.GetActiveScene().name == "Level_10" ||
            SceneManager.GetActiveScene().name == "Level_11" ||
            SceneManager.GetActiveScene().name == "Level_12" ||
            SceneManager.GetActiveScene().name == "Intermission_3")
            songName = "ThirdSection";

        else if (SceneManager.GetActiveScene().name == "Level_boss")
            songName = "BossMusic";

        else
            songName = "CreditsMusic";
    }

    private void Update()
    {
        if (audio == null)
        {
            audio = FindObjectOfType<AudioManager>();
            audio.Play(songName);
        }
    }

    public void setTimeValue(float newVelocity)
    {
        if(newVelocity > timeVelocity)
        {
            audio.TickTock(false);
        }
        else
        {
            audio.TickTock(true);
        }

        timeVelocity = newVelocity;
        audio.Pitch(songName, newVelocity);
    }

    public void RestartLevel()
    {
        if (SceneManager.GetActiveScene().name == "Level_boss")
            audio.Stop(songName);

        audio.Pitch(songName, 1f);
        SceneManager.LoadScene(currentLevelIndex);
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" ||
            SceneManager.GetActiveScene().name == "Intermission_1" ||
            SceneManager.GetActiveScene().name == "Intermission_2" ||
            SceneManager.GetActiveScene().name == "Intermission_3")
            audio.Stop(songName);

        audio.Pitch(songName, 1f);
        SceneManager.LoadScene(currentLevelIndex + 1);
    }

    public void LoadMenu()
    {
        audio.Stop(songName);
        SceneManager.LoadScene("MainMenu");
    }
}
