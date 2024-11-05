// HexCell.cs
using UnityEngine;
using System.Collections.Generic;

public class HexCell : MonoBehaviour
{
    public HexGrid Grid { get; private set; }
    public float HexSize { get; private set; }
    public HexTileData TerrainType { get; set; }
    public Vector2 OffsetCoordinates { get; private set; }
    public Vector3 CubeCoordinates { get; private set; }
    public Vector2 AxialCoordinates { get; private set; }
    public List<HexCell> Neighbors { get; private set; } = new List<HexCell>();
    public GameObject VisualRepresentation { get; set; } // Holds the reference to the instantiated visual hex prefab

    public void Initialize(HexGrid grid, Vector2 offsetCoords)
    {
        Grid = grid;
        OffsetCoordinates = offsetCoords;
        AxialCoordinates = OffsetToAxial(offsetCoords);
        CubeCoordinates = AxialToCube(AxialCoordinates);
    }

    private Vector2 OffsetToAxial(Vector2 offsetCoords)
    {
        float q = offsetCoords.x - (offsetCoords.y - ((int)offsetCoords.y & 1)) / 2f;
        float r = offsetCoords.y;
        return new Vector2(q, r);
    }

    private Vector3 AxialToCube(Vector2 axialCoords)
    {
        float x = axialCoords.x;
        float z = axialCoords.y;
        float y = -x - z;
        return new Vector3(x, y, z);
    }

    private Vector2 CubeToAxial(Vector3 cubeCoords)
    {
        float q = cubeCoords.x;
        float r = cubeCoords.z;
        return new Vector2(q, r);
    }

    private Vector2 AxialToOffset(Vector2 axialCoords)
    {
        float col = axialCoords.x + (axialCoords.y - ((int)axialCoords.y & 1)) / 2f;
        float row = axialCoords.y;
        return new Vector2(col, row);
    }

    private Vector2 CubeToOffset(Vector3 cubeCoords)
    {
        Vector2 axial = CubeToAxial(cubeCoords);
        return AxialToOffset(axial);
    }

    public void AddNeighbor(HexCell neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
            neighbor.Neighbors.Add(this);
        }
    }
}
