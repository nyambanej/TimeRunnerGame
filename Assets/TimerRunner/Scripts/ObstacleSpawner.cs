using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float spawnRate = 2f;
    public float minY = -2f;
    public float maxY = 2f;
    public float moveSpeed = 5f;

    private bool isRewinding = false;
    private float nextSpawnTime;
    private MoveLeft[] moveLeftScripts;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 1f, spawnRate);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            StopRewind();
        }
    }

    void StartRewind()
    {
        isRewinding = true;

        // Reverse movement direction of all existing obstacles
        moveLeftScripts = FindObjectsByType<MoveLeft>(FindObjectsSortMode.None);
        foreach (MoveLeft script in moveLeftScripts)
        {
            script.speed *= -1;
        }

        // Stop new obstacles from spawning
        CancelInvoke(nameof(SpawnObstacle));
    }

    void StopRewind()
    {
        isRewinding = false;

        // Restore movement direction of all obstacles
        moveLeftScripts = FindObjectsByType<MoveLeft>(FindObjectsSortMode.None);
        foreach (MoveLeft script in moveLeftScripts)
        {
            script.speed = Mathf.Abs(script.speed); // Ensure positive value
        }

        // Resume spawning
        InvokeRepeating(nameof(SpawnObstacle), 0f, spawnRate);
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0) return;

        int index = Random.Range(0, obstaclePrefabs.Length);
        float groundY = -4.1f;
        float spawnOffset = 4f;
        float spawnX = Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect) + spawnOffset;
        Vector3 spawnPosition = new Vector3(spawnX, groundY, 0);

        GameObject obstacle = Instantiate(obstaclePrefabs[index], spawnPosition, Quaternion.identity);

        MoveLeft moveScript = obstacle.GetComponent<MoveLeft>();
        if (moveScript == null)
        {
            moveScript = obstacle.AddComponent<MoveLeft>();
        }
        moveScript.speed = isRewinding ? -moveSpeed : moveSpeed;
    }
}
