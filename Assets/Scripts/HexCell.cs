using System.Collections.Generic;
using UnityEngine;

public class HexCell
{
    public HexGrid Grid { get; private set; }
    public float HexSize { get; private set; }
    public HexTileData TerrainType { get; private set; }
    public Vector2 OffsetCoordinates { get; private set; }
    public Vector3 CubeCoordinates { get; private set; }
    public Vector2 AxialCoordinates { get; private set; }
    public List<HexCell> Neighbors { get; private set; } = new List<HexCell>();

    public HexCell(HexGrid grid, Vector2 offsetCoords, HexTileData terrainType)
    {
        Grid = grid;
        OffsetCoordinates = offsetCoords;
        TerrainType = terrainType;
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

    private Vector2 CubeToOffset(Vector3 cubeCoords)
    {
        float col = cubeCoords.x + (cubeCoords.z - ((int)cubeCoords.z & 1)) / 2f;
        float row = cubeCoords.z;
        return new Vector2(col, row);
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
