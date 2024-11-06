// Scripts/Grid/HexMapVisuals.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(HexGrid))]
public class HexMapVisuals : MonoBehaviour
{
    public GameObject basicHexPrefab;
    public HexTileData[] tileDataArray;

    [Header("Noise Settings")]
    /// <summary>
    // public float scale
    // Controls the "zoom" level of the noise. 
    // Higher values create larger, smoother regions, while lower values create more detailed, smaller areas. 
    // Setting scale too low or zero may result in overly noisy output or runtime errors.
    // public int octaves
    // The number of layers of noise added together to form the final map. 
    // Each octave increases the detail level by adding noise at a different frequency and amplitude. 
    // More octaves create more complex terrain but are computationally heavier.
    // float persistence
    // Controls the reduction in amplitude of each octave. 
    // Lower values result in a smoother, more blended terrain, as each subsequent octave contributes less. 
    // Values closer to 1 make octaves contribute more equally, producing rougher terrain.
    //float lacunarity
    // Controls the increase in frequency of each octave. 
    // Higher values create increasingly smaller features in each octave, adding more high-frequency detail to the map.
    /// </summary>
    public int seed;
    public float scale = 10f;
    public int octaves = 4;
    [Range(0, 1)] public float persistence = 0.5f;
    public float lacunarity = 2.0f;
    public Vector2 offset;


    private Dictionary<TileType, HexTileData> tileDataDictionary;
    private HexGrid hexGrid;

    [Header("Biome Config")]
    public BiomeConfig biomeConfig;

    void Start()
    {
        if(seed == -1)
        {
            seed = Random.Range(0, 100000);
        }

        hexGrid = GetComponent<HexGrid>();

        InitializeTileDataDictionary();
        RenderMap();
        SetRandomStartingLocation();
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

            TileType tileType = DetermineTileType(cell.OffsetCoordinates);
            HexTileData tileData = tileDataDictionary[tileType];

            if (tileData != null)
            {
                GameObject tile = TileFactory.CreateTile(tileData, position, cell.transform);
                if (tile != null)
                {
                    cell.TerrainType = tileData;
                    cell.VisualRepresentation = tile;
                }
            }
            else
            {
                Debug.LogWarning($"No data found for tile type: {tileType}. Please ensure all tile types have assigned data.");
            }
        }
    }

    private void AssignTileTypeBasedOnNoise(HexCell cell, GameObject hexTile)
    {
        TileType tileType = DetermineTileType(cell.OffsetCoordinates);
        HexTileData tileData = tileDataDictionary[tileType];

        if (tileData != null)
        {
            Destroy(hexTile);
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

    /*private TileType DetermineTileType(Vector2 offsetCoords)
    {
        float noiseValue = GenerateMultiOctaveNoise(offsetCoords);
        
        if (noiseValue < 0.3f) return TileType.Water;
        if (noiseValue > 0.6f) return TileType.Forest;
        return TileType.Grass;
    }*/
    private TileType DetermineTileType(Vector2 offsetCoords)
    {
        float noiseValue = GenerateMultiOctaveNoise(offsetCoords);
        return biomeConfig.GetTileTypeForNoise(noiseValue);
    }

    private float GenerateMultiOctaveNoise(Vector2 offsetCoords)
    {
        if (scale <= 0) scale = 0.0001f; // Prevent division by zero

        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        for (int i = 0; i < octaves; i++)
        {
            float sampleX = (offsetCoords.x / scale * frequency) + octaveOffsets[i].x;
            float sampleY = (offsetCoords.y / scale * frequency) + octaveOffsets[i].y;

            float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        return Mathf.InverseLerp(-1f, 1f, noiseHeight); // Normalize to 0-1
    }

    // In HexMapVisuals.cs
    public void SetRandomStartingLocation()
    {
        // Collect all grass tiles
        List<HexCell> grassTiles = new List<HexCell>();
        foreach (var cell in hexGrid.cells.Values)
        {
            if (cell.TerrainType != null && cell.TerrainType.Type == TileType.Grass)
            {
                grassTiles.Add(cell);
            }
        }

        // Choose a random grass tile as the starting location
        if (grassTiles.Count == 0)
        {
            Debug.LogWarning("No grass tiles found on the map. Cannot set a starting location.");
            return;
        }

        HexCell startCell = grassTiles[Random.Range(0, grassTiles.Count)];

        // Disable fog on the starting tile and its neighbors
        DisableFogInArea(startCell, 3);
    }

    // Helper method to disable fog in an area around a central tile
    private void DisableFogInArea(HexCell centerCell, int range)
    {
        List<HexCell> hexesInRange = hexGrid.GetHexesInRange(centerCell, range);
        foreach (HexCell cell in hexesInRange)
        {
            Transform fogOfWar = cell.VisualRepresentation.transform.Find("FogOfWar");
            if (fogOfWar != null)
            {
                fogOfWar.gameObject.SetActive(false);
            }
        }
    }

}
