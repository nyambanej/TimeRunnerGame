using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public TimeSurvivedDisplay timeDisplay;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void gameOver()
{
    timeDisplay.StopTimer();     // This stops the timer
    gameOverUI.SetActive(true);  // This shows the Game Over screen
}


    public void mainMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void quit()
    {
        Application.Quit();
    }
}
