// Scripts/Factories/TileFactory.cs
using UnityEngine;
using System.Collections.Generic;

public static class TileFactory
{
    public static GameObject CreateTile(HexTileData tileData, Vector3 position, Transform parent)
    {
        if (tileData == null || tileData.Prefab == null)
        {
            Debug.LogWarning($"Tile data or prefab is missing for terrain: {tileData?.TerrainName}");
            return null;
        }

        GameObject tile = Object.Instantiate(tileData.Prefab, position, Quaternion.identity, parent);
        ApplyVisualVariation(tile, tileData);
        return tile;
    }

    private static void ApplyVisualVariation(GameObject tile, HexTileData tileData)
    {
        Renderer renderer = tile.GetComponent<Renderer>();
        if (renderer == null) return;

        if (tileData.ColorVariations.Count > 0)
            renderer.material.color = tileData.ColorVariations[Random.Range(0, tileData.ColorVariations.Count)];

        if (tileData.TextureVariations.Count > 0)
            renderer.material.mainTexture = tileData.TextureVariations[Random.Range(0, tileData.TextureVariations.Count)];
    }
}
