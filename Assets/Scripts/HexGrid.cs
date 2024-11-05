// HexGrid.cs
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float tileSizeX = 2f;
    public float tileSizeZ = 2f;
    public bool useFlatTop = true;

    public Transform hexMapParent;  // Reference to the HexMap parent object in the hierarchy
    public Dictionary<Vector2, HexCell> cells = new Dictionary<Vector2, HexCell>();

    void Start()
    {
        InitializeGrid();
        AssignNeighbors();
    }

    private void InitializeGrid()
    {
        // Ensure hexMapParent is set; otherwise, create a default one
        if (hexMapParent == null)
        {
            GameObject hexMapObj = new GameObject("HexMap");
            hexMapParent = hexMapObj.transform;
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                Vector2 offsetCoords = new Vector2(x, z);

                // Create a HexCell game object and set it as a child of hexMapParent
                GameObject cellObj = new GameObject("HexCell");
                cellObj.transform.parent = hexMapParent;  // Parent to HexMap
                HexCell cell = cellObj.AddComponent<HexCell>();
                cell.Initialize(this, offsetCoords);

                // Store cell in dictionary for easy lookup
                cells[offsetCoords] = cell;
            }
        }
    }

    private void AssignNeighbors()
    {
        foreach (var cellEntry in cells)
        {
            Vector2 offsetCoords = cellEntry.Key;
            HexCell cell = cellEntry.Value;

            Vector2[] neighborOffsets = GetNeighborOffsets((int)offsetCoords.y);
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

    private Vector2[] GetNeighborOffsets(int row)
    {
        return row % 2 == 0
            ? new Vector2[] { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 1),
                              new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, -1) }
            : new Vector2[] { new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                              new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1) };
    }

    public Vector2 GetFlatTopHexCoords(int x, int z)
    {
        float xPos = x * tileSizeX * Mathf.Cos(Mathf.Deg2Rad * 30);
        float zPos = z * tileSizeZ + ((x % 2 == 1) ? tileSizeZ * 0.5f : 0);
        return new Vector2(xPos, zPos);
    }

    public Vector2 GetPointyTopHexCoords(int x, int z)
    {
        float xPos = x * tileSizeX + ((z % 2 == 1) ? tileSizeX * 0.5f : 0);
        float zPos = z * tileSizeZ * Mathf.Cos(Mathf.Deg2Rad * 30);
        return new Vector2(xPos, zPos);
    }
}
