// Scripts/Grid/HexMapVisuals.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(HexGrid))]
public class HexMapVisuals : MonoBehaviour
{
    public GameObject basicHexPrefab;
    public HexTileData[] tileDataArray;
    public int startingVision = 3;
    private HexCell startingLocation;   

    public HexCell GetStartingLocation() => startingLocation;

    [Header("Noise Settings")]
    public int seed;
    public float scale = 10f;
    public int octaves = 4;
    [Range(0, 1)] public float persistence = 0.5f;
    public float lacunarity = 2.0f;
    public Vector2 offset;

    public Dictionary<TileType, HexTileData> tileDataDictionary;
    public HexGrid hexGrid;

    [Header("Biome Config")]
    public BiomeConfig biomeConfig;

    void Start()
    {
        if (seed == -1) seed = Random.Range(0, 100000);

        hexGrid = GetComponent<HexGrid>();
        InitializeTileDataDictionary();
        RenderMap();
        SetRandomStartingLocation();
    }

    // Initializes dictionary for tile types
    private void InitializeTileDataDictionary()
    {
        tileDataDictionary = new Dictionary<TileType, HexTileData>();
        foreach (var tileData in tileDataArray)
        {
            if (tileData != null)
            {
                tileDataDictionary[tileData.Type] = tileData;
            }
        }
    }

    // Render tiles based on noise configuration and tile data
    private void RenderMap()
    {
        foreach (var cellEntry in hexGrid.cells)
        {
            HexCell cell = cellEntry.Value;
            Vector3 position = HexCoordinateHelper.GetWorldPosition(
                cell.OffsetCoordinates, hexGrid.UseFlatTopOrientation, hexGrid.tileSizeX, hexGrid.tileSizeZ
            );

            TileType tileType = DetermineTileType(cell.OffsetCoordinates);
            HexTileData tileData = tileDataDictionary[tileType];

            if (tileData != null)
            {
                GameObject tile = TileFactory.CreateTile(tileData, position, cell.transform);
                cell.TerrainType = tileData;
                cell.VisualRepresentation = tile;
            }
        }
    }

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

        return Mathf.InverseLerp(-1f, 1f, noiseHeight); 
    }

    // Sets a random starting location on a grass tile and disables fog in the vicinity
    public void SetRandomStartingLocation()
    {
        List<HexCell> grassTiles = new List<HexCell>();
        foreach (var cell in hexGrid.cells.Values)
        {
            if (cell.TerrainType != null && cell.TerrainType.Type == TileType.Grass)
            {
                grassTiles.Add(cell);
            }
        }

        if (grassTiles.Count > 0)
        {
            startingLocation = grassTiles[Random.Range(0, grassTiles.Count)];
            DisableFogInArea(startingLocation, startingVision);
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
