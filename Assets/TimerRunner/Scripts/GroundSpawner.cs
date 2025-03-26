using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] groundChunks;
    public GameObject longFloorPrefab;

    [Header("Spawn Settings")]
    public float spawnY = -3.5f;
    public float chunkSpacing = 5f;
    public float environmentSpeed = 5f;
    public float transitionBufferTime = 3f;

    private float distanceTracker = 0f;
    private float nextSpawnX;
    private MapTransition transitionManager;

    private bool spawnedLongFloor1 = false;
    private bool spawnedLongFloor2 = false;

    void Start()
    {
        transitionManager = FindObjectOfType<MapTransition>();
        nextSpawnX = transform.position.x;
        SpawnChunk();
        nextSpawnX += chunkSpacing;
    }

    void Update()
    {
        distanceTracker += environmentSpeed * Time.deltaTime;

        if (distanceTracker >= chunkSpacing)
        {
            distanceTracker = 0f;
            SpawnChunk();
            nextSpawnX += chunkSpacing;
        }
    }

    void SpawnChunk()
    {
        if ((groundChunks.Length == 0 && longFloorPrefab == null))
        {
            Debug.LogWarning("No ground prefabs assigned!");
            return;
        }

        Vector3 spawnPos = new Vector3(nextSpawnX, spawnY, 0f);

        if (transitionManager)
        {
            float time = transitionManager.GetElapsedTime();

            float transition1 = transitionManager.transitionTime;
            float transition2 = transitionManager.transitionTime2; // Add this to your MapTransition script

            bool nearFirst = time >= (transition1 - transitionBufferTime) && time <= (transition1 + transitionBufferTime);
            bool nearSecond = time >= (transition2 - transitionBufferTime) && time <= (transition2 + transitionBufferTime);

            if (nearFirst && !spawnedLongFloor1)
            {
                SpawnLongFloor();
                spawnedLongFloor1 = true;
                return;
            }

            if (nearSecond && !spawnedLongFloor2)
            {
                SpawnLongFloor();
                spawnedLongFloor2 = true;
                return;
            }
        }

        // Default: spawn normal chunk
        GameObject prefab = groundChunks[Random.Range(0, groundChunks.Length)];
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    void SpawnLongFloor()
    {
        float yOffset = 8.5f;
        Vector3 adjustedSpawnPos = new Vector3(nextSpawnX, spawnY + yOffset, 0f);
        Instantiate(longFloorPrefab, adjustedSpawnPos, Quaternion.identity);
    }
}
