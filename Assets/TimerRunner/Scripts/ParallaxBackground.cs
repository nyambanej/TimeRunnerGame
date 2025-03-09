using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float speedMultiplier = 1f; // Adjust per layer in the Inspector
    private float startX;
    private float length;

    void Start()
    {
        startX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Move left continuously based on speed
        float temp = transform.position.x - (speedMultiplier * Time.deltaTime);
        transform.position = new Vector3(temp, transform.position.y, transform.position.z);

        // Reset the position when it moves too far left
        if (transform.position.x < startX - length)
        {
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        }
    }
}
