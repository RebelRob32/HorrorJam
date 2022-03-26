using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject buttonPanel;
    public GameObject howtoPlayPanel;


    public void Update()
    {
        
    }


    public void BeginGame()
    {
        SceneManager.LoadScene("Alice'sHouse");
    }

    public void CreditsScreen()
    {
        bool isActive = creditsPanel.activeSelf;
        creditsPanel.SetActive(true);
        buttonPanel.SetActive(false);
    }

    public void ButtonScreen()
    {
        bool isActive = buttonPanel.activeSelf;
        buttonPanel.SetActive(true);
        creditsPanel.SetActive(false);
        howtoPlayPanel.SetActive(false);
    }

    public void HowToPlay()
    {
        bool isActive = howtoPlayPanel.activeSelf;
        howtoPlayPanel.SetActive(true);
        buttonPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
