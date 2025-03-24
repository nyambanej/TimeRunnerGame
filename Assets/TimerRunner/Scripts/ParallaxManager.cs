using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    private ParallaxBackground[] parallaxLayers;
    private WindLine[] windLines;
    private bool isRewinding = false;

    private WindManager windManager;

    void Start()
    {
        parallaxLayers = Object.FindObjectsByType<ParallaxBackground>(FindObjectsSortMode.None);
        windManager = FindObjectOfType<WindManager>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            StartRewind();
        if (Input.GetKeyUp(KeyCode.F))
            StopRewind();
    }

    public void StartRewind()
    {
        if (isRewinding) return;
        isRewinding = true;

        foreach (ParallaxBackground layer in parallaxLayers)
            layer.StartRewind();

        WindLine[] windLines = Object.FindObjectsByType<WindLine>(FindObjectsSortMode.None);
        foreach (WindLine wind in windLines)
            wind.StartRewind();

        windManager?.StartRewind();
    }

    public void StopRewind()
    {
        if (!isRewinding) return;
        isRewinding = false;

        foreach (ParallaxBackground layer in parallaxLayers)
            layer.StopRewind();

        WindLine[] windLines = Object.FindObjectsByType<WindLine>(FindObjectsSortMode.None);
        foreach (WindLine wind in windLines)
            wind.StopRewind();

        windManager?.StopRewind();
    }
}
