using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float speedMultiplier = 1f;
    private float startX;
    private float length;
    private float originalSpeed;
    private bool isRewinding = false;

    void Start()
    {
        startX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        originalSpeed = speedMultiplier;
    }

    void Update()
    {

        // Move background
        transform.position += Vector3.left * speedMultiplier * Time.deltaTime;

        // Loop background when it goes off screen
        if (transform.position.x < startX - length)
        {
            transform.position += new Vector3(length * 2, 0, 0);
        }
        else if (transform.position.x > startX + length) // needed for rewinding
        {
            transform.position -= new Vector3(length * 2, 0, 0);
        }
    }

    public void StartRewind()
    {
        if (!isRewinding)
        {
            speedMultiplier = -Mathf.Abs(originalSpeed);
            isRewinding = true;
        }
    }

    public void StopRewind()
    {
        if (isRewinding)
        {
            speedMultiplier = Mathf.Abs(originalSpeed);
            isRewinding = false;
        }
    }
}
