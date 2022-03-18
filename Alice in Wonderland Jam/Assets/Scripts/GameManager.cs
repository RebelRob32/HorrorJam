using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public GameObject alice;
    public GameObject rabbit;
    public GameObject gameOverPanel;


    public Slider fearSlider;


    public void Awake()
    {
        alice = GameObject.FindGameObjectWithTag("Alice");
        rabbit = GameObject.FindGameObjectWithTag("Rabbit");
    }

    public void Update()
    {
        fearSlider.value = alice.GetComponent<AliceController>().fearLevel;
    }

 public void MoveToWonderland()
    {
        SceneManager.LoadScene("Wonderland");
    }

    public void LoseGame()
    {
        if(alice.GetComponent<AliceController>().fearLevel == 100f)
        {
            bool isActive = gameOverPanel.activeSelf;
            gameOverPanel.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
