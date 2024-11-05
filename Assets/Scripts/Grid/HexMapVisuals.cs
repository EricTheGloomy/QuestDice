// Scripts/Grid/HexMapVisuals.cs
using UnityEngine;

[RequireComponent(typeof(HexGrid))]
public class HexMapVisuals : MonoBehaviour
{
    public GameObject basicHexPrefab;
    public HexTileData[] tileTypes;

    private HexGrid hexGrid;
    private float noiseSeed;

    void Start()
    {
        hexGrid = GetComponent<HexGrid>();
        noiseSeed = Random.Range(0f, 10000000f);
        RenderMap();
    }

    private void RenderMap()
    {
        foreach (var cellEntry in hexGrid.cells)
        {
            HexCell cell = cellEntry.Value;
            Vector3 position = HexCoordinateHelper.GetWorldPosition(cell.OffsetCoordinates, hexGrid.HexOrientation, hexGrid.tileSizeX, hexGrid.tileSizeZ);
            
            GameObject hexTile = Instantiate(basicHexPrefab, position, Quaternion.identity, cell.transform);
            cell.VisualRepresentation = hexTile;

            ApplyTerrainTypeBasedOnNoise(cell, hexTile);
        }
    }

    private void ApplyTerrainTypeBasedOnNoise(HexCell cell, GameObject hexTile)
    {
        HexTileData terrainType = DetermineTerrainType(cell.OffsetCoordinates);
        if (terrainType != cell.TerrainType)
        {
            cell.TerrainType = terrainType;

            // Destroy the basic tile
            Destroy(hexTile);

            // Check if the terrainType's prefab is assigned
            if (terrainType.Prefab != null)
            {
                GameObject terrainPrefab = terrainType.Prefab;
                GameObject newTile = Instantiate(terrainPrefab, hexTile.transform.position, Quaternion.identity, cell.transform);
                cell.VisualRepresentation = newTile;

                ApplyVisualVariation(newTile, terrainType);
            }
            else
            {
                Debug.LogWarning($"Prefab not assigned for terrain type: {terrainType.TerrainName}. Please assign a prefab in the Inspector.");
            }
        }
    }

    private HexTileData DetermineTerrainType(Vector2 offsetCoords)
    {
        float noiseValue = Mathf.PerlinNoise((offsetCoords.x + noiseSeed) / 10f, (offsetCoords.y + noiseSeed) / 10f);
        return noiseValue < 0.3f ? tileTypes[0] : noiseValue > 0.6f ? tileTypes[1] : tileTypes[2];
    }

    private void ApplyVisualVariation(GameObject tile, HexTileData terrainType)
    {
        Renderer renderer = tile.GetComponent<Renderer>();
        if (renderer == null) return;

        if (terrainType.ColorVariations.Count > 0)
            renderer.material.color = terrainType.ColorVariations[Random.Range(0, terrainType.ColorVariations.Count)];

        if (terrainType.TextureVariations.Count > 0)
            renderer.material.mainTexture = terrainType.TextureVariations[Random.Range(0, terrainType.TextureVariations.Count)];
    }
}
