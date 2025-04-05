using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject fallingMinePrefab;

    [Header("Spawn Settings")]
    public float fallingMineRate = 5f;

    private bool isSpawning = true;

    void Start()
    {
        InvokeRepeating(nameof(SpawnFallingMine), 1f, fallingMineRate);
    }

    public void StartRewind()
    {
        CancelInvoke(nameof(SpawnFallingMine));
        isSpawning = false;
    }

    public void StopRewind()
    {
        // Wait 2 seconds before resuming spawning
        Invoke(nameof(ResumeSpawning), 2f);
    }

    private void ResumeSpawning()
    {
        if (!isSpawning) // Only resume if it hasn't already resumed
        {
            isSpawning = true;
            InvokeRepeating(nameof(SpawnFallingMine), 0f, fallingMineRate);
        }
    }

    void SpawnFallingMine()
    {
        if (!isSpawning || fallingMinePrefab == null)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float offsetX = 6f;
        float spawnRange = 0.1f;

        float spawnX = player.transform.position.x + offsetX + Random.Range(-spawnRange, spawnRange);
        float spawnY = Camera.main.transform.position.y + Camera.main.orthographicSize + 1f;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        GameObject mine = Instantiate(fallingMinePrefab, spawnPosition, Quaternion.identity);

        if (!mine.GetComponent<FallingMine>())
            mine.AddComponent<FallingMine>();
    }
}
