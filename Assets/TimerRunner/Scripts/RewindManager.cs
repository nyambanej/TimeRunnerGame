using IndieMarc.Platformer;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public static RewindManager Instance { get; private set; }

    public float rewindDuration = 3f;
    public float rewindCooldown = 5f;

    private bool isRewinding = false;
    private float rewindTimer = 0f;
    private float cooldownTimer = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Update()
    {
        if (!isRewinding && cooldownTimer <= 0f && Input.GetKeyDown(KeyCode.F))
        {
            StartRewind();
        }

        if (isRewinding)
        {
            rewindTimer += Time.deltaTime;
            if (rewindTimer >= rewindDuration)
            {
                StopRewind();
            }
        }
        else if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
        rewindTimer = 0f;
        cooldownTimer = rewindCooldown;

        // Platforms move in reverse direction
        foreach (var platform in FindObjectsByType<PlatformChunk>(FindObjectsSortMode.None))
            platform.StartRewind();

        // Player keeps moving
        FindFirstObjectByType<PlayerCharacter>()?.Freeze(false);

        // Notify managers
        FindFirstObjectByType<WindManager>()?.StartRewind();
        FindFirstObjectByType<ObstacleSpawner>()?.StartRewind();
        FindFirstObjectByType<GroundSpawner>()?.StartRewind();

        // Start rewind for all falling mines
        foreach (var mine in FindObjectsByType<FallingMine>(FindObjectsSortMode.None))
            mine.StartRewind();

        // Reverse all objects with MoveLeft
        foreach (var mover in FindObjectsByType<MoveLeft>(FindObjectsSortMode.None))
            mover.speed *= -1f;
    }

    public void StopRewind()
    {
        isRewinding = false;

        // Platforms go back to forward direction
        foreach (var platform in FindObjectsByType<PlatformChunk>(FindObjectsSortMode.None))
            platform.StopRewind();

        FindFirstObjectByType<PlayerCharacter>()?.Freeze(false);

        FindFirstObjectByType<WindManager>()?.StopRewind();
        FindFirstObjectByType<ObstacleSpawner>()?.StopRewind();
        FindFirstObjectByType<GroundSpawner>()?.StopRewind();

        foreach (var wind in FindObjectsByType<WindLine>(FindObjectsSortMode.None))
            Destroy(wind.gameObject);

        foreach (var mine in FindObjectsByType<FallingMine>(FindObjectsSortMode.None))
            mine.StopRewind();

        // Reset MoveLeft direction to positive
        foreach (var mover in FindObjectsByType<MoveLeft>(FindObjectsSortMode.None))
            mover.speed = Mathf.Abs(mover.speed);
    }

    public static bool IsRewinding() => Instance != null && Instance.isRewinding;
}
