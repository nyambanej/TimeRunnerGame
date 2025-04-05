using UnityEngine;

public class ExplosionSelfDestruct : MonoBehaviour
{
    public float destroyAfter = 1f; // Match your explosion animation duration

    void Start()
    {
        Destroy(gameObject, destroyAfter);
    }
}
