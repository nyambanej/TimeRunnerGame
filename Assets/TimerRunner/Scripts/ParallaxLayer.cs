using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float speed = 1f;
    [HideInInspector] public float backgroundWidth;
    [HideInInspector] public Transform[] tilePool;

    private float direction = -1f;

    void Update()
    {
        // Automatically adjust direction based on RewindManager
        direction = RewindManager.IsRewinding() ? 1f : -1f;

        float move = direction * speed * Time.deltaTime;

        foreach (Transform tile in tilePool)
            tile.position += new Vector3(move, 0, 0);

        for (int i = 0; i < tilePool.Length; i++)
        {
            Transform tile = tilePool[i];

            if (direction < 0 && tile.position.x < GetLeftMostX() - backgroundWidth)
                tile.position += new Vector3(backgroundWidth * tilePool.Length, 0, 0);

            else if (direction > 0 && tile.position.x > GetRightMostX() + backgroundWidth)
                tile.position -= new Vector3(backgroundWidth * tilePool.Length, 0, 0);
        }
    }

    float GetLeftMostX() => tilePool[0].position.x;
    float GetRightMostX() => tilePool[tilePool.Length - 1].position.x;
}
