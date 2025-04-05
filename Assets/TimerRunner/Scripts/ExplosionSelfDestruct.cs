using UnityEngine;

public class ExplosionSelfDestruct : MonoBehaviour
{
    public float destroyAfter = 1f; // Match your explosion animation duration
    public AudioSource explosionSound; // Drag this in from the prefab

    void Start()
    {

        if (explosionSound != null)
        {
            explosionSound.Play();
        }

        Destroy(gameObject, destroyAfter);
    }
}
