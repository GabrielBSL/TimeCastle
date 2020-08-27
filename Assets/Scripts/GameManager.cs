using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public float timeVelocity;

    [HideInInspector]
    public int currentLevelIndex;

    private AudioManager audio;

    [HideInInspector]
    public string songName;

    public GameObject pausePanelUI;
    public GameObject inGameUI;
    public GameObject levelLoader;

    private bool isGamePaused = false;

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

        if (SceneManager.GetActiveScene().name != "MainMenu" &&
            SceneManager.GetActiveScene().name != "Credits")
        {

            if (Input.GetKeyDown(KeyCode.Escape) && !isGamePaused && FindObjectOfType<PlayerMovement>().isAlive)
            {
                PauseGame();
            }

            else if (Input.GetKeyDown(KeyCode.Escape) && isGamePaused)
            {
                ResumeGame();
            }

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

        Invoke("WaitToUnpause", 1f);

        levelLoader.GetComponent<LevelLoaderAnim>().StartTransition(currentLevelIndex);
    }

    public void WaitToUnpause()
    {
        audio.Pitch(songName, 1f);
        audio.Unpause(songName, 1f);
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" ||
            SceneManager.GetActiveScene().name == "Intermission_1" ||
            SceneManager.GetActiveScene().name == "Intermission_2" ||
            SceneManager.GetActiveScene().name == "Intermission_3" ||
            SceneManager.GetActiveScene().name == "Level_boss")
        {
            Invoke("WaitToStop", 1f);
        }

        else
            audio.Pitch(songName, 1f);

        levelLoader.GetComponent<LevelLoaderAnim>().StartTransition(currentLevelIndex + 1);
    }

    public void WaitToStop()
    {
        audio.Pitch(songName, 1f);
        audio.Reset(songName);
        audio.Stop(songName);
        audio.reverseBGM = false;
    }

    public void LoadMenu()
    {
        audio.noMenuIntro = true;

        audio.Pitch(songName, 1f);
        audio.Reset(songName);
        audio.Stop(songName);
        Time.timeScale = 1f;

        audio.SetTime("MenuMusic", 8f);

        levelLoader.GetComponent<LevelLoaderAnim>().StartTransition(0);
    }

    public void PauseGame()
    {
        audio.PlaySound("PauseSound");

        pausePanelUI.SetActive(true);
        inGameUI.SetActive(false);
        isGamePaused = true;
        audio.Pause(songName);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        audio.PlaySound("PauseSound");

        pausePanelUI.SetActive(false);
        inGameUI.SetActive(true);

        isGamePaused = false;
        audio.Unpause(songName, timeVelocity);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
