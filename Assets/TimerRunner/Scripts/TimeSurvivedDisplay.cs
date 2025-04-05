using UnityEngine;
using TMPro;

public class TimeSurvivedDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public float scoreMultiplier = 100f;

    private float timeSurvived = 0f;
    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return;

        timeSurvived += Time.deltaTime;

        // Score increases every full second
        int wholeSeconds = Mathf.FloorToInt(timeSurvived);
        int score = wholeSeconds * Mathf.FloorToInt(scoreMultiplier);
        timeText.text = $"{score}";
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("✅ StopTimer was called");
    }

    public void ResetTimer()
    {
        timeSurvived = 0f;
        isRunning = true;
    }

    public int GetScore()
    {
        return Mathf.FloorToInt(timeSurvived) * Mathf.FloorToInt(scoreMultiplier);
    }

    public void SetMultiplier(float newMultiplier)
    {
        scoreMultiplier = newMultiplier;
    }
}