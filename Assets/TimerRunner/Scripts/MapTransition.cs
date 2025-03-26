using UnityEngine;
using UnityEngine.Video;

public class MapTransition : MonoBehaviour
{
    [Header("Timing")]
    public float transitionTime = 30f;  // First glitch
    public float transitionTime2 = 70f; // Second glitch
    public float glitchDuration = 3f;    // How long the glitch video plays (optional if you want to time it manually)

    [Header("References")]
    public GameObject oldParallax;       // Drag your old map object here
    public GameObject newParallax;       // Drag your new map object here
    public GameObject glitchVideoUI;     // The RawImage that displays the glitch video
    public VideoPlayer glitchVideo;      // The VideoPlayer component

    private float timer = 0f;
    private bool triggered = false;

    void Start()
    {
        // Ensure glitch UI is hidden at start
        if (glitchVideoUI) glitchVideoUI.SetActive(false);

        // Make sure new parallax is off initially
        if (newParallax) newParallax.SetActive(false);
    }

    void Update()
    {
        // If we've already triggered the transition, do nothing
        if (triggered) return;

        timer += Time.deltaTime;

        // Once we reach the transition time, start the glitch effect
        if (timer >= transitionTime)
        {
            triggered = true;
            StartCoroutine(DoGlitchTransition());
        }
    }

    private System.Collections.IEnumerator DoGlitchTransition()
    {
        // Show the glitch UI
        if (glitchVideoUI) glitchVideoUI.SetActive(true);

        // Start playing the video
        if (glitchVideo)
        {
            glitchVideo.time = 0f;
            glitchVideo.Play();
        }

        // Option A: Wait a fixed duration
        yield return new WaitForSeconds(glitchDuration);

        // Option B (instead of fixed wait): Wait for video to finish
        // while (glitchVideo && glitchVideo.isPlaying)
        // {
        //     yield return null;
        // }

        // Hide glitch UI
        if (glitchVideoUI) glitchVideoUI.SetActive(false);

        // Disable old parallax
        if (oldParallax) oldParallax.SetActive(false);

        // Enable new parallax
        if (newParallax) newParallax.SetActive(true);
    }

    public bool WillTransitionSoon(float bufferTime = 3f)
    {
        return !triggered && (transitionTime - timer <= bufferTime);
    }

    public bool IsTransitionPlaying()
    {
        return glitchVideoUI != null && glitchVideoUI.activeSelf;
    }

    public float GetElapsedTime()
    {
        return timer;
    }


}
