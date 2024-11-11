// Scripts/Grid/HexMapVisuals.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(HexGrid))]
public class HexMapVisuals : MonoBehaviour
{
    public HexTileData[] tileDataArray;                    // Array of terrain types (HexTileData assets)
    private HexCell startingLocation;                      // Initial starting location
    public HexCell GetStartingLocation() => startingLocation; // For camera positioning or other uses

    public Dictionary<int, HexTileData> tileDataDictionary; // Maps terrain ID to HexTileData
    public HexGrid hexGrid;

    [Header("Biome Config")]
    public BiomeConfig biomeConfig; // Reference to the BiomeConfig scriptable object

    [Header("Noise Settings")]
    public int seed;                 // Seed for noise generation
    public float scale = 10f;        // Scale of the noise
    public int octaves = 4;          // Number of noise layers
    [Range(0, 1)] public float persistence = 0.5f;
    public float lacunarity = 2.0f;
    public Vector2 offset;

    void Start()
    {
        if (seed == -1) seed = Random.Range(0, 100000);

        hexGrid = GetComponent<HexGrid>();
        InitializeTileDataDictionary();
        RenderMap();
        SetRandomStartingLocation();
    }

    // Initializes dictionary for terrain types based on ID
    private void InitializeTileDataDictionary()
    {
        tileDataDictionary = new Dictionary<int, HexTileData>();
        foreach (var tileData in tileDataArray)
        {
            if (tileData != null)
            {
                tileDataDictionary[tileData.Id] = tileData;  // Corrected to tileData.Id
            }
        }
    }

    // Renders the map based on noise configuration and terrain data
    private void RenderMap()
    {
        foreach (var cellEntry in hexGrid.cells)
        {
            HexCell cell = cellEntry.Value;
            Vector3 position = HexCoordinateHelper.GetWorldPosition(
                cell.OffsetCoordinates, hexGrid.UseFlatTopOrientation, hexGrid.tileSizeX, hexGrid.tileSizeZ
            );

            int terrainID = DetermineTerrainID(cell.OffsetCoordinates);
            if (tileDataDictionary.TryGetValue(terrainID, out HexTileData tileData))
            {
                cell.TerrainType = tileData;
                cell.VisualRepresentation = TileFactory.CreateTile(tileData, position, cell.transform);
            }
        }
    }

    // Determine the terrain ID based on noise or biome logic
    private int DetermineTerrainID(Vector2 offsetCoords)
    {
        float noiseValue = GenerateMultiOctaveNoise(offsetCoords);
        // Assuming biomeConfig.GetTerrainIDForNoise would return an ID based on noise
        return biomeConfig.GetTerrainIDForNoise(noiseValue);
    }

    // Generates multi-octave noise for biome variation
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

        return Mathf.InverseLerp(-1f, 1f, noiseHeight); 
    }

    public void SetRandomStartingLocation()
    {
        List<HexCell> walkableTiles = new List<HexCell>();
        foreach (var cell in hexGrid.cells.Values)
        {
            if (cell.TerrainType != null && cell.TerrainType.isWalkable)
            {
                walkableTiles.Add(cell);
            }
        }

        if (walkableTiles.Count > 0)
        {
            startingLocation = walkableTiles[Random.Range(0, walkableTiles.Count)];
            DisableFogInArea(startingLocation, 3);
        }
    }

    private void DisableFogInArea(HexCell centerCell, int range)
    {
        List<HexCell> hexesInRange = hexGrid.GetHexesInRange(centerCell, range);
        foreach (HexCell cell in hexesInRange)
        {
            var fog = cell.VisualRepresentation?.transform.Find("FogOfWar");
            if (fog) fog.gameObject.SetActive(false);
        }
    }
}
