using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Array to hold obstacle types
    public float spawnRate = 2f; // Time between spawns
    public float minY = -2f; // Lowest possible spawn height
    public float maxY = 2f;  // Highest possible spawn height
    public float moveSpeed = 5f; // Speed of obstacles moving left

    void Start()
    {
        // Start spawning obstacles at set intervals
        InvokeRepeating(nameof(SpawnObstacle), 1f, spawnRate);
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0) return; // Ensure obstacles exist

        // Choose a random obstacle
        int index = Random.Range(0, obstaclePrefabs.Length);
        float groundY = -4.1f; // Set this to match your ground height
        float spawnOffset = 4f; // How far off-screen obstacles should spawn
        float spawnX = Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect) + spawnOffset;
        Vector3 spawnPosition = new Vector3(spawnX, groundY, 0);



        // Instantiate the obstacle
        GameObject obstacle = Instantiate(obstaclePrefabs[index], spawnPosition, Quaternion.identity);

        // Ensure obstacle moves left
        MoveLeft moveScript = obstacle.GetComponent<MoveLeft>();
        if (moveScript == null)
        {
            moveScript = obstacle.AddComponent<MoveLeft>(); // Add MoveLeft script if missing
        }
        moveScript.speed = moveSpeed; // Set speed for moving left
    }
}
