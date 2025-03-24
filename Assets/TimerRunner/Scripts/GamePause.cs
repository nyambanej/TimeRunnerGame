using UnityEngine;

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
        Time.timeScale = 0f; // Pauses game logic
        pauseScreen.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resumes game logic
        pauseScreen.SetActive(false);
        isPaused = false;
    }

}
