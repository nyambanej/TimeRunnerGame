using UnityEngine;

public class FallingMine : MonoBehaviour
{
    public float fallSpeed = 4f;
    public GameObject explosionPrefab;

    private bool isFrozen = false;
    private bool hasLanded = false;
    private bool isRewinding = false;

    private Rigidbody2D rb;
    private Vector3 originalPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isRewinding)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, fallSpeed * Time.deltaTime);
            return;
        }

        if (isFrozen || hasLanded)
            return;

        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }


    public void Freeze(bool freeze)
    {
        isFrozen = freeze;
    }

    public void StartRewind()
    {
        isRewinding = true;
        hasLanded = false;
        isFrozen = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        transform.SetParent(null); // Detach from platform if parented
    }

    public void StopRewind()
    {
        isRewinding = false;
        isFrozen = false;

        if (rb != null)
        {
            rb.simulated = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasLanded && collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;

            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }

            transform.SetParent(collision.transform);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasLanded && other.CompareTag("Player"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
