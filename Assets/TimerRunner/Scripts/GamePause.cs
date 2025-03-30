using UnityEngine;
using UnityEngine.SceneManagement;  

public class GamePause : MonoBehaviour
{
    public GameObject pauseScreen;  
    private bool isPaused = false;  

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;       
        pauseScreen.SetActive(true); // Show the pause screen
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;         // Resume game time
        pauseScreen.SetActive(false); 
        isPaused = false;
    }

    // This function is for the Menu button (goes back to the title screen)
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("TitleScene");  // 
    }

    // This function is for the Quit button (exits the game)
    public void QuitGame()
    {
        Application.Quit();  

    }
}

