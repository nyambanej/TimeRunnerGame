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
        // Move left continuously based on speed multiplier
        transform.position += Vector3.left * speedMultiplier * Time.deltaTime;

        // Check if the sprite has moved completely off screen and reposition it
        if (transform.position.x < startX - length)
        {
            transform.position += new Vector3(length * 2, 0, 0);
        }
    }
}
