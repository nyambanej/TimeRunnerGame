using UnityEngine;

public class FallingMine : MonoBehaviour
{
    public float fallSpeed = 4f;
    public GameObject explosionPrefab;

    private bool isFrozen = false;
    private bool hasLanded = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isFrozen || hasLanded)
            return;

        // Fall manually
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    public void Freeze(bool freeze)
    {
        isFrozen = freeze;
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
                // Don't disable simulation! We want trigger events to still work
                // rb.simulated = false;
            }

            // Parent to platform
            transform.SetParent(collision.transform);
        }

        // Optional: explode if player is touched while still falling
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
