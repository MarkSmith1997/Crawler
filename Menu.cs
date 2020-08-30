using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject mainMenuScreen;
    public GameObject difficultyScreen;
    public GameObject highScoresScreen;
    public GameObject optionsScreen;

    public void Crawl()
    {
        mainMenuScreen.SetActive(false);
        difficultyScreen.SetActive(true);
    }
    public void CrawlBack()
    {
        mainMenuScreen.SetActive(true);
        difficultyScreen.SetActive(false);
    }


    public void Relaxed()
    {
        PlayerPrefs.SetFloat("difficulty", 0.5f);
        SceneManager.LoadScene(1);
    }

    public void Difficult()
    {
        PlayerPrefs.SetFloat("difficulty", 1f);
        SceneManager.LoadScene(1);
    }

    public void Difficulter()
    {
        PlayerPrefs.SetFloat("difficulty", 2f);
        SceneManager.LoadScene(1);
    }

    public void Highscores()
    {
        mainMenuScreen.SetActive(false);
        highScoresScreen.SetActive(true);
    }
    public void HighScoreBack()
    {
        mainMenuScreen.SetActive(true);
        highScoresScreen.SetActive(false);
    }


    public void Options()
    {
        mainMenuScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }
    public void OptionsBack()
    {
        mainMenuScreen.SetActive(true);
        optionsScreen.SetActive(false);
    }


    public void Quit()
    {
        Application.Quit();
    }
}
