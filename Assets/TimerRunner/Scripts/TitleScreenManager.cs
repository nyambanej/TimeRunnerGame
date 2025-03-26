using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject pressAnyKeyText;
    public VideoPlayer videoPlayer;

    private bool hasStarted = false;
    private float blinkTimer = 0f;
    private bool textVisible = true;

    void Start()
    {
        videoPlayer.Play(); // Start the video
    }

    void Update()
    {
        // Blink text
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= 0.5f)
        {
            textVisible = !textVisible;
            pressAnyKeyText.SetActive(textVisible);
            blinkTimer = 0f;
        }

        // Detect any key
        if (!hasStarted && Input.anyKeyDown)
        {
            hasStarted = true;
            StartGame();
        }
    }

    void StartGame()
    {
        Debug.Log("Loading Scene: TimeRunner");
        SceneManager.LoadScene("TimeRunner");
    }

}
