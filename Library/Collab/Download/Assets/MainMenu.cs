using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject mainMenuUI;

    public void PlayGame()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void PlayTutorial() {
        SceneManager.LoadScene("TutorialScene");
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void Controls()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        GameObject mainMenu = transform.GetChild(2).gameObject;
        mainMenu.transform.GetChild(0).gameObject.SetActive(false);
        mainMenu.transform.GetChild(1).gameObject.SetActive(false);
        mainMenu.transform.GetChild(2).gameObject.SetActive(true);
        mainMenu.transform.GetChild(3).gameObject.SetActive(true);
        mainMenu.transform.GetChild(4).gameObject.SetActive(false);
        mainMenu.transform.GetChild(5).gameObject.SetActive(false);
    }

    public void Back()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        GameObject mainMenu = transform.GetChild(2).gameObject;
        mainMenu.transform.GetChild(0).gameObject.SetActive(true);
        mainMenu.transform.GetChild(1).gameObject.SetActive(true);
        mainMenu.transform.GetChild(2).gameObject.SetActive(false);
        mainMenu.transform.GetChild(3).gameObject.SetActive(false);
        mainMenu.transform.GetChild(4).gameObject.SetActive(true);
        mainMenu.transform.GetChild(5).gameObject.SetActive(true);
    }
}
