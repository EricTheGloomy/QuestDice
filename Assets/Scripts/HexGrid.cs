using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public GameObject _hexPrefabGrass;  // The hex tile prefab to instantiate
    public GameObject _hexPrefabWater;
    public GameObject _hexPrefabTrees;
    public int _mapWidth;          // Width of the hex grid
    public int _mapHeight;         // Height of the hex grid
    public float _tileSizeX;       // Horizontal spacing between hex centers
    public float _tileSizeZ;       // Vertical spacing between hex centers
    public bool useFlatTop = true; // Toggle to choose between flat-top and pointy-top hexes

    //Noise settings
    public float _noiseSeed = 123456f;
    public float _noiseFrequency = 100f;
    public float _waterThreshold = 0.4f;
    public float _treesThreshold = 0.7f;

    void Start()
    {
        // Choose the correct grid generation method based on the hex orientation
        if (useFlatTop)
            CreateFlatTopHexGrid();
        else
            CreatePointyTopHexGrid();
    }

    // Method to calculate coordinates for flat-top hexes
    private Vector2 GetFlatTopHexCoords(int x, int z)
    {
        float xPos = x * _tileSizeX * Mathf.Cos(Mathf.Deg2Rad * 30); // Horizontal position
        float zPos = z * _tileSizeZ + ((x % 2 == 1) ? _tileSizeZ * 0.5f : 0); // Staggered row

        return new Vector2(xPos, zPos); // Return position as Vector2 for simplicity
    }

    // Method to calculate coordinates for pointy-top hexes
    private Vector2 GetPointyTopHexCoords(int x, int z)
    {
        float xPos = x * _tileSizeX + ((z % 2 == 1) ? _tileSizeX * 0.5f : 0); // Staggered column
        float zPos = z * _tileSizeZ * Mathf.Cos(Mathf.Deg2Rad * 30); // Vertical position

        return new Vector2(xPos, zPos); // Return position as Vector2 for simplicity
    }

    // Method to generate a flat-top hex grid
    void CreateFlatTopHexGrid()
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int z = 0; z < _mapHeight; z++)
            {
                // Get hex coordinates for each tile
                Vector2 hexCoords = GetFlatTopHexCoords(x, z);

                // Convert 2D coordinates into a 3D position
                Vector3 position = new Vector3(hexCoords.x, 0, hexCoords.y);

                GameObject prefab = _hexPrefabGrass;

                //random seed
                if(_noiseSeed == -1f)
                {
                    _noiseSeed = Random.Range(0f, 10000000f);
                }

                //get noise values
                float noiseValue = Mathf.PerlinNoise((hexCoords.x + _noiseSeed) / _noiseFrequency, (hexCoords.y + _noiseSeed) / _noiseFrequency);

                if(noiseValue < _waterThreshold)
                {
                    prefab = _hexPrefabWater;
                }
                if(noiseValue > _treesThreshold)
                {
                    prefab = _hexPrefabTrees;
                }

                // Instantiate the hex tile at the calculated position
                GameObject hexTile = Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }

    // Method to generate a pointy-top hex grid
    void CreatePointyTopHexGrid()
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int z = 0; z < _mapHeight; z++)
            {
                // Get hex coordinates for each tile
                Vector2 hexCoords = GetPointyTopHexCoords(x, z);

                // Convert 2D coordinates into a 3D position
                Vector3 position = new Vector3(hexCoords.x, 0, hexCoords.y);

                GameObject prefab = _hexPrefabGrass;

                //random seed
                if(_noiseSeed == -1f)
                {
                    _noiseSeed = Random.Range(0f, 10000000f);
                }

                //get noise values
                float noiseValue = Mathf.PerlinNoise((hexCoords.x + _noiseSeed) / _noiseFrequency, (hexCoords.y + _noiseSeed) / _noiseFrequency);

                if(noiseValue < _waterThreshold)
                {
                    prefab = _hexPrefabWater;
                }
                if(noiseValue > _treesThreshold)
                {
                    prefab = _hexPrefabTrees;
                }

                // Instantiate the hex tile at the calculated position
                GameObject hexTile = Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }

    //TO DO
    //replacing tiles for other tiles if X happens
    //NEEDS design first - to do later
    /*
    public void ReplaceTile (GameObject oldTile, GameObject newTile)
    {
        Instantiate(newTile, oldTile.transform);
        Destroy(oldTile);
    }
    */
}
