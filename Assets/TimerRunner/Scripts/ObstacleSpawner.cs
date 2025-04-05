using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject fallingMinePrefab;

    [Header("Spawn Settings")]
    public float fallingMineRate = 5f; // How frequently mines spawn

    // We'll not track isRewinding here, as RewindManager handles it globally

    void Start()
    {
        // Start spawning falling mines
        InvokeRepeating(nameof(SpawnFallingMine), 1f, fallingMineRate);
    }

    // Called by RewindManager when rewind begins
    public void StartRewind()
    {
        // Stop spawning new falling mines
        CancelInvoke(nameof(SpawnFallingMine));

        // Optionally you could find mines and call StartRewind there, 
        // but RewindManager already does that.
    }

    // Called by RewindManager when rewind ends
    public void StopRewind()
    {
        // Resume spawning
        InvokeRepeating(nameof(SpawnFallingMine), 0f, fallingMineRate);
    }

    void SpawnFallingMine()
    {
        if (fallingMinePrefab == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float offsetX = 2f; // How far in front of the player the bomb should fall
        float spawnRange = 1.5f; // Slight variation for randomness

        float spawnX = player.transform.position.x + offsetX + Random.Range(-spawnRange, spawnRange);
        float spawnY = Camera.main.transform.position.y + Camera.main.orthographicSize + 1f;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        GameObject mine = Instantiate(fallingMinePrefab, spawnPosition, Quaternion.identity);

        if (!mine.GetComponent<FallingMine>())
            mine.AddComponent<FallingMine>();
    }

}
