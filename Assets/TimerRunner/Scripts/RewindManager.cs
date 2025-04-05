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

        // Freeze player & floors
        foreach (var platform in FindObjectsOfType<PlatformChunk>())
            platform.Freeze(true);

        FindObjectOfType<PlayerCharacter>()?.Freeze(true);
        FindObjectOfType<WindManager>()?.StartRewind();
        FindObjectOfType<ObstacleSpawner>()?.StartRewind();
        FindObjectOfType<GroundSpawner>()?.StartRewind();

        // Freeze all falling mines
        foreach (var mine in FindObjectsOfType<FallingMine>())
            mine.Freeze(true);

        // Freeze all floor tiles (MoveLeft-based)
        foreach (var mover in FindObjectsOfType<MoveLeft>())
            mover.speed = 0f;
    }

    public void StopRewind()
    {
        isRewinding = false;

        foreach (var platform in FindObjectsOfType<PlatformChunk>())
            platform.Freeze(false);

        FindObjectOfType<PlayerCharacter>()?.Freeze(false);
        FindObjectOfType<WindManager>()?.StopRewind();
        FindObjectOfType<ObstacleSpawner>()?.StopRewind();
        FindObjectOfType<GroundSpawner>()?.StopRewind();

        foreach (var wind in FindObjectsOfType<WindLine>())
            Destroy(wind.gameObject);

        foreach (var mine in FindObjectsOfType<FallingMine>())
            mine.Freeze(false);

        // Resume movement on all floor tiles (MoveLeft)
        foreach (var mover in FindObjectsOfType<MoveLeft>())
            mover.speed = FindObjectOfType<GroundSpawner>()?.environmentSpeed ?? 5f;
    }

    public static bool IsRewinding() => Instance != null && Instance.isRewinding;
}
