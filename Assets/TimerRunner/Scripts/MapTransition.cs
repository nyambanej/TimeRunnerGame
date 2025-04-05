using UnityEngine;
using UnityEngine.Video;

public class MapTransition : MonoBehaviour
{
    [Header("Timing")]
    public float transitionTime1 = 30f;
    public float transitionTime2 = 70f;
    public float glitchDuration = 3f;

    [Header("References")]
    public GameObject level1Parallax;
    public GameObject level2Parallax;
    public GameObject level3Parallax;
    public GameObject glitchVideoUI;
    public VideoPlayer glitchVideo;

    private float timer = 0f;
    private int currentStage = 0;

    void Start()
    {
        if (glitchVideoUI) glitchVideoUI.SetActive(false);
        if (level1Parallax) level1Parallax.SetActive(true);
        if (level2Parallax) level2Parallax.SetActive(false);
        if (level3Parallax) level3Parallax.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (currentStage == 0 && timer >= transitionTime1)
        {
            currentStage = 1;
            StartCoroutine(DoGlitchTransition(level1Parallax, level2Parallax));
        }
        else if (currentStage == 1 && timer >= transitionTime2)
        {
            currentStage = 2;
            StartCoroutine(DoGlitchTransition(level2Parallax, level3Parallax));
        }
    }

    private System.Collections.IEnumerator DoGlitchTransition(GameObject oldParallax, GameObject newParallax)
    {
        if (glitchVideoUI) glitchVideoUI.SetActive(true);

        if (glitchVideo)
        {
            glitchVideo.time = 0f;
            glitchVideo.Play();
        }

        yield return new WaitForSeconds(glitchDuration);

        if (glitchVideoUI) glitchVideoUI.SetActive(false);
        if (oldParallax) oldParallax.SetActive(false);
        if (newParallax) newParallax.SetActive(true);
    }

    public bool WillTransitionSoon(float bufferTime = 3f)
    {
        if (currentStage == 0)
            return transitionTime1 - timer <= bufferTime;
        else if (currentStage == 1)
            return transitionTime2 - timer <= bufferTime;
        return false;
    }

    public bool IsTransitionPlaying()
    {
        return glitchVideoUI != null && glitchVideoUI.activeSelf;
    }

    public float GetElapsedTime()
    {
        return timer;
    }

    public float TransitionTime1 => transitionTime1;
    public float TransitionTime2 => transitionTime2;
}
