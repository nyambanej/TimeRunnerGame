using UnityEngine;

public class PlatformChunk : MonoBehaviour
{
    // Set this value to match GroundSpawner's environmentSpeed
    public float speed = 5f;
    private bool isRewinding = false;
    private float originalSpeed;

    void Start()
    {
        originalSpeed = speed;
    }

    void Update()
    {
        // Move the chunk left
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void StartRewind()
    {
        if (isRewinding) return;
        isRewinding = true;
        speed = -originalSpeed;
    }

    public void StopRewind()
    {
        if (!isRewinding) return;
        isRewinding = false;
        speed = originalSpeed;
    }
}
