using UnityEngine;

public class WindManager : MonoBehaviour
{
    public GameObject windLinePrefab;
    public float spawnInterval = 0.1f;
    public float minY = -2f;
    public float maxY = 2f;

    private bool isRewinding = false;

    void Start()
    {
        StartSpawning();
    }

    void SpawnWind()
    {
        if (isRewinding) return;

        float camX = Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect + 1f;
        float randY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(camX, randY, -1);

        GameObject wind = Instantiate(windLinePrefab, spawnPos, Quaternion.identity);
        wind.GetComponent<WindLine>().speed = Random.Range(3f, 10f);

        // Random scale for variety
        float length = Random.Range(2f, 5f);
        float thickness = Random.Range(0.02f, 0.05f);
        wind.transform.localScale = new Vector3(length, thickness, 1f);
    }

    private void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnWind), 0f, spawnInterval);
    }

    private void StopSpawning()
    {
        CancelInvoke(nameof(SpawnWind));
    }

    public void StartRewind()
    {
        isRewinding = true;
        StopSpawning();
    }

    public void StopRewind()
    {
        isRewinding = false;
        StartSpawning();
    }
}
