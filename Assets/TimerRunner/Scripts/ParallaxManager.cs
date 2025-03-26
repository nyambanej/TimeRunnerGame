using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    private ParallaxBackground[] parallaxLayers;
    private WindManager windManager;
    private bool isRewinding = false;

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

        foreach (WindLine wind in Object.FindObjectsByType<WindLine>(FindObjectsSortMode.None))
            wind.StartRewind();

        foreach (PlatformChunk chunk in Object.FindObjectsByType<PlatformChunk>(FindObjectsSortMode.None))
            chunk.StartRewind();

        windManager?.StartRewind();
    }

    public void StopRewind()
    {
        if (!isRewinding) return;
        isRewinding = false;

        foreach (ParallaxBackground layer in parallaxLayers)
            layer.StopRewind();

        foreach (WindLine wind in Object.FindObjectsByType<WindLine>(FindObjectsSortMode.None))
            wind.StopRewind();

        foreach (PlatformChunk chunk in Object.FindObjectsByType<PlatformChunk>(FindObjectsSortMode.None))
            chunk.StopRewind();

        windManager?.StopRewind();
    }
}
