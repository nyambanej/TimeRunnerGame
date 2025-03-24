using UnityEngine;

public class WindLine : MonoBehaviour
{
    public float speed = 2f;
    private bool isRewinding = false;
    private float originalSpeed;

    void Start()
    {
        originalSpeed = speed;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Destroy when off-screen
        if (transform.position.x < Camera.main.transform.position.x - 20f
            || transform.position.x > Camera.main.transform.position.x + 20f)
        {
            Destroy(gameObject);
        }
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
