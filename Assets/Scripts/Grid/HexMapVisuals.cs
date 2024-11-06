// Scripts/Grid/HexMapVisuals.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(HexGrid))]
public class HexMapVisuals : MonoBehaviour
{
    public GameObject basicHexPrefab;
    public HexTileData[] tileDataArray;  // Populate this array in the Inspector

    private Dictionary<TileType, HexTileData> tileDataDictionary;
    private HexGrid hexGrid;
    private float noiseSeed;

    void Start()
    {
        hexGrid = GetComponent<HexGrid>();
        noiseSeed = Random.Range(0f, 10000000f);

        InitializeTileDataDictionary();
        RenderMap();
    }

    private void InitializeTileDataDictionary()
    {
        tileDataDictionary = new Dictionary<TileType, HexTileData>();
        foreach (var tileData in tileDataArray)
        {
            if (tileData != null)
            {
                tileDataDictionary[tileData.Type] = tileData;
            }
            else
            {
                Debug.LogWarning("Null entry in tileDataArray. Please check the array in the Inspector.");
            }
        }
    }

    private void RenderMap()
    {
        foreach (var cellEntry in hexGrid.cells)
        {
            HexCell cell = cellEntry.Value;
            Vector3 position = HexCoordinateHelper.GetWorldPosition(cell.OffsetCoordinates, hexGrid.UseFlatTopOrientation, hexGrid.tileSizeX, hexGrid.tileSizeZ);

            GameObject hexTile = Instantiate(basicHexPrefab, position, Quaternion.identity, cell.transform);
            cell.VisualRepresentation = hexTile;

            AssignTileTypeBasedOnNoise(cell, hexTile);
        }
    }

    private void AssignTileTypeBasedOnNoise(HexCell cell, GameObject hexTile)
    {
        TileType tileType = DetermineTileType(cell.OffsetCoordinates);
        HexTileData tileData = tileDataDictionary[tileType];

        if (tileData != null)
        {
            // Use TileFactory to create and configure the tile with appropriate prefab and variations
            Destroy(hexTile); // Destroy the placeholder tile before replacing it
            GameObject newTile = TileFactory.CreateTile(tileData, hexTile.transform.position, cell.transform);

            if (newTile != null)
            {
                cell.TerrainType = tileData;
                cell.VisualRepresentation = newTile;
            }
        }
        else
        {
            Debug.LogWarning($"No data found for tile type: {tileType}. Please ensure all tile types have assigned data.");
        }
    }

    private TileType DetermineTileType(Vector2 offsetCoords)
    {
        float noiseValue = Mathf.PerlinNoise((offsetCoords.x + noiseSeed) / 10f, (offsetCoords.y + noiseSeed) / 10f);
        if (noiseValue < 0.3f) return TileType.Water;
        if (noiseValue > 0.6f) return TileType.Forest;
        return TileType.Grass;
    }
}
