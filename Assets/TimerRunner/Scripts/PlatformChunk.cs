using UnityEngine;

public class PlatformChunk : MonoBehaviour
{
    public float speed = 5f;
    private float originalSpeed;
    private bool isFrozen = false;

    void Start()
    {
        originalSpeed = speed;
    }

    void Update()
    {
        if (isFrozen) return;

        // Move the chunk left
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void StartRewind()
    {
        speed = -originalSpeed;
    }

    public void StopRewind()
    {
        speed = originalSpeed;
    }

    public void Freeze(bool freeze)
    {
        isFrozen = freeze;
    }
}