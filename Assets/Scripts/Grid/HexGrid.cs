// Scripts/Grid/HexGrid.cs
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float tileSizeX = 2f;
    public float tileSizeZ = 2f;
    public bool UseFlatTopOrientation;  // true for flat-top, false for pointy-top

    public Transform hexMapParent;
    public Dictionary<Vector2, HexCell> cells = new Dictionary<Vector2, HexCell>();

    void Start()
    {
        InitializeGrid();
        AssignNeighbors();
    }

    private void InitializeGrid()
    {
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
                GameObject cellObj = new GameObject($"HexCell_{x}_{z}");
                cellObj.transform.parent = hexMapParent;
                
                HexCell cell = cellObj.AddComponent<HexCell>();
                cell.Initialize(this, offsetCoords);
                cells[offsetCoords] = cell;
            }
        }
    }

    private void AssignNeighbors()
    {
        foreach (var cellEntry in cells)
        {
            HexCell cell = cellEntry.Value;
            Vector2[] neighborOffsets = GetNeighborOffsets((int)cell.OffsetCoordinates.y);

            foreach (Vector2 offset in neighborOffsets)
            {
                Vector2 neighborCoords = cell.OffsetCoordinates + offset;
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
            ? new Vector2[] { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 1), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, -1) }
            : new Vector2[] { new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1) };
    }
}
