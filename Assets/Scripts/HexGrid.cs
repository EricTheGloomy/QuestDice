using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public GameObject _hexPrefabGrass;
    public GameObject _hexPrefabWater;
    public GameObject _hexPrefabTrees;
    public int _mapWidth;
    public int _mapHeight;
    public float _tileSizeX;
    public float _tileSizeZ;
    public bool useFlatTop = true;

    public float _noiseSeed = -1f;
    public float _noiseFrequency = 100f;
    public float _waterThreshold = 0.4f;
    public float _treesThreshold = 0.7f;

    public HexTileData[] tileTypes; // Array of different terrain types (ScriptableObjects)
    public Dictionary<Vector2, HexCell> cells = new Dictionary<Vector2, HexCell>();

    void Start()
    {
        if (_noiseSeed == -1f)
        {
            _noiseSeed = Random.Range(0f, 10000000f);
        }

        InitializeGrid();  // Initialize HexCells
        AssignNeighbors();  // Assign neighbors for pathfinding
    }

    private void InitializeGrid()
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int z = 0; z < _mapHeight; z++)
            {
                Vector2 offsetCoords = new Vector2(x, z);
                HexTileData terrainType = SelectTerrainType(offsetCoords);

                // Determine prefab based on terrain type
                GameObject prefab = SelectPrefab(terrainType);

                // Instantiate the tile at the calculated position
                Vector3 position = useFlatTop ? 
                    new Vector3(GetFlatTopHexCoords(x, z).x, 0, GetFlatTopHexCoords(x, z).y) : 
                    new Vector3(GetPointyTopHexCoords(x, z).x, 0, GetPointyTopHexCoords(x, z).y);

                GameObject tile = Instantiate(prefab, position, Quaternion.identity);

                // Attach HexCell component and initialize
                HexCell cell = tile.AddComponent<HexCell>();
                cell.Initialize(this, offsetCoords, terrainType);

                // Store in dictionary for easy lookup
                cells[offsetCoords] = cell;
            }
        }
    }

    private HexTileData SelectTerrainType(Vector2 offsetCoords)
    {
        float noiseValue = Mathf.PerlinNoise((offsetCoords.x + _noiseSeed) / _noiseFrequency,
                                             (offsetCoords.y + _noiseSeed) / _noiseFrequency);
        if (noiseValue < _waterThreshold) return tileTypes[0];  // Water
        if (noiseValue > _treesThreshold) return tileTypes[1];  // Trees
        return tileTypes[2];  // Grass
    }

    private void AssignNeighbors()
    {
        foreach (var cellEntry in cells)
        {
            Vector2 offsetCoords = cellEntry.Key;
            HexCell cell = cellEntry.Value;

            Vector2[] neighborOffsets = (int)offsetCoords.y % 2 == 0
                ? new Vector2[] { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 1),
                                  new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, -1) }
                : new Vector2[] { new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                                  new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1) };

            foreach (Vector2 offset in neighborOffsets)
            {
                Vector2 neighborCoords = offsetCoords + offset;
                if (cells.TryGetValue(neighborCoords, out HexCell neighbor))
                {
                    cell.AddNeighbor(neighbor);
                }
            }
        }
    }

    public Vector2 GetFlatTopHexCoords(int x, int z)
    {
        float xPos = x * _tileSizeX * Mathf.Cos(Mathf.Deg2Rad * 30);
        float zPos = z * _tileSizeZ + ((x % 2 == 1) ? _tileSizeZ * 0.5f : 0);
        return new Vector2(xPos, zPos);
    }

    public Vector2 GetPointyTopHexCoords(int x, int z)
    {
        float xPos = x * _tileSizeX + ((z % 2 == 1) ? _tileSizeX * 0.5f : 0);
        float zPos = z * _tileSizeZ * Mathf.Cos(Mathf.Deg2Rad * 30);
        return new Vector2(xPos, zPos);
    }

    private GameObject SelectPrefab(HexTileData terrainType)
    {
        if (terrainType == tileTypes[0]) return _hexPrefabWater;  // Water
        if (terrainType == tileTypes[1]) return _hexPrefabTrees;  // Trees
        return _hexPrefabGrass;                                   // Grass
    }
}
