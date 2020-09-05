using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEditor;

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

    public Text scoreText;
    public Text timeText;

    private bool isGamePaused = false;

    private float timeSinceStarted;
    private int stagePoints;

    [HideInInspector]
    public bool isInTransition;

    private void Start()
    {
        timeVelocity = 1f;
        Time.timeScale = 1f;

        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            songName = "MenuMusic";
            PlayerPrefs.SetInt("Current_score", 0);
            PlayerPrefs.SetFloat("Current_time", 0f);
        }

        else if (SceneManager.GetActiveScene().name == "Level_1" ||
            SceneManager.GetActiveScene().name == "Level_2" ||
            SceneManager.GetActiveScene().name == "Level_3" ||
            SceneManager.GetActiveScene().name == "Level_4" ||
            SceneManager.GetActiveScene().name == "Intermission_1")
        {
            songName = "FirstSection";
            scoreText.text = PlayerPrefs.GetInt("Current_score").ToString();
            timeSinceStarted = PlayerPrefs.GetFloat("Current_time");
        }

        else if (SceneManager.GetActiveScene().name == "Level_5" ||
            SceneManager.GetActiveScene().name == "Level_6" ||
            SceneManager.GetActiveScene().name == "Level_7" ||
            SceneManager.GetActiveScene().name == "Level_8" ||
            SceneManager.GetActiveScene().name == "Intermission_2")
        {
            songName = "SecondSection";
            scoreText.text = PlayerPrefs.GetInt("Current_score").ToString();
            timeSinceStarted = PlayerPrefs.GetFloat("Current_time");
        }

        else if (SceneManager.GetActiveScene().name == "Level_9" ||
            SceneManager.GetActiveScene().name == "Level_10" ||
            SceneManager.GetActiveScene().name == "Level_11" ||
            SceneManager.GetActiveScene().name == "Level_12" ||
            SceneManager.GetActiveScene().name == "Intermission_3")
        {
            songName = "ThirdSection";
            scoreText.text = PlayerPrefs.GetInt("Current_score").ToString();
            timeSinceStarted = PlayerPrefs.GetFloat("Current_time");
        }

        else if (SceneManager.GetActiveScene().name == "Level_boss")
        {
            songName = "BossMusic";
            scoreText.text = PlayerPrefs.GetInt("Current_score").ToString();
            timeSinceStarted = PlayerPrefs.GetFloat("Current_time");
        }

        else
        {
            songName = "CreditsMusic";
            ShowFinalScore();
        }

        stagePoints = PlayerPrefs.GetInt("Current_score");
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
            timeSinceStarted += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Escape) && !isGamePaused && FindObjectOfType<PlayerMovement>().isAlive && !isInTransition)
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
        {
            if (timeVelocity >= 0)
                audio.SetTime(songName, 0f);

            else
                audio.SetTime(songName + "_reversed", 0f);
        }

        Invoke("WaitToUnpause", 1f);
        PlayerPrefs.SetFloat("Current_time", timeSinceStarted);

        levelLoader.GetComponent<LevelLoaderAnim>().StartTransition(currentLevelIndex);
    }

    public void WaitToUnpause()
    {
        audio.Pitch(songName, 1f);
        audio.Unpause(songName, 1f);
    }

    public void WaitToPitch()
    {
        audio.Pitch(songName, 1f);
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" ||
            SceneManager.GetActiveScene().name == "Intermission_1" ||
            SceneManager.GetActiveScene().name == "Intermission_2" ||
            SceneManager.GetActiveScene().name == "Intermission_3" ||
            SceneManager.GetActiveScene().name == "Level_boss")
        {
            Invoke("WaitToStop", 0.9f);
        }

        else
            Invoke("WaitToPitch", 1f);

        PlayerPrefs.SetFloat("Current_time", timeSinceStarted);
        PlayerPrefs.SetInt("Current_score", stagePoints);

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

    public void AddScore()
    {
        stagePoints++;
        scoreText.text = stagePoints.ToString();
    }

    private void ShowFinalScore()
    {
        scoreText.text = PlayerPrefs.GetInt("Current_score").ToString() + "/175";
        

        timeText.text = Mathf.Floor(PlayerPrefs.GetFloat("Current_time") / 60).ToString("00");
        timeText.text += ":" + Mathf.Floor(PlayerPrefs.GetFloat("Current_time") % 60).ToString("00");
        timeText.text += ":" + Mathf.Floor((PlayerPrefs.GetFloat("Current_time") * 1000) % 1000).ToString("000");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
