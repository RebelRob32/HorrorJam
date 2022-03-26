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
    public GameObject wonderlandPanel;
    public GameObject pausePanel;
    public Transform wonderlandTransition;

    public Text timerText;
    public float timerNum;
    public bool isPaused;

    public Slider fearSlider;


    public void Awake()
    {
        alice = GameObject.FindGameObjectWithTag("Alice");
        rabbit = GameObject.FindGameObjectWithTag("Rabbit");
        wonderlandTransition = GameObject.FindGameObjectWithTag("Trigger").transform;
        Time.timeScale = 1;

        timerNum = 300f;
    }

    public void Update()
    {
        fearSlider.value = alice.GetComponent<AliceController>().fearLevel;
        MovePanelActivate();
        AliceAfraid();
        Timer();
        PausePanel();
    }

    public void MovePanelActivate()
    {
        if(rabbit.transform.position == wonderlandTransition.transform.position)
        {
            bool isActive = wonderlandPanel.activeSelf;
            wonderlandPanel.SetActive(true);
        }
        else
        {
            wonderlandPanel.SetActive(false);
        }
    }

    public void AliceAfraid()
    {
        if(alice.GetComponent<AliceController>().fearLevel == 100f)
        {
            LoseGame();
        }
    }

    public void MoveToWonderland()
    {
        SceneManager.LoadScene("Wonderland");
    }

    public void LoseGame()
    {
        StartCoroutine(GameOverCountdown());
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        if(isPaused == true)
        {
            Time.timeScale = 1;
        }
    }

    public void PausePanel()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;
            bool isActive = pausePanel.activeSelf;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        if(isPaused == true)
        {
            return;
        }
    }

    public void ResumeButton()
    {
        if(isPaused == true)
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            isPaused = false;
        }
    }


    IEnumerator GameOverCountdown()
    {
        bool isActive = gameOverPanel.activeSelf;
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
    }

    public void Timer()
    {
        timerNum -= 1f * Time.deltaTime;
        float mintues = Mathf.FloorToInt(timerNum / 60);
        float seconds = Mathf.FloorToInt(timerNum % 60);

        timerText.text = string.Format("{0:00}:{1:00}", mintues, seconds);

        if(timerNum <= 0f)
        {
            LoseGame();
        }

        
    }


}
