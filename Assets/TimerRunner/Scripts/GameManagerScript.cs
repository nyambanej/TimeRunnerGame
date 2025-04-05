using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public TimeSurvivedDisplay timeDisplay;
    public TextMeshProUGUI finalScoreText;

    public void gameOver()
    {
        timeDisplay.StopTimer();
        gameOverUI.SetActive(true);

        if (finalScoreText != null)
        {
            int finalScore = timeDisplay.GetScore();
            finalScoreText.text = $"Final Score: {finalScore}";
        }
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
