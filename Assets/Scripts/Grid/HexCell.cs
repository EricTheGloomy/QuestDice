// Scripts/Grid/HexCell.cs
using UnityEngine;
using System.Collections.Generic;

public class HexCell : MonoBehaviour
{
    public HexGrid Grid { get; private set; }  // Reference to the HexGrid this cell belongs to
    public HexTileData TerrainType { get; set; }
    public Vector2 OffsetCoordinates { get; private set; }
    public Vector3 CubeCoordinates { get; private set; }
    public List<HexCell> Neighbors { get; private set; } = new List<HexCell>();
    public GameObject VisualRepresentation { get; set; }

    // Updated Initialize to accept both grid and offset coordinates as parameters
    public void Initialize(HexGrid grid, Vector2 offsetCoords)
    {
        Grid = grid;
        OffsetCoordinates = offsetCoords;
        CubeCoordinates = HexCoordinateHelper.AxialToCube(HexCoordinateHelper.OffsetToAxial(offsetCoords));
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
