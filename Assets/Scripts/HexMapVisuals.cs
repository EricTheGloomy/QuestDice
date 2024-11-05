// HexMapVisuals.cs
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HexGrid))]
public class HexMapVisuals : MonoBehaviour
{
    public GameObject basicHexPrefab;         // Basic hex prefab for each cell
    public float noiseSeed = -1f;
    public float noiseFrequency = 10f;
    public float waterThreshold = 0.3f;
    public float treesThreshold = 0.6f;

    public HexTileData[] tileTypes;           // Array of terrain types
    private HexGrid hexGrid;

    void Start()
    {
        hexGrid = GetComponent<HexGrid>();
        if (hexGrid == null)
        {
            Debug.LogError("HexGrid component not found.");
            return;
        }

        if (noiseSeed == -1f)
            noiseSeed = Random.Range(0f, 10000000f);

        RenderMap();
    }

    private void RenderMap()
    {
        foreach (var cellEntry in hexGrid.cells)
        {
            HexCell cell = cellEntry.Value;

            // Get the position based on grid orientation
            Vector3 position = hexGrid.useFlatTop
                ? new Vector3(hexGrid.GetFlatTopHexCoords((int)cell.OffsetCoordinates.x, (int)cell.OffsetCoordinates.y).x, 0,
                              hexGrid.GetFlatTopHexCoords((int)cell.OffsetCoordinates.x, (int)cell.OffsetCoordinates.y).y)
                : new Vector3(hexGrid.GetPointyTopHexCoords((int)cell.OffsetCoordinates.x, (int)cell.OffsetCoordinates.y).x, 0,
                              hexGrid.GetPointyTopHexCoords((int)cell.OffsetCoordinates.x, (int)cell.OffsetCoordinates.y).y);

            // Instantiate a basic hex tile prefab as a child of the cell
            GameObject hexTile = Instantiate(basicHexPrefab, position, Quaternion.identity, cell.transform);
            cell.VisualRepresentation = hexTile;

            ReplaceTileBasedOnNoise(cell, hexTile);
        }
    }

    private void ReplaceTileBasedOnNoise(HexCell cell, GameObject hexTile)
    {
        // Determine the terrain type based on noise
        HexTileData terrainType = SelectTerrainType(cell.OffsetCoordinates);
        
        // Check if the noise-based terrain type is different from the default (basic) tile
        if (terrainType != cell.TerrainType)
        {
            cell.TerrainType = terrainType;

            // Destroy the basic tile
            Destroy(hexTile);

            // Instantiate the appropriate prefab based on terrain type
            GameObject terrainPrefab = SelectPrefab(terrainType);
            GameObject newTile = Instantiate(terrainPrefab, hexTile.transform.position, Quaternion.identity, cell.transform);
            cell.VisualRepresentation = newTile;

            ApplyVisualVariation(newTile, terrainType);  // Apply color/texture variations
        }
    }

    private GameObject SelectPrefab(HexTileData terrainType)
    {
        if (terrainType == tileTypes[0]) return tileTypes[0].prefab;  // Water
        if (terrainType == tileTypes[1]) return tileTypes[1].prefab;  // Trees
        return tileTypes[2].prefab;                                   // Grass
    }

    private HexTileData SelectTerrainType(Vector2 offsetCoords)
    {
        float noiseValue = Mathf.PerlinNoise((offsetCoords.x + noiseSeed) / noiseFrequency,
                                             (offsetCoords.y + noiseSeed) / noiseFrequency);
        if (noiseValue < waterThreshold) return tileTypes[0];  // Water
        if (noiseValue > treesThreshold) return tileTypes[1];  // Trees
        return tileTypes[2];  // Grass
    }

    private void ApplyVisualVariation(GameObject tile, HexTileData terrainType)
    {
        Renderer renderer = tile.GetComponent<Renderer>();
        if (renderer == null) return;

        if (terrainType.colorVariations.Count > 0)
            renderer.material.color = terrainType.colorVariations[Random.Range(0, terrainType.colorVariations.Count)];

        if (terrainType.textureVariations.Count > 0)
            renderer.material.mainTexture = terrainType.textureVariations[Random.Range(0, terrainType.textureVariations.Count)];
    }
}
