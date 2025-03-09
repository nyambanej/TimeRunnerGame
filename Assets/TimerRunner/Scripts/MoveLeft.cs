using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Destroy objects once they move off-screen
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}
