using UnityEngine;
using TMPro;

public class TimeSurvivedDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private float timeSurvived = 0f;
    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return;
        

        timeSurvived += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeSurvived / 60f);
        int seconds = Mathf.FloorToInt(timeSurvived % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
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
}
