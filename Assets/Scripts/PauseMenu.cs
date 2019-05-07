using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;


    // Update is called once per frame
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Menu()
    {
        Debug.Log("Quit Game");
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void Controls()
    {
        GameObject pauseMenu = transform.GetChild(0).gameObject;
        pauseMenu.transform.GetChild(0).gameObject.SetActive(false);
        pauseMenu.transform.GetChild(1).gameObject.SetActive(false);
        pauseMenu.transform.GetChild(2).gameObject.SetActive(false);
        pauseMenu.transform.GetChild(3).gameObject.SetActive(true);
        pauseMenu.transform.GetChild(4).gameObject.SetActive(true);
    }

    public void Back()
    {
        GameObject pauseMenu = transform.GetChild(0).gameObject;
        pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
        pauseMenu.transform.GetChild(1).gameObject.SetActive(true);
        pauseMenu.transform.GetChild(2).gameObject.SetActive(true);
        pauseMenu.transform.GetChild(3).gameObject.SetActive(false);
        pauseMenu.transform.GetChild(4).gameObject.SetActive(false);
    }
}
