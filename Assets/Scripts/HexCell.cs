using System.Collections.Generic;
using UnityEngine;

public class HexCell
{
    public HexGrid Grid { get; private set; }          // Reference to the HexGrid this cell belongs to
    public float HexSize { get; private set; }         // Size of the hex cell
    public TerrainData TerrainType { get; private set; } // Terrain data reference for this cell

    public Vector2 OffsetCoordinates { get; private set; } // Offset coordinates on the grid
    public Vector3 CubeCoordinates { get; private set; }   // Cube coordinates for 3D hex calculations
    public Vector2 AxialCoordinates { get; private set; }  // Axial coordinates for 2D hex layout

    public List<HexCell> Neighbors { get; private set; }   // List of neighboring cells

    // Is Constructor needed???

    // Method to add a neighbor to this cell
    public void AddNeighbor(HexCell neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
            neighbor.Neighbors.Add(this); // Also add this cell as a neighbor of the other cell
        }
    }
}
