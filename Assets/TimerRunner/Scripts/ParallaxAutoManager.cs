using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnSettings
{
    public string targetName;
    public float spacingMultiplier = 1f;
}

public class ParallaxAutoManager : MonoBehaviour
{
    public int tilesPerLayer = 6;
    public float[] speeds;
    public List<SpawnSettings> customSpacings;

    private ParallaxLayer[] parallaxLayers;

    void Start()
    {
        int layerIndex = 0;

        foreach (Transform layer in transform)
        {
            var originalTiles = new Transform[2];
            int childCount = layer.childCount;

            if (childCount < 1)
            {
                Debug.LogWarning($"Layer {layer.name} has no children to clone.");
                continue;
            }

            originalTiles[0] = layer.GetChild(0);
            originalTiles[1] = (childCount > 1) ? layer.GetChild(1) : layer.GetChild(0);

            SpriteRenderer refRenderer = originalTiles[0].GetComponent<SpriteRenderer>();
            if (refRenderer == null) continue;

            float pixelsPerUnit = refRenderer.sprite.pixelsPerUnit;
            float textureWidth = refRenderer.sprite.rect.width;
            float baseWidth = (textureWidth / pixelsPerUnit) * originalTiles[0].lossyScale.x;

            for (int i = 0; i < childCount; i++)
                layer.GetChild(i).gameObject.SetActive(false);

            Transform[] tilePool = new Transform[tilesPerLayer];

            for (int i = 0; i < tilesPerLayer; i++)
            {
                Transform prefab = originalTiles[i % 2];
                Transform tile = Instantiate(prefab, layer);
                tile.name = $"{layer.name}_Tile_{i}";
                tile.gameObject.SetActive(true);

                SpriteRenderer tileSR = tile.GetComponent<SpriteRenderer>();
                if (tileSR != null)
                    tileSR.flipX = (i % 2 == 1);

                float spacing = GetSpacingMultiplier(prefab.name);

                Vector3 snappedPos;
                if (i == 0)
                {
                    snappedPos = prefab.localPosition;
                }
                else
                {
                    Transform prev = tilePool[i - 1];
                    SpriteRenderer prevSR = prev.GetComponent<SpriteRenderer>();

                    float width = prevSR.sprite.rect.width / prevSR.sprite.pixelsPerUnit * prev.localScale.x;
                    Vector3 prevPos = prev.localPosition;

                    snappedPos = new Vector3(
                        prevPos.x + width * spacing,
                        prefab.localPosition.y,
                        prefab.localPosition.z
                    );
                }

                tile.localPosition = RoundVector3(snappedPos, 3);
                tilePool[i] = tile;
            }

            var parallax = layer.GetComponent<ParallaxLayer>();
            if (parallax == null)
                parallax = layer.gameObject.AddComponent<ParallaxLayer>();

            parallax.speed = (speeds.Length > layerIndex) ? speeds[layerIndex] : 1f;
            parallax.backgroundWidth = baseWidth;
            parallax.tilePool = tilePool;

            layerIndex++;
        }

        // Cache all parallax layers (used externally if needed)
        parallaxLayers = FindObjectsOfType<ParallaxLayer>();
    }

    private float GetSpacingMultiplier(string name)
    {
        foreach (var setting in customSpacings)
        {
            if (name.Contains(setting.targetName))
                return Mathf.Max(0.1f, setting.spacingMultiplier);
        }
        return 1f;
    }

    private Vector3 RoundVector3(Vector3 v, int decimals)
    {
        float multiplier = Mathf.Pow(10, decimals);
        return new Vector3(
            Mathf.Round(v.x * multiplier) / multiplier,
            Mathf.Round(v.y * multiplier) / multiplier,
            Mathf.Round(v.z * multiplier) / multiplier
        );
    }
}
