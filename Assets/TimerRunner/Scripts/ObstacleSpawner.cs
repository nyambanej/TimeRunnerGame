using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject fallingMinePrefab; // ← Add this

    public float spawnRate = 2f;
    public float fallingMineRate = 5f; // ← Add this
    public float minY = -2f;
    public float maxY = 2f;
    public float moveSpeed = 5f;

    private bool isRewinding = false;
    private float nextSpawnTime;
    private MoveLeft[] moveLeftScripts;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 1f, spawnRate);
        InvokeRepeating(nameof(SpawnFallingMine), 1f, fallingMineRate); // ← Add this
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

        moveLeftScripts = FindObjectsByType<MoveLeft>(FindObjectsSortMode.None);
        foreach (MoveLeft script in moveLeftScripts)
        {
            script.speed *= -1;
        }

        CancelInvoke(nameof(SpawnObstacle));
        CancelInvoke(nameof(SpawnFallingMine)); // ← Pause falling too
    }

    void StopRewind()
    {
        isRewinding = false;

        moveLeftScripts = FindObjectsByType<MoveLeft>(FindObjectsSortMode.None);
        foreach (MoveLeft script in moveLeftScripts)
        {
            script.speed = Mathf.Abs(script.speed);
        }

        InvokeRepeating(nameof(SpawnObstacle), 0f, spawnRate);
        InvokeRepeating(nameof(SpawnFallingMine), 0f, fallingMineRate); // ← Resume falling
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

    void SpawnFallingMine()
    {
        if (fallingMinePrefab == null) return;

        float screenX = Camera.main.transform.position.x;
        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float spawnX = Random.Range(screenX - screenHalfWidth, screenX + screenHalfWidth);
        float spawnY = Camera.main.transform.position.y + Camera.main.orthographicSize + 1f;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        Instantiate(fallingMinePrefab, spawnPosition, Quaternion.identity);
    }
}
