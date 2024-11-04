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

    void Start()
    {
        if (_noiseSeed == -1f)
        {
            _noiseSeed = Random.Range(0f, 10000000f);
        }

        if (useFlatTop)
            CreateFlatTopHexGrid();
        else
            CreatePointyTopHexGrid();
    }

    private Vector2 GetFlatTopHexCoords(int x, int z)
    {
        float xPos = x * _tileSizeX * Mathf.Cos(Mathf.Deg2Rad * 30);
        float zPos = z * _tileSizeZ + ((x % 2 == 1) ? _tileSizeZ * 0.5f : 0);
        return new Vector2(xPos, zPos);
    }

    private Vector2 GetPointyTopHexCoords(int x, int z)
    {
        float xPos = x * _tileSizeX + ((z % 2 == 1) ? _tileSizeX * 0.5f : 0);
        float zPos = z * _tileSizeZ * Mathf.Cos(Mathf.Deg2Rad * 30);
        return new Vector2(xPos, zPos);
    }

    void CreateFlatTopHexGrid()
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int z = 0; z < _mapHeight; z++)
            {
                Vector2 hexCoords = GetFlatTopHexCoords(x, z);
                Vector3 position = new Vector3(hexCoords.x, 0, hexCoords.y);
                GameObject prefab = SelectPrefab(hexCoords);
                Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }

    void CreatePointyTopHexGrid()
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int z = 0; z < _mapHeight; z++)
            {
                Vector2 hexCoords = GetPointyTopHexCoords(x, z);
                Vector3 position = new Vector3(hexCoords.x, 0, hexCoords.y);
                GameObject prefab = SelectPrefab(hexCoords);
                Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }

    private GameObject SelectPrefab(Vector2 hexCoords)
    {
        float noiseValue = Mathf.PerlinNoise((hexCoords.x + _noiseSeed) / _noiseFrequency, (hexCoords.y + _noiseSeed) / _noiseFrequency);
        if (noiseValue < _waterThreshold) return _hexPrefabWater;
        if (noiseValue > _treesThreshold) return _hexPrefabTrees;
        return _hexPrefabGrass;
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

